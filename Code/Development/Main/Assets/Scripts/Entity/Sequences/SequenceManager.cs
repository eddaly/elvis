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
	public int m_ObstacleFlipFlop = 1;
	
	
	public bool[] m_ShowEnvLayers = new bool[(int)(EntityDefs.EnvLayer.NUM)];
	public int[,] m_EnvPieces = new int[(int)(EntityDefs.EnvLayer.NUM), 2];
	
	public int[] m_SkyboxPieces = new int[2];
	public int[] m_BackgroundPieces = new int[2];
	public int[] m_MidgroundPieces = new int[2];
	public int[] m_ForegroundPieces = new int[2];
	public int[] m_OverlayPieces = new int[2];
	
	CriticallyDampedVector3Spring m_cameraPos = new CriticallyDampedVector3Spring();
	CriticallyDampedVector3Spring m_cameraAngle = new CriticallyDampedVector3Spring();
	bool m_snapCamera = true;
	
	int m_speedLevel = 0;
	public float m_SpeedNormal = 0.0f;
	public bool m_ChargeZoom = false;
	
	//	For managing pieces in play mode
	bool m_playModeSequenceInitialised = false;

	ObstacleTrackMaker m_ObstacleTrackMaker = new ObstacleTrackMaker();
	EnvironmentTrackMaker m_EnvironmentTrackMaker = new EnvironmentTrackMaker();
	
	void Start() 
	{		
		ResetForLevel();
	}
	
	void Update() 
	{	
		if( Application.isPlaying ) 
			updatePlayMode();
		else
			updateEditMode();
	}

	
	//-----------------------------------------------------------------------------
	// Method:	ResetForLevel()
	// Desc:	Sets the sequence up to begin the level
	public void ResetForLevel()
	{
		if( RL.m_Obstacles )
		{
			RL.m_Obstacles.HideAllPieces(); 
		}
		
		m_MasterDistance = 0.0f;
		m_MetersPerSecond = 8.0f;

		m_playModeSequenceInitialised = false;
		
		m_cameraPos.SetApproachTime( new Vector3( 20.0f, 500.0f, 20.0f ) );
		m_cameraAngle.SetApproachTime( new Vector3( 20.0f, 20.0f, 20.0f ) );
		m_snapCamera = true;
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
//		while( m_ObstaclePieces[0] == m_ObstaclePieces[1] )
//			m_ObstaclePieces[1] = m_ObstacleTrackMaker.NextPiece();
		
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
//			while( m_EnvPieces[l, 0] == m_EnvPieces[l, 1] )
//				m_EnvPieces[l, 1] = m_EnvironmentTrackMaker.NextPiece( l );
			
			RL.m_Environment.ValidatePieceIdx( l, ref m_EnvPieces[l, 1] );
			RL.m_Environment.SetPieceDistance( l, m_EnvPieces[l, 1],
				RL.m_Environment.GetPieceWidth( l, m_EnvPieces[l, 0] ) );
			RL.m_Environment.ShowPiece( l, m_EnvPieces[l, 1] );
		}		
	}
	
	
	public float CurrentObstacleWidth()
	{
		return RL.m_Obstacles.GetPieceWidth( m_ObstaclePieces[0] );
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
				
		calculateSpeedLevel();
		setCamera();
		setRunSpeed();
	}
	
	void calculateSpeedLevel()
	{
		//	The -15.0f is because3 the actual moment it happens is on 15m
		m_speedLevel = (int)((m_MasterDistance - 10.0f)/500.0f);
		
		//	When you get to 4000m, this is maximum speed. The final 1000m is the boss battle
		if( m_speedLevel > 8 )	m_speedLevel = 8;
		m_SpeedNormal = ((float)m_speedLevel)/8.0f;
		
//
//		m_SpeedNormal = 0.5f;
	}
	
	void setCamera()
	{
		//	Set camera according to prototype settings
		float dist = RL.m_Prototype.m_viewDistance;
		
		if( Application.isPlaying )
		{
			switch( RL.m_MainLoop.m_CurrentState )
			{
			case MainLoop.GameState.START:
				dist = 0.0f;
				break;
			case MainLoop.GameState.INTRO:
				dist = 0.0f;
				break;
			case MainLoop.GameState.RUNNING:
				float wideAngle = 1.0f - m_SpeedNormal;
				wideAngle *= wideAngle;
				wideAngle = 1.0f - wideAngle;
				dist = 1.0f + wideAngle;
				break;
			case MainLoop.GameState.COLLIDED:
				dist = 0.25f;
				break;
			case MainLoop.GameState.SUMMARY:
				dist = 0.75f;
				break;
			}
		}					
		
		//	Add in a camera event for speed-up moments
		float runDist = m_MasterDistance;
		bool judder = false;
		if( runDist < 4300.0f && runDist > 100.0f )
		{
			while( runDist > 500.0f )
				runDist -= 500.0f;
			
			runDist += 10.0f;		// offset so it starts just before you hit 500

			m_ChargeZoom = false;
			
			float approachMod = dist + 0.1f;
			if( approachMod > 1.0f )
				approachMod = 1.0f;
			
			m_cameraPos.SetApproachTime( new Vector3( 20.0f*approachMod, 500.0f*approachMod, 20.0f*approachMod ) );
			
			if( runDist < 20.0f )
			{
				//	Coming up to the speed up moment, so zoom in
				float interp = (500.0f - runDist)/25.0f;
				dist = interp*dist + (1.0f - interp)*0.5f;

				float zoomInSpeed = runDist/50.0f + 10.0f;
				m_cameraPos.SetApproachTime( new Vector3( 20.0f, 500.0f, zoomInSpeed ) );
				
				dist = 0.5f;
				m_ChargeZoom = true;
			}
			else if( runDist < 25.0f )
			{
				//	Judder the camera (which is now zoomed to 0.5)
				float judderZoom = Mathf.Sin( (runDist - 15.0f)*4.0f );
				judderZoom *= judderZoom*judderZoom;
				
				dist = 0.5f + judderZoom*0.025f;
				judder = true;
				m_ChargeZoom = true;
			}
		}
		
//		dist = 0.0f;
		
		Vector3 zeroPos = new Vector3( 3.0f, 2.1f, -5.0f );
		Vector3 zeroAng = new Vector3( 7.5f, 0.0f, 0.0f );
		
		Vector3 onePos = new Vector3( 10.0f, 3.0f, -16.0f );
		Vector3 oneAng = new Vector3( -9.0f, 0.0f, 0.0f );
		
		Vector3 twoPos = new Vector3( 9.0f, 3.3f, -25.0f );
		Vector3 twoAng = new Vector3( -7.0f, 10.0f, 0.0f );
				
		
		//	When fully zoomed in you need an offset on y of 
		//	9.7 (7.6) when player is at 7.5
		//	7.3 (5.2) when player is at 5
		//	4.7 (2.6) when player is at 2.5
		//	so 7.6 above the zoom = 0 vector.
		
		//	SO! (playerY/7.5)*7.8
		if( RL.m_MainLoop.m_CurrentState == MainLoop.GameState.INTRO )
			m_snapCamera = true;
		else if( dist < 1.0f )
		{
			float playerY = RL.m_Player.m_CollisionBox.m_CollisionBoxBase.y;
			
			if( playerY > 0.0f )
				zeroPos.y += (playerY/7.5f)*7.8f;
		}
		
		RL.m_Prototype.m_viewDistance = dist;
		Vector3 newPos;
		Vector3 newAng;
		if( dist < 1.0f )
		{			
			newPos = (1.0f - dist)*zeroPos + dist*onePos;
			newAng = (1.0f - dist)*zeroAng + dist*oneAng;
		}
		else
		{
			float interp = dist - 1.0f;
			
			newPos = (1.0f - interp)*onePos + interp*twoPos;
			newAng = (1.0f - interp)*oneAng + interp*twoAng;
		}
		
		m_cameraPos.SetTarget( newPos );
		m_cameraPos.Update( Time.deltaTime );
		m_cameraAngle.SetTarget( newAng );
		m_cameraAngle.Update( Time.deltaTime );
		
		if( m_snapCamera || judder )
		{
			m_snapCamera = false;
			
			m_cameraPos.SetPos( newPos );
			m_cameraAngle.SetPos( newAng );
		}
		
		GameObject cameraObject = RL.m_MainCamera.gameObject;

		cameraObject.transform.position = m_cameraPos.m_Pos;
		cameraObject.transform.localEulerAngles = m_cameraAngle.m_Pos;
	}
	
	void setRunSpeed()
	{
		switch( RL.m_MainLoop.m_CurrentState )
		{
		case MainLoop.GameState.START:
			break;
		case MainLoop.GameState.INTRO:
			break;
		case MainLoop.GameState.RUNNING:
			m_MetersPerSecond = 8.0f + 15.0f*m_SpeedNormal;
			break;
		case MainLoop.GameState.COLLIDED:
			
			m_MetersPerSecond = -1.5f*((float)(RL.m_Player.m_CollidedBounces));
			break;
		case MainLoop.GameState.SUMMARY:
			break;
		}
		
		if( RL.m_Prototype.m_PlayerType == PrototypeConfiguration.PlayerTypes.BALDY )
			RL.m_Player.m_animRunSpeed = m_MetersPerSecond*2.5f;		
		else
			RL.m_Player.m_animRunSpeed = m_MetersPerSecond*3.2f;		
	}
}
