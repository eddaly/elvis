//-----------------------------------------------------------------------------
// File: ObstacleKillBox.cs
//
// Desc: 	Monobehaviour for killbox objects. GameObjects in a scene should 
//			have one of these attached. They are found by ObstaclePiece objects
//			when they do a scene scan.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ObstacleKillBox : Obstacle
{
	public float m_Width = 1.0f;
	public float m_Height = 1.0f;

	public void OnEnable() 
	{
		if( Application.isPlaying )
		{
			if( m_QuadRenderer != null )
				Debug.Log( "!** Showing an already visible KillBox" );
				
			m_QuadRenderer = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.OBSTACLE_BATCH );
			
			m_QuadRenderer.m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			m_QuadRenderer.m_Position = transform.position;
			m_QuadRenderer.m_Rotation = 0.0f;
			m_QuadRenderer.m_Scale = new Vector2( m_Width, m_Height );
			m_QuadRenderer.m_TextureIdx = (int)TextureAtlas.Obstacles.KILLBOX;
		}
	}	
		 
	public override void DebugRender()
	{
		if( m_HighlightColour )
		{
			DebugRenderHelpers.DrawFilled2DBoxC( EditModeHelpers.WorldPosition( transform ), 
				m_Width, m_Height, new Color( 1.0f, 0.0f, 0.0f, 1.0f ) );
			m_HighlightColour = false;
		}
		else
		{
			DebugRenderHelpers.DrawFilled2DBoxC( EditModeHelpers.WorldPosition( transform ),
				m_Width, m_Height, new Color( 1.0f, 1.0f, 0.0f, 1.0f ) );
		}
	}
	
	public override void CollideWithPlayer()
	{
		Vector3 currentPos = transform.position;
		
		PlayerCollisionDef playerBox = RL.m_Player.m_CollisionBox;
		
		//	Axis aligned boxes again...
		float halfMyWidth = m_Width*0.5f;
		float halfMyHeight = m_Height*0.5f;
		float halfBoxWidth = playerBox.m_CollisionBoxDimensions.x*0.5f;
		
		if( RL.m_Prototype.m_CollisionType == PrototypeConfiguration.CollisionTypes.FRONT_COLLISION )
		{
			//	Only collide with front section of box
			currentPos.x -= halfMyWidth;
			halfMyWidth = 0.0f;
		}
		
		//	Process of elimination - if box is too far to the right on x axis there's no collision
		if( currentPos.x + halfMyWidth < playerBox.m_CollisionBoxBase.x - halfBoxWidth )		
			return;
		
		//	Too far to the left
		if( currentPos.x - halfMyWidth > playerBox.m_CollisionBoxBase.x + halfBoxWidth )		
			return;

		//	Too far down on the y axis
		if( currentPos.y + halfMyHeight < playerBox.m_CollisionBoxBase.y )	
			return;
		
		//	Too far up
		if( currentPos.y - halfMyHeight > playerBox.m_CollisionBoxBase.y + playerBox.m_CollisionBoxDimensions.y )	
			return;
		
		//	If we got this far, then there's some overlap, so set the collision
		playerBox.m_KillCollision = this;
	}
}
