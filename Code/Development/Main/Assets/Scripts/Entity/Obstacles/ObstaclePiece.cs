//-----------------------------------------------------------------------------
// File: ObstaclePiece.cs
//
// Desc:	Script to be placed on the parent object of a hierarchy of 
//			obstacle game objects.
//
//			ObstaclePieces have a distance offset so that they can be scrolled
//			as the player moves through the course and should be a minimum of
//			20 metres wide so no more than 2 obstacle pieces are visible at 
//			any time.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObstaclePiece : MonoBehaviour
{
	public float m_Width = 20.0f;
	public float m_Distance = 0.0f;
	
	void Update()
	{
		if( !Application.isPlaying )
			DebugRender();
	}
	
	public void DebugRender()
	{
		//	Render the piece bounds
		float distanceOffset = m_Distance - RL.m_Sequencer.m_MasterDistance; 
		
		DebugRenderHelpers.DrawLine( new Vector3( distanceOffset, -20.0f, 0.0f ),
			new Vector3( distanceOffset, 40.0f, 0.0f ), 
			new Color( 1.0f, 0.0f, 1.0f, 0.7f ) );

		DebugRenderHelpers.DrawLine( new Vector3( distanceOffset + m_Width, -20.0f, 0.0f ),
			new Vector3( distanceOffset + m_Width, 40.0f, 0.0f ), 
			new Color( 1.0f, 0.0f, 1.0f, 0.7f ) );
	}
}
