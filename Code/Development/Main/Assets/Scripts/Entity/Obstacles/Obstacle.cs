//-----------------------------------------------------------------------------
// File: Obstacle.cs
//
// Desc:	An individual obstacle. Essentially a collider and a renderer.
//
//			For derived classes, the managing object should set up the public
//			variables as they need before calling Initialise()
//
// Note:	Assumes m_Parent is not null - obstacles should always be part of
//			an obstacle piece.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Obstacle : MonoBehaviour
{
	public bool m_HighlightColour = false;
	
	public BatchedQuadDef m_QuadRenderer = null;

	
	void OnDisable() 
	{
		if( Application.isPlaying )
		{
			if( m_QuadRenderer != null )
			{
				PrimitiveLibrary.Get.ReleaseQuadDefinition( m_QuadRenderer );
				m_QuadRenderer = null;
			}
		}
	}

	void Update()
	{
		if( !Application.isPlaying )
		{
			DebugRender();
			
			Vector3 localPos = transform.localPosition;
			transform.localPosition = localPos;
		}
		else
		{
			CollideWithPlayer();
			
			if( RL.m_Sequencer.m_ShowObstacles )
				UpdateRenderer();
		}
	}

	public virtual void CollideWithPlayer() {}
	
	public virtual void DebugRender() {}
	
	public void UpdateRenderer()
	{
		if( m_QuadRenderer != null )
		{
			m_QuadRenderer.m_Position = transform.position;
		}
	}

	public void SetHighlight()
	{
		m_HighlightColour = true;
	}
}

public class PlayerCollisionDef
{
	public Vector3 m_CollisionBoxBase;
	public Vector3 m_CollisionBoxVelocity;
	
	public Vector2 m_CollisionBoxDimensions;	
	
	public Obstacle m_KillCollision;
	public Vector3 m_KillCollisionPoint;
	
	public ObstaclePlatform m_PlatformCollision;
	public Vector3 m_PlatformCollisionPoint;		
}
	
	