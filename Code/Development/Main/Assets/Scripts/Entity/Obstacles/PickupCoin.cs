//-----------------------------------------------------------------------------
// File: PickupCoin.cs
//
// Desc: 	Monobehaviour for coin objects. GameObjects in a scene should 
//			have one of these attached. They are found by ObstaclePiece objects
//			when they do a scene scan.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class PickupCoin : Obstacle
{
	public bool m_Standard = true;
	float m_radius = 0.35f;
	
	bool m_collected = false;
	float m_animFrame = 0.0f;
	
	public void OnEnable() 
	{
		if( m_Standard )
			m_radius = 0.35f;
		else
			m_radius = 0.5f;
		
		if( Application.isPlaying )
		{
			m_collected = false;
			m_animFrame = 1.0f/20.0f;
			
			if( m_QuadRenderer != null )
				Debug.Log( "!** Showing an already visible KillCircle" );
				
			m_QuadRenderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.COINS_BATCH );
			
			m_QuadRenderer.m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			m_QuadRenderer.m_Position = transform.position;
			m_QuadRenderer.m_Rotation = 0.0f;
			m_QuadRenderer.m_Scale = new Vector2( m_radius*2.0f, m_radius*2.0f );
			m_QuadRenderer.m_TextureIdx = (int)(transform.position.x/2.0f + transform.position.y/3.0f);
			
			while( m_QuadRenderer.m_TextureIdx < 0 )
				m_QuadRenderer.m_TextureIdx += 15;
			
			while( m_QuadRenderer.m_TextureIdx > 15 )
				m_QuadRenderer.m_TextureIdx -= 15;
			
			if( !m_Standard )
				m_QuadRenderer.m_TextureIdx += 16;
		}
	}
	
	public override void DebugRender()
	{
		if( m_Standard )
			DebugRenderHelpers.DrawFilledCircle( transform.position, 0.35f, new Color( 0.8f, 0.8f, 0.8f, 1.0f ) );
		else
			DebugRenderHelpers.DrawFilledCircle( transform.position, 0.5f, new Color( 1.0f, 0.75f, 0.0f, 1.0f ) );
	}
	
	public override void CollideWithPlayer()
	{
		if( m_collected )
			return;
		
		Vector3 currentPos = transform.position;		
		PlayerCollisionDef playerBox = RL.m_Player.m_CollisionBox;
		
		
		//	We're assuming the box is axis aligned here...
		Vector3 box_center = playerBox.m_CollisionBoxBase;
		box_center.y += playerBox.m_CollisionBoxDimensions.y*0.5f;
		
		float box_width = playerBox.m_CollisionBoxDimensions.x;
		float box_height = playerBox.m_CollisionBoxDimensions.y*1.75f;
		
		//	Find the closest point within the rectangle to the center of the circle
		float closestX = Mathf.Clamp( currentPos.x, box_center.x - box_width*0.5f, box_center.x + box_width*0.5f );
		float closestY = Mathf.Clamp( currentPos.y, box_center.y - box_height*0.5f, box_center.y + box_height*0.5f );
		
		//	Now find the squared distance between this point and the circle center
		float xDist = closestX - currentPos.x;
		xDist *= xDist;
		float yDist = closestY - currentPos.y;
		yDist *= yDist;
		float sqDist = xDist + yDist;
		
		//	If this is less than the squared radius, then the box penetrates the circle
		//	Allow a little bit of wiggle room
		float radius = m_radius - 0.1f;
		if( sqDist > radius*radius )
			return;
		
		//	So that's a collision, set ourselves on the player
		m_collected = true;
		PrimitiveLibrary.Get.ReleaseQuadDefinition( m_QuadRenderer );
		m_QuadRenderer = null;
		
		if( m_Standard )
		{
			RL.m_Player.m_Coins += 1;
			//Debug.Log ("Pickup standard coin");
			// Pickup sound
			//RL.m_SoundController.Play("Pickup_01");
			RL.m_SoundController.PlaySting("Pickup_01");
		}
		else
		{
			RL.m_Player.m_Coins += 100;
			//Debug.Log ("Pickup super coin");
			// Pickup sound
			RL.m_SoundController.Play("Pickup_02");
		}
	}
	
	
	
	public override void ObstacleUpdate() 
	{
		if( m_collected )
			return;
		
		m_animFrame -= Time.deltaTime;
		
		if( m_animFrame < 0.0f )
		{			
			m_QuadRenderer.m_TextureIdx++;
			
			if( m_Standard )
			{
				m_animFrame = 1.0f/20.0f;
				
				if( m_QuadRenderer.m_TextureIdx > 15 )
					m_QuadRenderer.m_TextureIdx = 0;
			}
			else
			{
				m_animFrame = 1.0f/10.0f;
				
				if( m_QuadRenderer.m_TextureIdx > 31 )
					m_QuadRenderer.m_TextureIdx = 16;
			}
		}
	}
}
