//-----------------------------------------------------------------------------
// File: Player.cs
//
// Desc:	Main class for the player entity.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	BatchedQuadDef m_Renderer;
	
	public enum PlayerAnimState
	{
		RUNNING = 0,
		JUMP_TAKE_OFF,
		JUMP_SAILING,
		JUMP_LANDING,
		JUMP_LANDED,
		
		NUM
	}
	public PlayerAnimState m_animState = PlayerAnimState.RUNNING;
	
	int m_animFrame = 0;
	float m_animFrameTime = 0.0f;
	float m_animFrameSpeed = 12.0f;
	public float m_animRunSpeed = 16.0f;
	float m_realAnimRunSpeed = 16.0f;
	
	float m_quadSize = 2.1f;
	float m_collisionWidth = 1.5f;
	float m_collisionHeight = 1.5f;
	
	float m_jumpVelocity = 0.0f;
	float m_jumpFloatVelocity = 5.0f;
	
	float m_fallTimer = 0.0f;
	
	float m_highAnalogueTimer = 0.0f;
	
	float m_gravity = -50.0f;
	
	Vector3 m_lastPosition;
	
	public bool m_DebugRender = false;
	
	public bool m_Colliding = false;
	
	public Vector3 m_Position;
	float m_yRendererOffset = 0.25f;

	public PlayerCollisionDef m_CollisionBox = new PlayerCollisionDef();
	
	void Start() 
	{
		if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
			m_Renderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.PLAYER_BATCH );
		else
			m_Renderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.ELVIS_BATCH );
		
		m_Position = new Vector3( 2.0f, 4.0f, 0.0f );
		m_Renderer.m_Position = m_Position + new Vector3( 0.0f, m_yRendererOffset, 0.0f );
		m_Renderer.m_Scale = new Vector3( m_quadSize, m_quadSize, m_quadSize );
		m_Renderer.m_TextureIdx = 0;
		
		m_lastPosition = new Vector3( 0.0f, 0.0f, 0.0f );
		
		m_realAnimRunSpeed = m_animRunSpeed;
	}
	
	void Update() 
	{	
		m_fallTimer -= Time.deltaTime;
		if( m_fallTimer < 0.0f )
			m_fallTimer = 0.0f;

		m_highAnalogueTimer -= Time.deltaTime;
		if( m_highAnalogueTimer < 0.0f )
			m_highAnalogueTimer = 0.0f;
		
		if( m_realAnimRunSpeed > m_animRunSpeed )
		{
			m_realAnimRunSpeed -= Time.deltaTime*40.0f;
			if( m_realAnimRunSpeed < m_animRunSpeed )
				m_realAnimRunSpeed = m_animRunSpeed;
		}
		if( m_realAnimRunSpeed < m_animRunSpeed )
		{
			m_realAnimRunSpeed += Time.deltaTime*40.0f;
			if( m_realAnimRunSpeed > m_animRunSpeed )
				m_realAnimRunSpeed = m_animRunSpeed;
		}
		
		InputManager.Get.Update();
		
		animatePlayer();	
		
		const float baseJumpVel = 16.0f;
		const float baseDropVel = 10.0f;
		
		//	Maybe do a jump while running
		if( InputManager.Get.m_FirePressed[0] && m_animState == PlayerAnimState.RUNNING )
		{
			changeState( PlayerAnimState.JUMP_TAKE_OFF );
			
			if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.FAST_DIGITAL )
				m_jumpVelocity = 45.0f;
			else if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.DIGITAL )
				m_jumpVelocity = baseJumpVel;
			else if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.HIGH_ANALOGUE )
			{
				m_highAnalogueTimer = 0.45f;
				m_jumpVelocity = baseJumpVel;
			}
			else if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.ANALOGUE )
			{
				m_highAnalogueTimer = 0.27f;
				m_jumpVelocity = baseJumpVel;
			}
		}

		//	Maybe do a drop while in the air
		if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.HIGH_ANALOGUE )
		{
			if( InputManager.Get.m_FireDown[0] && m_animState != PlayerAnimState.RUNNING && m_highAnalogueTimer > 0.0f && m_highAnalogueTimer < 0.4f  )
			{
				m_jumpVelocity += Time.deltaTime*60.0f;
			}
			
			if( InputManager.Get.m_FireDown[1] && m_Position.y > 1.0f && m_jumpVelocity > -baseDropVel &&
				(m_animState == PlayerAnimState.JUMP_SAILING || m_animState == PlayerAnimState.JUMP_LANDING) )
			{
				m_jumpVelocity = -baseDropVel;
			}
		}
		else if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.ANALOGUE )
		{
			if( InputManager.Get.m_FireDown[0] && m_animState != PlayerAnimState.RUNNING && m_highAnalogueTimer > 0.0f && m_highAnalogueTimer < 0.17f )
			{
				m_jumpVelocity += Time.deltaTime*60.0f;
			}
			
			if( InputManager.Get.m_FireDown[1] && m_Position.y > 1.0f &&	m_jumpVelocity > -baseDropVel &&
				(m_animState == PlayerAnimState.JUMP_SAILING || m_animState == PlayerAnimState.JUMP_LANDING) )
			{
				m_jumpVelocity = -baseDropVel;
			}
		}
		else
		{
			if( InputManager.Get.m_FireDown[1] && m_Position.y > 1.0f && m_jumpVelocity > -baseDropVel &&
				(m_animState == PlayerAnimState.JUMP_SAILING || m_animState == PlayerAnimState.JUMP_LANDING) )
			{
				m_jumpVelocity = -baseDropVel;
			}
		}
		
		//	Maybe try to fall down a level while running
		if( InputManager.Get.m_FirePressed[1] && m_animState == PlayerAnimState.RUNNING && 
			m_Position.y > 1.0f )
		{
			bool canPassThrough = true;
			
			if( m_CollisionBox.m_PlatformCollision )
			{
				if( !m_CollisionBox.m_PlatformCollision.m_PassThrough )
					canPassThrough = false;
			}
				
			if( canPassThrough )
			{
				changeState( PlayerAnimState.JUMP_SAILING );
				m_fallTimer = 0.1f;
	
				if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.FAST_DIGITAL )
					m_jumpVelocity = -8.0f;
			}
		}
		
		
		if( m_animState != PlayerAnimState.RUNNING && m_animState != PlayerAnimState.JUMP_LANDED )
		{
			Vector3 position = m_Position;
			position.y += m_jumpVelocity*Time.deltaTime;
			
			if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.FAST_DIGITAL )
			{
				if( m_jumpVelocity > 0.0f )
				{
					m_jumpVelocity += (m_gravity*10)*Time.deltaTime;
					if( m_jumpVelocity < 0.0f )
						m_jumpVelocity = 0.0f;
				}
				else
					m_jumpVelocity += (m_gravity*0.5f)*Time.deltaTime;
			}
			else
				m_jumpVelocity += m_gravity*Time.deltaTime;
						
			m_Position = position;
		}
		
		if( m_DebugRender )
		{
			DebugRenderHelpers.Draw2DBoxC( m_Position, 
				m_collisionWidth, m_collisionHeight,
				new Color( 1.0f, 1.0f, 1.0f, 0.75f ) );			
		}
		
		
		
		//	Set collision box ready for obstacles to check for collision.
		//	Note that player.cs is set to execute before all Obstacle scripts
		m_CollisionBox.m_CollisionBoxBase = m_Position;
		m_CollisionBox.m_CollisionBoxBase.y -= m_collisionHeight*0.5f;
		
		m_CollisionBox.m_CollisionBoxVelocity = m_Position - m_lastPosition;
		Debug.Log( "vel " + m_CollisionBox.m_CollisionBoxVelocity.ToString() );
		
		m_CollisionBox.m_CollisionBoxDimensions.x = m_collisionWidth;
		m_CollisionBox.m_CollisionBoxDimensions.y = m_collisionHeight;
		
		m_CollisionBox.m_KillCollision = null;
		m_CollisionBox.m_PlatformCollision = null;		
	}
	
	void LateUpdate()
	{
		//	Respond to collisions that happened this frame
		if( m_CollisionBox.m_KillCollision != null )
		{
			m_CollisionBox.m_KillCollision.SetHighlight();
			m_Colliding = true;			
		}
		else
			m_Colliding = false;
		
		if( m_fallTimer > 0.0f )
			m_CollisionBox.m_PlatformCollision = null;
		
		//	If there's no platform beneath us when we're running, then fall
		if( m_CollisionBox.m_PlatformCollision == null )
		{
			if( m_animState == PlayerAnimState.RUNNING )
			{
				changeState( PlayerAnimState.JUMP_SAILING );
				
				if( RL.m_Prototype.m_JumpType == PrototypeConfiguration.JumpTypes.FAST_DIGITAL )
					m_jumpVelocity = -8.0f;
			}
		}
		else
		{
			m_Position.y = m_CollisionBox.m_PlatformCollisionPoint.y + m_collisionHeight*0.5f;
			m_CollisionBox.m_PlatformCollision.SetHighlight();

			if( m_animState != PlayerAnimState.RUNNING && m_animState != PlayerAnimState.JUMP_LANDED )
			{
				m_jumpVelocity = 0.0f;
				changeState( PlayerAnimState.JUMP_LANDED );
			}
		}
		
		//	Set up for next frame
		m_lastPosition = m_Position;
		m_Renderer.m_Position = m_Position;
		m_Renderer.m_Position.y += m_yRendererOffset;
	}
	
	void animatePlayer()
	{
		m_animFrameTime -= Time.deltaTime;
		
		switch( m_animState )
		{
		case PlayerAnimState.RUNNING:
			if( m_animFrameTime < 0.0f )
			{
				m_animFrame++;
				m_animFrameTime = 1.0f/m_realAnimRunSpeed;
				
				if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
				{
					if( m_animFrame > 11 )
						m_animFrame = 0;
				}
				else
				{
					if( m_animFrame > 10 )
						m_animFrame = 0;
				}
			}
			break;
		
		case PlayerAnimState.JUMP_TAKE_OFF:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
			{
				if( m_animFrameTime < 0.0f && m_animFrame == 16 )
					m_animFrame = 17;
			}

			if( Mathf.Abs( m_jumpVelocity ) < m_jumpFloatVelocity )
				changeState( PlayerAnimState.JUMP_SAILING );
			break;
			
		case PlayerAnimState.JUMP_SAILING:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.ELVIS )
			{
				if( m_animFrameTime < 0.0f )
				{
					m_animFrame++;
					m_animFrameTime = 1.0f/m_realAnimRunSpeed;
					
					if( m_animFrame > 15 )
						m_animFrame = 12;
				}
			}
			
			if( Mathf.Abs( m_jumpVelocity ) > m_jumpFloatVelocity )
				changeState( PlayerAnimState.JUMP_LANDING );
			break;
			
		case PlayerAnimState.JUMP_LANDING:
			//	We wait until collision switches the state here
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
			{
				if( m_animFrameTime < 0.0f && m_animFrame == 19 )
					m_animFrame = 20;
			}
			break;
			
		case PlayerAnimState.JUMP_LANDED:
			if( m_animFrameTime < 0.0f )
				changeState( PlayerAnimState.RUNNING );
			break;
		}
		
		m_Renderer.m_TextureIdx = m_animFrame;
	}
	
	void changeState( PlayerAnimState new_state )
	{
		m_animState = new_state;
		
		
		switch( m_animState )
		{
		case PlayerAnimState.RUNNING:
			m_animFrame = 0;
			m_animFrameTime = 1.0f/m_realAnimRunSpeed;
			break;
		
		case PlayerAnimState.JUMP_TAKE_OFF:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
				m_animFrame = 16;
			else
				m_animFrame = 11;
			
			m_animFrameTime = 1.0f/m_animFrameSpeed;
			break;
			
		case PlayerAnimState.JUMP_SAILING:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
				m_animFrame = 18;
			else
			{
				m_animFrameTime = 1.0f/m_animFrameSpeed;
				m_animFrame = 12;
			}
			
			break;
			
		case PlayerAnimState.JUMP_LANDING:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
				m_animFrame = 19;
			else
				m_animFrame = 16;
			
			m_animFrameTime = 2.0f/m_animFrameSpeed;
			break;
			
		case PlayerAnimState.JUMP_LANDED:
			if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
				m_animFrame = 19;
			else
				m_animFrame = 16;
			
			m_animFrameTime = 0.2f/m_animFrameSpeed;
			
			m_realAnimRunSpeed = m_animRunSpeed + 15.0f;
			break;
		}
	}
}
