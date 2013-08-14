//-----------------------------------------------------------------------------
// File: ObstaclePlatform.cs
//
// Desc: 	Monobehaviour for platform objects. GameObjects in a scene should 
//			have one of these attached. They are found by ObstaclePiece objects
//			when they do a scene scan.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ObstaclePlatform : Obstacle
{
	public float m_Width = 1.0f;
	public bool m_PassThrough = false;

	public void OnEnable() 
	{
		if( Application.isPlaying )
		{
			if( m_QuadRenderer != null )
				Debug.Log( "!** Showing an already visible platform" );
				
			//	Only render platforms that you can pass through - based on the assumption that no 
			//	solid platforms float in mid air without an object beneath them
			if( m_PassThrough )
			{
				m_QuadRenderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.PLATFORM_BATCH );
				
				m_QuadRenderer.m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
				m_QuadRenderer.m_Position = transform.position;
				m_QuadRenderer.m_Rotation = 0.0f;
				m_QuadRenderer.m_Scale = new Vector2( m_Width, 0.2f );
				
//				if( m_PassThrough )
					m_QuadRenderer.m_TextureIdx = (int)TextureAtlas.Platforms.PASS_THROUGH;
//				else
//					m_QuadRenderer.m_TextureIdx = (int)TextureAtlas.Platforms.SOLID;
			}
		}
	}	
	
	public override void DebugRender()
	{
		Vector3 currentPos = transform.position;
		
		if( m_HighlightColour )
		{
			DebugRenderHelpers.DrawLine( currentPos - new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
				currentPos + new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
				new Color( 0.0f, 1.0f, 1.0f, 1.0f ) );
		
			m_HighlightColour = false;
		}
		else
			DebugRenderHelpers.DrawLine( currentPos - new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
				currentPos + new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
				new Color( 0.0f, 0.75f, 1.0f, 1.0f ) );
	}	

	public override void CollideWithPlayer()
	{
		Vector3 currentPos = transform.position;		
		PlayerCollisionDef playerBox = RL.m_Player.m_CollisionBox;
		

		//	Going to massively simplify this using the assumption that the player only moves
		//	up and down in y (not true of course).
		Vector3 boxBottom = playerBox.m_CollisionBoxBase;
		Vector3 boxVelocity = playerBox.m_CollisionBoxVelocity;

		//	Discard check if point is beyond our x scale
		if( boxBottom.x < currentPos.x - m_Width*0.5f )
			return;
		if( boxBottom.x > currentPos.x + m_Width*0.5f )
			return;
		
		//	Also discard velocities in positive y so we can jump through under platforms
		if( m_PassThrough && boxVelocity.y > 0.0f )
			return;
		
		//	So we coincide on the x-axis - do we penetrate on the y?
		if( boxBottom.y > currentPos.y && boxBottom.y + boxVelocity.y > currentPos.y )
			return;
		
		float thickness = 0.01f;
		if( boxBottom.y < currentPos.y - thickness && boxBottom.y - boxVelocity.y < currentPos.y - thickness )
			return;
		
		//	Collided, set the collision point to the top of the platform
		playerBox.m_PlatformCollisionPoint = new Vector3( boxBottom.x, currentPos.y, 0.0f );
		playerBox.m_PlatformCollision = this;

		
		/*
		//	Do line intersection on the line of movement of the center-bottom point on the box (call it x1, y1 to x2, y2)
		//	and the line of the platform (call it x3, y3 to x4, y3).
		//	Simplifying assumptions are that the platform is flat on the x-axis and that x3 < x4
		
		//	Shift the box line down so it's in 'platform-space' on the y-axis
		float x2 = box_center.x;
		float y2 = box_center.y - box_height*0.5f - currentPos.y;
		
		float x1 = x2 - movement_vector.x;
		float y1 = y2 - movement_vector.y;
		
		float x3 = currentPos.x - m_Width*0.5f;		
		float x4 = x3 + m_Width;		
		
		//	This is like an axis-aligned bounding box check now with the platform box being infinitely thin on the y-axis
		if( y1 < 0.0f && y2 < 0.0f )
			return false;
		if( y1 > 0.0f && y2 > 0.0f )
			return false;
		if( x1 < x3 && x2 < x3 )
			return false;
		if( x1 > x4 && x2 > x4 )
			return false;
		
		//	There are still cases left...
		//	Now find the normalised point of intersection using the gradient of the box line,
		//	testing when it crosses the y == 0 line
		float dy = Mathf.Abs( y2 - y1 );
		float normalIntersection = Mathf.Abs( y1 )/dy;
		
		//	Therefore find the actual point of intersection on the x-axis
		float intersectX = x1 + normalIntersection*(x2 - x1);
		
		//	See if this is within the platform line segment
		if( intersectX < x3 || intersectX > x4 )
			return false;
		
		//	We have an intersection, find the point and put it back in world-space
		collision_point.x = intersectX;
		collision_point.y = currentPos.y;
		collision_point.z = 0.0f;
		
		return true;
		*/
	}
}
