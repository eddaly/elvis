//-----------------------------------------------------------------------------
// File: BackgroundPiece.cs
//
// Desc:	Script applied to the parent object of any of the different
//			types of background pieces.
//
//			Provides information about the dimensions of the piece and a place
//			for the game management objects to manipulate the piece itself.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EnvironmentPiece : MonoBehaviour 
{
	public float m_Width = 20.0f;
	
	public float m_Distance = 0.0f;
	public float m_yOffset = 0.0f;

	public EntityDefs.EnvLayer m_Type;
	
	public void DebugRender()
	{
		//	Render the piece bounds
		float distanceOffset = m_Distance - RL.m_Sequencer.m_MasterDistance 
			+ transform.parent.transform.position.x; 
		
		float red = 0.8f;
		if( m_Type == EntityDefs.EnvLayer.SKYBOX )
			red = 0.0f;
		if( m_Type == EntityDefs.EnvLayer.BACKGROUND )
			red = 0.2f;
		if( m_Type == EntityDefs.EnvLayer.MIDGROUND )
			red = 0.4f;
		if( m_Type == EntityDefs.EnvLayer.FOREGROUND )
			red = 0.6f;
		
		DebugRenderHelpers.DrawLine( new Vector3( distanceOffset, -20.0f, transform.position.z ),
			new Vector3( distanceOffset, 40.0f, transform.position.z ), 
			new Color( red, 0.0f, 1.0f, 0.7f ) );
		
		DebugRenderHelpers.DrawLine( new Vector3( distanceOffset + m_Width, -20.0f, transform.position.z ),
			new Vector3( distanceOffset + m_Width, 40.0f, transform.position.z ), 
			new Color( red, 0.0f, 1.0f, 0.7f ) );
	}
	
	
	void Start() 
	{
	}
	
	void Update() 
	{	
	}
}
