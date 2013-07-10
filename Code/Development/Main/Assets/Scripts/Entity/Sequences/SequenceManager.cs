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
	public float m_MetersPerSecond = 10.0f;
	
	public enum ActivePiece
	{
		ON_SCREEN = 0,
		OFF_SCREEN,
		
		NUM
	}
	ObstaclePiece[] m_ActiveObstaclePieces = new ObstaclePiece[(int)ActivePiece.NUM];
	EnvironmentPiece[,] m_ActiveEnvironmentPieces = 
		new EnvironmentPiece[(int)(EntityDefs.EnvLayer.NUM), (int)ActivePiece.NUM];
	
	
	//	For showing obstacle pieces in edit mode:
	public bool m_ShowObstacles = true;
	
	public int m_PrimaryObstacle = 0;
	public int m_SecondaryObstacle = 1;
	
	
	//	For managing pieces in play mode
	bool m_playModeSequenceInitialised = false;

	ObstacleTrackMaker m_ObstacleTrackMaker = new ObstacleTrackMaker();
	
	void Start() 
	{
		if( ReferenceLibrary.m_ObstacleManager )
		{
			ReferenceLibrary.m_ObstacleManager.HideAllPieces(); 
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
		if( ReferenceLibrary.m_ObstacleManager )
		{
			ReferenceLibrary.m_ObstacleManager.HideAllPieces(); 
			
			ReferenceLibrary.m_ObstacleManager.ValidatePieceIdx( ref m_PrimaryObstacle );
			ReferenceLibrary.m_ObstacleManager.ValidatePieceIdx( ref m_SecondaryObstacle );
			
			ReferenceLibrary.m_ObstacleManager.ShowPiece( m_PrimaryObstacle );
			ReferenceLibrary.m_ObstacleManager.SetPieceDistance( m_PrimaryObstacle, 0.0f );
			
			ReferenceLibrary.m_ObstacleManager.ShowPiece( m_SecondaryObstacle );
			ReferenceLibrary.m_ObstacleManager.SetPieceDistance( m_SecondaryObstacle, 
				ReferenceLibrary.m_ObstacleManager.GetPieceWidth( m_PrimaryObstacle ) );
		}
	}

	
	//-----------------------------------------------------------------------------
	// Method:	initialisePlayMode()
	// Desc:	Warms up the track makers and sets the initial piece slots
	void initialisePlayMode()
	{
		ReferenceLibrary.m_ObstacleManager.HideAllPieces(); 

		m_ObstacleTrackMaker.StartPlayMode();
		
		m_PrimaryObstacle = m_ObstacleTrackMaker.NextPiece();			
		ReferenceLibrary.m_ObstacleManager.ValidatePieceIdx( ref m_PrimaryObstacle );
		ReferenceLibrary.m_ObstacleManager.SetPieceDistance( m_PrimaryObstacle, 0.0f );
		ReferenceLibrary.m_ObstacleManager.ShowPiece( m_PrimaryObstacle );
		
		m_SecondaryObstacle = m_ObstacleTrackMaker.NextPiece();			
		ReferenceLibrary.m_ObstacleManager.ValidatePieceIdx( ref m_SecondaryObstacle );
		ReferenceLibrary.m_ObstacleManager.SetPieceDistance( m_SecondaryObstacle,
			ReferenceLibrary.m_ObstacleManager.GetPieceWidth( m_PrimaryObstacle ) );
		ReferenceLibrary.m_ObstacleManager.ShowPiece( m_SecondaryObstacle );
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
		
		float hiddenDistance = ReferenceLibrary.m_ObstacleManager.GetPieceDistance( m_PrimaryObstacle ) +
			ReferenceLibrary.m_ObstacleManager.GetPieceWidth( m_PrimaryObstacle );
		
		if( m_MasterDistance > hiddenDistance )
		{
			//	Time to shuffle the pieces along and put a new one on the front
			ReferenceLibrary.m_ObstacleManager.HidePiece( m_PrimaryObstacle );
			m_PrimaryObstacle = m_SecondaryObstacle;
			
			m_SecondaryObstacle = m_ObstacleTrackMaker.NextPiece();			
			ReferenceLibrary.m_ObstacleManager.ValidatePieceIdx( ref m_SecondaryObstacle );
			
			ReferenceLibrary.m_ObstacleManager.SetPieceDistance( m_SecondaryObstacle,
				ReferenceLibrary.m_ObstacleManager.GetPieceDistance( m_PrimaryObstacle ) +
				ReferenceLibrary.m_ObstacleManager.GetPieceWidth( m_PrimaryObstacle ) );
			
			ReferenceLibrary.m_ObstacleManager.ShowPiece( m_SecondaryObstacle );			
		}
	}
}
