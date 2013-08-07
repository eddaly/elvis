//-----------------------------------------------------------------------------
// File: ObstacleKillCircle.cs
//
// Desc: 	Monobehaviour for circlebox objects. GameObjects in a scene should 
//			have one of these attached. They are found by ObstaclePiece objects
//			when they do a scene scan.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ObstacleKillCircle : Obstacle
{
	public float m_Radius = 1.0f;
	
	public void OnEnable() 
	{
		if( Application.isPlaying )
		{
			if( m_QuadRenderer != null )
				Debug.Log( "!** Showing an already visible KillCircle" );
				
			m_QuadRenderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.OBSTACLE_BATCH );
			
			m_QuadRenderer.m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			m_QuadRenderer.m_Position = transform.position;
			m_QuadRenderer.m_Rotation = 0.0f;
			m_QuadRenderer.m_Scale = new Vector2( m_Radius*2.0f, m_Radius*2.0f );
			m_QuadRenderer.m_TextureIdx = (int)TextureAtlas.Obstacles.KILLCIRCLE;
		}
	}
	
	public override void DebugRender()
	{
		if( m_HighlightColour )
		{
			DebugRenderHelpers.DrawFilledCircle( transform.position, m_Radius, new Color( 1.0f, 0.0f, 0.0f, 1.0f ) );
			m_HighlightColour = false;
		}
		else
		{
			DebugRenderHelpers.DrawFilledCircle( transform.position, m_Radius, new Color( 1.0f, 1.0f, 0.0f, 1.0f ) );
		}
	}
	
	public override void CollideWithPlayer()
	{
		Vector3 currentPos = transform.position;		
		PlayerCollisionDef playerBox = RL.m_Player.m_CollisionBox;
		
		
		//	We're assuming the box is axis aligned here...
		Vector3 box_center = playerBox.m_CollisionBoxBase;
		box_center.y += playerBox.m_CollisionBoxDimensions.y*0.5f;
		
		float box_width = playerBox.m_CollisionBoxDimensions.x;
		float box_height = playerBox.m_CollisionBoxDimensions.y;
		
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
		float radius = m_Radius - 0.1f;
		if( sqDist > radius*radius )
			return;
		
		//	So that's a collision, set ourselves on the player
		playerBox.m_KillCollision = this;
	}
}
