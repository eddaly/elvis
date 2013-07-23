//-----------------------------------------------------------------------------
// File: ObstacleManager.cs
//
// Desc:	Creates and contains the master list of obstacle pieces
//
//			An obstacle piece is a list of obstacle entities
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObstacleManager : MonoBehaviour 
{
	
	const int kMaxPieces = 50;
	ObstaclePiece[] m_obstaclePieces = new ObstaclePiece[50];
	
	
	void Start() 
	{
		setupSomePieces();	
	}
	
	void Update() 
	{	
		if( RL.m_Sequencer.m_ShowObstacles )
		{		
			for( int p = 0; p<kMaxPieces; p++ )
			{
				if( m_obstaclePieces[p] != null && m_obstaclePieces[p].m_Active )
					m_obstaclePieces[p].DebugRender();
			}
		}
	}
		
	//-----------------------------------------------------------------------------
	// Method:	CollideWithBox()
	// Desc:	Collides all obstacles in the obstacle piece with the provided box.
	//			Returns the first obstacle that collides, or null if none of them 
	//			do.
	public Obstacle CollideWithBox( int piece_idx, Vector3 box_center, float box_width, float box_height )
	{
		return m_obstaclePieces[piece_idx].CollideWithBox( box_center, box_width, box_height );
	}
	
	//-----------------------------------------------------------------------------
	// Method:	PlatformCollide()
	// Desc:	Platform collides all obstacles in the obstacle piece with the	
	//			provided box. Returns the first obstacle that collides, or null if
	//			none of them do.
	public Obstacle PlatformCollide( int piece_idx, Vector3 box_center, float box_height, Vector3 move_vector,
		ref Vector3 collision_point )
	{
		return m_obstaclePieces[piece_idx].PlatformCollide( box_center, box_height, move_vector, 
			ref collision_point );
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	ValidatePieceIdx()
	// Desc:	Bounds check for piece indices.
	public void ValidatePieceIdx( ref int piece_idx )
	{
		if( piece_idx < 0 )				piece_idx = 0;
		if( piece_idx >= kMaxPieces )	piece_idx = kMaxPieces - 1;
		
		while( m_obstaclePieces[piece_idx] == null && piece_idx > 0 )
			piece_idx--;
	}


	//-----------------------------------------------------------------------------
	// Method:	HideAllPieces()
	public void HideAllPieces()
	{
		for( int p = 0; p<kMaxPieces; p++ )
		{
			if( m_obstaclePieces[p] != null )
				m_obstaclePieces[p].m_Active = false;
		}
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	ShowPiece()
	public void ShowPiece( int piece_idx )
	{
		m_obstaclePieces[piece_idx].m_Active = true;
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	HidePiece()
	public void HidePiece( int piece_idx )
	{
		m_obstaclePieces[piece_idx].m_Active = false;
	}

	
	//-----------------------------------------------------------------------------
	// Method:	SetPieceDistance()
	public void SetPieceDistance( int piece_idx, float distance )
	{
		m_obstaclePieces[piece_idx].m_Distance = distance;
	}

	
	//-----------------------------------------------------------------------------
	// Method:	GetPieceDistance()
	public float GetPieceDistance( int piece_idx )
	{
		return m_obstaclePieces[piece_idx].m_Distance;
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	GetPieceWidth()
	public float GetPieceWidth( int piece_idx )
	{
		return m_obstaclePieces[piece_idx].m_Width;
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	RefreshPieces()
	// Desc:	Forces the manager to flush and refresh the pieces
	public void RefreshPieces()
	{
		//	Lost references will be garbage collected
		setupSomePieces();
	}
	
	//-----------------------------------------------------------------------------
	// Method:	setupSomePieces()
	// Desc:	For now, create some obstacle pieces by hand
	void setupSomePieces()
	{
		//	Clear the references we have for now, let garbage collection deal with the
		//	freed references
		for( int p = 0; p<kMaxPieces; p++ )
		{
			m_obstaclePieces[p] = null;
		}
		
		switch( RL.m_Prototype.m_TrackType )
		{
		case PrototypeConfiguration.TrackTypes.ANALOGUE_JUMPS1:
		case PrototypeConfiguration.TrackTypes.MULTI_TRACK:
			setupAnalogueJumpsTrack();
			break;
		case PrototypeConfiguration.TrackTypes.EASY_JUMPS1:
			break;
		case PrototypeConfiguration.TrackTypes.FULL_SCREEN_OBSTACLES:
			break;
		}
	}
	
	void setupAnalogueJumpsTrack()
	{
		m_obstaclePieces[0] = new ObstaclePiece();
		
		KillSphere newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 4.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 1.0f;
		m_obstaclePieces[0].AddObstacle( newSphere );
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 11.0f, 5.0f, 0.0f );
		newSphere.m_Radius = 2.0f;
		m_obstaclePieces[0].AddObstacle( newSphere );

		Platform newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 10.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 20.0f;
		m_obstaclePieces[0].AddObstacle( newPlatform );


		m_obstaclePieces[1] = new ObstaclePiece();
		
		KillBox newBox = new KillBox();
		newBox.m_Position = new Vector3( 5.0f, 0.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[1].AddObstacle( newBox );

		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 7.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 8.0f;
		m_obstaclePieces[1].AddObstacle( newPlatform );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 14.0f, 4.4f, 0.0f );
		newPlatform.m_Width = 12.0f;
		m_obstaclePieces[1].AddObstacle( newPlatform );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 10.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 20.0f;
		m_obstaclePieces[1].AddObstacle( newPlatform );

		
		m_obstaclePieces[2] = new ObstaclePiece();
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 13.0f, 6.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 8.0f;
		m_obstaclePieces[2].AddObstacle( newBox );

		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 2.0f, 1.0f, 0.0f );
		newSphere.m_Radius = 3.0f;
		m_obstaclePieces[2].AddObstacle( newSphere );

		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 10.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 20.0f;
		m_obstaclePieces[2].AddObstacle( newPlatform );
		
		
		m_obstaclePieces[3] = new ObstaclePiece();
		m_obstaclePieces[3].m_Width = 20.0f;
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 10.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 20.0f; 
		m_obstaclePieces[3].AddObstacle( newPlatform );

		//	Step up
		newPlatform = new Platform(); 
		newPlatform.m_Position = new Vector3( 10.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 10.0f;
		m_obstaclePieces[3].AddObstacle( newPlatform );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 6.0f, 1.1f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[3].AddObstacle( newBox );
		
		//	Again
		newPlatform = new Platform(); 
		newPlatform.m_Position = new Vector3( 13.0f, 4.4f, 0.0f );
		newPlatform.m_Width = 5.0f;
		m_obstaclePieces[3].AddObstacle( newPlatform );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 12.5f, 3.3f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[3].AddObstacle( newBox );

		newPlatform = new Platform(); 
		newPlatform.m_Position = new Vector3( 18.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 6.0f;
		m_obstaclePieces[3].AddObstacle( newPlatform );
		

		//	Bunch of tricky jumps
		m_obstaclePieces[4] = new ObstaclePiece();
		m_obstaclePieces[4].m_Width = 70.0f;
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 35.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 70.0f; 
		m_obstaclePieces[4].AddObstacle( newPlatform );

		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 10.0f, 1.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[4].AddObstacle( newBox );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 10.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 2.0f;
		m_obstaclePieces[4].AddObstacle( newPlatform );

		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 15.0f, 1.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[4].AddObstacle( newBox );
		
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 30.0f, 1.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[4].AddObstacle( newBox );

		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 45.0f, 1.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[4].AddObstacle( newBox );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 45.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 2.0f;
		m_obstaclePieces[4].AddObstacle( newPlatform );

		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 53.0f, 2.0f, 0.0f );
		newBox.m_Width = 2.2f;
		newBox.m_Height = 4.4f;
		m_obstaclePieces[4].AddObstacle( newBox );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 53.0f, 4.4f, 0.0f );
		newPlatform.m_Width = 4.0f;
		m_obstaclePieces[4].AddObstacle( newPlatform );
		
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 63.0f, 6.0f, 0.0f );
		newSphere.m_Radius = 2.0f;
		m_obstaclePieces[4].AddObstacle( newSphere );

		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 67.0f, 0.25f, 0.0f );
		newSphere.m_Radius = 0.5f;
		m_obstaclePieces[4].AddObstacle( newSphere );
		
		
		m_obstaclePieces[5] = new ObstaclePiece();
		m_obstaclePieces[5].m_Width = 30.0f;
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 10.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 1.6f;
		m_obstaclePieces[5].AddObstacle( newSphere );
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 15.0f, 5.0f, 0.0f );
		newSphere.m_Radius = 1.6f;
		m_obstaclePieces[5].AddObstacle( newSphere );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 20.0f, 0.5f, 0.0f );
		newBox.m_Width = 2.5f;
		newBox.m_Height = 1.0f;
		m_obstaclePieces[5].AddObstacle( newBox );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 15.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 30.0f;
		m_obstaclePieces[5].AddObstacle( newPlatform );
		
		
		m_obstaclePieces[6] = new ObstaclePiece();
		m_obstaclePieces[6].m_Width = 40.0f;
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 7.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 0.5f;
		m_obstaclePieces[6].AddObstacle( newSphere );
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 14.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 1.0f;
		m_obstaclePieces[6].AddObstacle( newSphere );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 29.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 2.0f;
		m_obstaclePieces[6].AddObstacle( newPlatform );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 29.0f, 1.0f, 0.0f );
		newBox.m_Width = 1.5f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[6].AddObstacle( newBox );
		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 33.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 3.0f;
		m_obstaclePieces[6].AddObstacle( newSphere );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 20.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 40.0f;
		m_obstaclePieces[6].AddObstacle( newPlatform );
		
		
		
		m_obstaclePieces[7] = new ObstaclePiece();
		m_obstaclePieces[7].m_Width = 25.0f;
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 22.0f, 1.0f, 0.0f );
		newBox.m_Width = 1.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[7].AddObstacle( newBox );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 12.5f, 0.0f, 0.0f );
		newPlatform.m_Width = 25.0f;
		m_obstaclePieces[7].AddObstacle( newPlatform );
		

		
		
		m_obstaclePieces[8] = new ObstaclePiece();
		m_obstaclePieces[8].m_Width = 25.0f;
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 16.0f, 0.25f, 0.0f );
		newBox.m_Width = 5.0f;
		newBox.m_Height = 0.5f;
		m_obstaclePieces[8].AddObstacle( newBox );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 16.0f, 0.75f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 1.5f;
		m_obstaclePieces[8].AddObstacle( newBox );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 12.5f, 0.0f, 0.0f );
		newPlatform.m_Width = 25.0f;
		m_obstaclePieces[8].AddObstacle( newPlatform );
		
		

		m_obstaclePieces[9] = new ObstaclePiece();
		m_obstaclePieces[9].m_Width = 40.0f;
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 20.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 20.0f;
		m_obstaclePieces[9].AddObstacle( newPlatform );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 15.0f, 1.0f, 0.0f );
		newBox.m_Width = 9.0f;
		newBox.m_Height = 2.0f;
		m_obstaclePieces[9].AddObstacle( newBox );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 18.0f, 2.7f, 0.0f );
		newBox.m_Width = 1.0f;
		newBox.m_Height = 1.0f;
		m_obstaclePieces[9].AddObstacle( newBox );
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 31.0f, 6.1f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 7.8f;
		m_obstaclePieces[9].AddObstacle( newBox );
		
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 20.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 40.0f;
		m_obstaclePieces[9].AddObstacle( newPlatform );

		
		
		//	Big tracks
		addRandomisedBigTrack( 10 );
		addRandomisedBigTrack( 11 );
		addRandomisedBigTrack( 12 );
		addRandomisedBigTrack( 13 );
		addRandomisedBigTrack( 14 );
	}
	
	void addRandomisedBigTrack( int idx )
	{
		Random.seed = idx;
		
		m_obstaclePieces[idx] = new ObstaclePiece();
		m_obstaclePieces[idx].m_Width = 100.0f;
				
		Platform newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 50.0f, 0.0f, 0.0f );
		newPlatform.m_Width = 100.0f; 
		m_obstaclePieces[idx].AddObstacle( newPlatform );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 50.0f, 2.2f, 0.0f );
		newPlatform.m_Width = 90.0f; 
		m_obstaclePieces[idx].AddObstacle( newPlatform );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 50.0f, 4.4f, 0.0f );
		newPlatform.m_Width = 90.0f; 
		m_obstaclePieces[idx].AddObstacle( newPlatform );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 50.0f, 6.6f, 0.0f );
		newPlatform.m_Width = 90.0f; 
		m_obstaclePieces[idx].AddObstacle( newPlatform );
				
		newPlatform = new Platform();
		newPlatform.m_Position = new Vector3( 50.0f, 8.8f, 0.0f );
		newPlatform.m_Width = 90.0f; 
		m_obstaclePieces[idx].AddObstacle( newPlatform );
		
		
		KillBox newBox;
		
		for( int o = 0; o<6; o++ )
		{
			int numBoxes = Random.Range( 1, 4 );
			
			//	Put a naughty bit in the middle
			if( o == 3 )
				numBoxes = 4;
			
			for( int b = 0; b<numBoxes; b++ )
			{
				float distance = 15.0f + (float)o*15.0f;
				
				newBox = new KillBox();
				newBox.m_Height = 2.0f;
				newBox.m_Width = 2.0f;
				
				int iHeight = Random.Range( 0, 5 );
				float height = 1.1f + (float)iHeight*2.2f;
				
				newBox.m_Position = new Vector3( distance, height, 0.0f );
				
				m_obstaclePieces[idx].AddObstacle( newBox );
			}
		}
	}
}
