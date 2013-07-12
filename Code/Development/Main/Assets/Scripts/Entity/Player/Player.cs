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
	
	enum PlayerAnimState
	{
		RUNNING = 0,
		JUMP_TAKE_OFF,
		JUMP_SAILING,
		JUMP_LANDING,
		JUMP_LANDED,
		
		NUM
	}
	PlayerAnimState m_animState = PlayerAnimState.RUNNING;
	
	int m_animFrame = 0;
	float m_animFrameTime = 0.0f;
	float m_animFrameSpeed = 12.0f;
	public float m_animRunSpeed = 16.0f;
	float m_realAnimRunSpeed = 16.0f;
	
	float m_playerHeight = 2.5f;
	float m_playerCollisionRatio = 0.75f;
	
	float m_jumpVelocity = 0.0f;
	float m_jumpFloatVelocity = 5.0f;
	
	float m_fallTimer = 0.0f;
	
	float m_gravity = -50.0f;
	
	Vector3 m_lastPosition;
	
	public bool m_DebugRender = false;
	
	
	void Start() 
	{
		m_Renderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.PLAYER_BATCH );
		
		m_Renderer.m_Position = new Vector3( 2.0f, 4.0f, 0.0f );
		m_Renderer.m_Scale = new Vector3( m_playerHeight, m_playerHeight, m_playerHeight );
		m_Renderer.m_TextureIdx = 0;
		
		m_lastPosition = new Vector3( 0.0f, 0.0f, 0.0f );
		
		m_realAnimRunSpeed = m_animRunSpeed;
	}
	
	void Update() 
	{	
		m_fallTimer -= Time.deltaTime;
		if( m_fallTimer < 0.0f )
			m_fallTimer = 0.0f;
		
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
		
		if( InputManager.Get.m_FireDown[0] && m_animState == PlayerAnimState.RUNNING )
		{
			changeState( PlayerAnimState.JUMP_TAKE_OFF );
			m_jumpVelocity = 16.0f;
		}
		
		if( InputManager.Get.m_FireDown[1] && m_animState == PlayerAnimState.RUNNING && 
			m_Renderer.m_Position.y > 1.0f )
		{
			changeState( PlayerAnimState.JUMP_SAILING );
			m_fallTimer = 0.1f;
		}
		
		if( m_animState != PlayerAnimState.RUNNING && m_animState != PlayerAnimState.JUMP_LANDED )
		{
			Vector3 position = m_Renderer.m_Position;
			position.y += m_jumpVelocity*Time.deltaTime;
			
			m_jumpVelocity += m_gravity*Time.deltaTime;
						
			m_Renderer.m_Position = position;
		}
		
		if( m_DebugRender )
		{
			DebugRenderHelpers.Draw2DBoxC( m_Renderer.m_Position, 
				m_playerHeight*m_playerCollisionRatio, m_playerHeight*m_playerCollisionRatio,
				new Color( 1.0f, 1.0f, 1.0f, 0.75f ) );			
		}
		
		Obstacle collided = ReferenceLibrary.m_SequenceManager.CollideWithBox( m_Renderer.m_Position, 
			m_playerHeight*m_playerCollisionRatio, m_playerHeight*m_playerCollisionRatio );
		
		if( collided != null )
			collided.SetHighlight();
		
		Vector3 velocity = m_Renderer.m_Position - m_lastPosition;
		Vector3 platformCollisionPoint = new Vector3( 0.0f, 0.0f, 0.0f );
		
		Obstacle platform = ReferenceLibrary.m_SequenceManager.PlatformCollide( m_Renderer.m_Position,
			m_playerHeight*m_playerCollisionRatio, velocity, ref platformCollisionPoint );
		
		if( m_fallTimer > 0.0f )
			platform = null;
		
		//	If there's no platform beneath us when we're running, then fall
		if( platform == null )
		{
			if( m_animState == PlayerAnimState.RUNNING )
			{
				changeState( PlayerAnimState.JUMP_SAILING );
			}
		}
		else
		{
			m_Renderer.m_Position.y = platformCollisionPoint.y + m_playerHeight*m_playerCollisionRatio*0.5f;
			platform.SetHighlight();

			if( m_animState != PlayerAnimState.RUNNING && m_animState != PlayerAnimState.JUMP_LANDED )
			{
				m_jumpVelocity = 0.0f;
				changeState( PlayerAnimState.JUMP_LANDED );
			}
		}
		
		m_lastPosition = m_Renderer.m_Position;
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
				
				if( m_animFrame > 11 )
					m_animFrame = 0;
			}
			break;
		
		case PlayerAnimState.JUMP_TAKE_OFF:
			if( m_animFrameTime < 0.0f && m_animFrame == 16 )
				m_animFrame = 17;

			if( Mathf.Abs( m_jumpVelocity ) < m_jumpFloatVelocity )
				changeState( PlayerAnimState.JUMP_SAILING );
			break;
			
		case PlayerAnimState.JUMP_SAILING:
			if( Mathf.Abs( m_jumpVelocity ) > m_jumpFloatVelocity )
				changeState( PlayerAnimState.JUMP_LANDING );
			break;
			
		case PlayerAnimState.JUMP_LANDING:
			//	We wait until collision switches the state here
			if( m_animFrameTime < 0.0f && m_animFrame == 19 )
				m_animFrame = 20;
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
			m_animFrame = 16;
			m_animFrameTime = 1.0f/m_animFrameSpeed;
			break;
			
		case PlayerAnimState.JUMP_SAILING:
			m_animFrame = 18;
			break;
			
		case PlayerAnimState.JUMP_LANDING:
			m_animFrame = 19;
			m_animFrameTime = 2.0f/m_animFrameSpeed;
			break;
			
		case PlayerAnimState.JUMP_LANDED:
			m_animFrame = 19;
			m_animFrameTime = 0.2f/m_animFrameSpeed;
			
			m_realAnimRunSpeed = m_animRunSpeed + 15.0f;
			break;
		}
	}
}
