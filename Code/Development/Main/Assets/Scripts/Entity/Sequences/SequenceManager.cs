//-----------------------------------------------------------------------------
// File: SequenceManager.cs
//
// Desc:	Controls the various pieces and sets their distances
//
//			The intention is that the SequenceManage has no real idea about
//			edit mode and play mode. It arranges the pieces as it is told to
//			do so by the ElvisEditor or ElvisGame objects. This means 
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SequenceManager : MonoBehaviour 
{
	public float m_MasterDistance = 0.0f;
	public float m_MetersPerSecond = 8.0f;

	
	//	The are 2 active pieces for all layers - piece 0 is the one on
	//	screen at any time and piece 1 is the one to the right that is either
	//	waiting to come on or overlaps into the viewable area. 
	//	When piece 0 scrolls left enough so that it is no longer on screen then
	//	piece 1 shuffles into the piece 0 slot and a new piece 1 is requested
	//	from the track makers.
	
	
	//	For choosing the various layers and pieces to be viewed in edito mode:
	public bool m_ShowObstacles = true;
	public int[] m_ObstaclePieces = new int[2];
	
	
	public bool[] m_ShowEnvLayers = new bool[(int)(EntityDefs.EnvLayer.NUM)];
	public int[,] m_EnvPieces = new int[(int)(EntityDefs.EnvLayer.NUM), 2];
	
	public int[] m_SkyboxPieces = new int[2];
	public int[] m_BackgroundPieces = new int[2];
	public int[] m_MidgroundPieces = new int[2];
	public int[] m_ForegroundPieces = new int[2];
	public int[] m_OverlayPieces = new int[2];
	
	
	//	For managing pieces in play mode
	bool m_playModeSequenceInitialised = false;

	ObstacleTrackMaker m_ObstacleTrackMaker = new ObstacleTrackMaker();
	EnvironmentTrackMaker m_EnvironmentTrackMaker = new EnvironmentTrackMaker();
	
	void Start() 
	{
		if( RL.m_Obstacles )
		{
			RL.m_Obstacles.HideAllPieces(); 
		}
	}
	
	void Update() 
	{	
		if( Application.isPlaying ) 
			updatePlayMode();
		else
			updateEditMode();
	}

	
	//-----------------------------------------------------------------------------
	// Method:	updateEditMode()
	// Desc:	Sets the inspector parameter pieces on the obstacle and environment
	//			managers to allow the user to slide back and forth on master time
	void updateEditMode()
	{
		if( RL.m_Obstacles && m_ShowObstacles )
		{
			RL.m_Obstacles.HideAllPiecesBut( m_ObstaclePieces[0], m_ObstaclePieces[1] ); 
			
			RL.m_Obstacles.ShowPiece( m_ObstaclePieces[0] );
			RL.m_Obstacles.SetPieceDistance( m_ObstaclePieces[0], 0.0f );
			
			RL.m_Obstacles.ShowPiece( m_ObstaclePieces[1] );
			RL.m_Obstacles.SetPieceDistance( m_ObstaclePieces[1], 
				RL.m_Obstacles.GetPieceWidth( m_ObstaclePieces[0] ) );
		}
		
		if( RL.m_Environment )
		{
			RL.m_Environment.HideAllPieces();
			
			//	Have to do this long hand until I make the inspector display 2d arrays			
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.SKYBOX,
				ref m_SkyboxPieces[0] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.SKYBOX, 0] = m_SkyboxPieces[0];
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.SKYBOX,
				ref m_SkyboxPieces[1] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.SKYBOX, 1] = m_SkyboxPieces[1];
			
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.BACKGROUND,
				ref m_BackgroundPieces[0] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.BACKGROUND, 0] = m_BackgroundPieces[0];
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.BACKGROUND,
				ref m_BackgroundPieces[1] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.BACKGROUND, 1] = m_BackgroundPieces[1];

			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.MIDGROUND,
				ref m_MidgroundPieces[0] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.MIDGROUND, 0] = m_MidgroundPieces[0];
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.MIDGROUND,
				ref m_MidgroundPieces[1] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.MIDGROUND, 1] = m_MidgroundPieces[1];
			
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.FOREGROUND,
				ref m_ForegroundPieces[0] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.FOREGROUND, 0] = m_ForegroundPieces[0];
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.FOREGROUND,
				ref m_ForegroundPieces[1] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.FOREGROUND, 1] = m_ForegroundPieces[1];
			
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.OVERLAY,
				ref m_OverlayPieces[0] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.OVERLAY, 0] = m_OverlayPieces[0];
			RL.m_Environment.ValidatePieceIdx( (int)EntityDefs.EnvLayer.OVERLAY,
				ref m_OverlayPieces[1] );
			m_EnvPieces[(int)EntityDefs.EnvLayer.OVERLAY, 1] = m_OverlayPieces[1];
			
			//	Now set all the layers according to master distance
			for( int l = 0; l<(int)EntityDefs.EnvLayer.NUM; l++ )
			{				
				if( m_ShowEnvLayers[l] )
				{
					RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 0] );
					RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 0], 0.0f );
					
					RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 1] );
					RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 1], 
						RL.m_Environment.GetPieceWidth( l, m_EnvPieces[l, 0] ) );			
				}
			}
		}
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	initialisePlayMode()
	// Desc:	Warms up the track makers and sets the initial piece slots
	void initialisePlayMode()
	{
		System.DateTime systemTime = System.DateTime.Now;		
		Random.seed = systemTime.Millisecond;
		
		RL.m_Obstacles.HideAllPieces(); 

		m_ObstacleTrackMaker.StartPlayMode();
		
		m_ObstaclePieces[0] = m_ObstacleTrackMaker.NextPiece();			
		RL.m_Obstacles.ValidatePieceIdx( ref m_ObstaclePieces[0] );
		RL.m_Obstacles.SetPieceDistance( m_ObstaclePieces[0], 0.0f );
		RL.m_Obstacles.ShowPiece( m_ObstaclePieces[0] );
		
		m_ObstaclePieces[1] = m_ObstacleTrackMaker.NextPiece();
		while( m_ObstaclePieces[0] == m_ObstaclePieces[1] )
			m_ObstaclePieces[1] = m_ObstacleTrackMaker.NextPiece();
		
		RL.m_Obstacles.ValidatePieceIdx( ref m_ObstaclePieces[1] );
		RL.m_Obstacles.SetPieceDistance( m_ObstaclePieces[1],
			RL.m_Obstacles.GetPieceWidth( m_ObstaclePieces[0] ) );
		RL.m_Obstacles.ShowPiece( m_ObstaclePieces[1] );
		
		
		RL.m_Environment.HideAllPieces();
		
		m_EnvironmentTrackMaker.StartPlayMode();
		
		for( int l = 0; l<(int)EntityDefs.EnvLayer.NUM; l++ )
		{
			m_EnvPieces[l, 0] = m_EnvironmentTrackMaker.NextPiece( l );			
			RL.m_Environment.ValidatePieceIdx( l, ref m_EnvPieces[l, 0] );
			RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 0], 0.0f );
			RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 0] );
			
			m_EnvPieces[l, 1] = m_EnvironmentTrackMaker.NextPiece( l );
			while( m_EnvPieces[l, 0] == m_EnvPieces[l, 1] )
				m_EnvPieces[l, 1] = m_EnvironmentTrackMaker.NextPiece( l );
			
			RL.m_Environment.ValidatePieceIdx( l, ref m_EnvPieces[l, 1] );
			RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 1],
				RL.m_Environment.GetPieceWidth( l, m_EnvPieces[l, 0] ) );
			RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 1] );
		}		
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	updatePlayMode()
	// Desc:	Increases distance over time and calls to the TrackMaker for pieces
	//			when a piece goes off screen to the left
	void updatePlayMode()
	{
		m_MasterDistance += Time.deltaTime*m_MetersPerSecond;
		
		if( !m_playModeSequenceInitialised )
		{
			m_playModeSequenceInitialised = true;			
			
			initialisePlayMode();
		}
		
		RL.m_Obstacles.UpdateMasterDistance( m_ObstaclePieces[0] );
		RL.m_Obstacles.UpdateMasterDistance( m_ObstaclePieces[1] );
		
		float hiddenDistance = RL.m_Obstacles.GetPieceDistance( m_ObstaclePieces[0] ) +
			RL.m_Obstacles.GetPieceWidth( m_ObstaclePieces[0] );
		
		if( m_MasterDistance > hiddenDistance )
		{
			//	Time to shuffle the pieces along and put a new one on the front
			RL.m_Obstacles.HidePiece( m_ObstaclePieces[0] );
			m_ObstaclePieces[0] = m_ObstaclePieces[1];
			
			m_ObstaclePieces[1] = m_ObstacleTrackMaker.NextPiece();			
			while( m_ObstaclePieces[0] == m_ObstaclePieces[1] )
				m_ObstaclePieces[1] = m_ObstacleTrackMaker.NextPiece();
		
			RL.m_Obstacles.ValidatePieceIdx( ref m_ObstaclePieces[1] );
			
			RL.m_Obstacles.SetPieceDistance( m_ObstaclePieces[1],
				RL.m_Obstacles.GetPieceDistance( m_ObstaclePieces[0] ) +
				RL.m_Obstacles.GetPieceWidth( m_ObstaclePieces[0] ) );
			
			RL.m_Obstacles.ShowPiece( m_ObstaclePieces[1] );			
		}
		
		
		//	Now do all the environment layers
		for( int l = 0; l<(int)EntityDefs.EnvLayer.NUM; l++ )
		{
			RL.m_Environment.UpdateMasterDistance( l, m_EnvPieces[l, 0] );
			RL.m_Environment.UpdateMasterDistance( l, m_EnvPieces[l, 1] );
			
			hiddenDistance = RL.m_Environment.GetPieceDistance( l, m_EnvPieces[l, 0] ) +
				RL.m_Environment.GetPieceWidth( l, m_EnvPieces[l, 0] );
			
			if( m_MasterDistance > hiddenDistance )
			{
				//	Time to shuffle the pieces along and put a new one on the front
				RL.m_Environment.HidePiece( l, m_EnvPieces[l, 0] );
				m_EnvPieces[l, 0] = m_EnvPieces[l, 1];
				
				m_EnvPieces[l, 1] = m_EnvironmentTrackMaker.NextPiece( l );			
				while( m_EnvPieces[l, 0] == m_EnvPieces[l, 1] )
					m_EnvPieces[l, 1] = m_EnvironmentTrackMaker.NextPiece( l );
			
				RL.m_Environment.ValidatePieceIdx( l, ref m_EnvPieces[l, 1] );
				
				RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 1],
					RL.m_Environment.GetPieceDistance( l, m_EnvPieces[l, 0] ) +
					RL.m_Environment.GetPieceWidth( l, m_EnvPieces[l, 0] ) );
				
				RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 1] );			
			}
		}
				
		
		//	Set camera according to prototype settings
		float dist = RL.m_Prototype.m_viewDistance;
		
		if( m_MasterDistance < 25.0f )
		{
			dist = 0.35f;
		}
		else if( m_MasterDistance < 30.0f )
		{
			float interp = (m_MasterDistance - 25.0f)/5.0f;
			float oldDist = dist;
			oldDist = 1.0f;	//	Sod it, just force the camera distance here
			
			dist = (1.0f - interp)*0.35f + interp*oldDist;
		}
		else
		{
			dist = 1.0f;
		}
		
		if( m_MetersPerSecond > 11.0f )
		{
			float interp = (m_MetersPerSecond - 11.0f)/19.0f;
			if( interp > 1.0f )	interp = 1.0f;
			
			dist = (1.0f - interp)*1.0f + interp*1.75f;			
		}
		
		GameObject cameraObject = RL.m_MainCamera.gameObject;

		//	Different function when zooming in or zooming out to take account of y
		RL.m_Prototype.m_viewDistance = dist;
		if( dist < 1.0f )
			cameraObject.transform.position = new Vector3( dist*10.0f, -0.7f + dist*6.2f - 2.5f, dist*-18.2f );
		else
			cameraObject.transform.position = new Vector3( dist*10.0f, 3.0f + dist*2.5f - 2.5f, dist*-18.2f );
		
		
		//	Speed player up depending on distance
		m_MetersPerSecond = 10.0f + m_MasterDistance/250.0f;
		
		if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
			RL.m_Player.m_animRunSpeed = m_MetersPerSecond*2.0f;		
		else
			RL.m_Player.m_animRunSpeed = m_MetersPerSecond*1.6f;		
	}	
}
