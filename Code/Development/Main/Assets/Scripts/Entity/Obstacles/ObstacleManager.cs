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
			for( int p = 0; p<kMaxPieces; p++ )
			{
				if( m_obstaclePieces[p] != null && m_obstaclePieces[p].m_Active )
					m_obstaclePieces[p].DebugRender();
			}
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
		Debug.Log( "Refreshing Pieces" );
		
		//	Lost references will be garbage collected
		setupSomePieces();
	}
	
	//-----------------------------------------------------------------------------
	// Method:	setupSomePieces()
	// Desc:	For now, create some obstacle pieces by hand
	void setupSomePieces()
	{
		Debug.Log( "Setting up some pieces" );
		
		KillSphere newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 4.0f, 0.0f, 0.0f );
		newSphere.m_Radius = 2.0f;
		
		m_obstaclePieces[0] = new ObstaclePiece();
		m_obstaclePieces[0].AddObstacle( newSphere );


		
		KillBox newBox = new KillBox();
		newBox.m_Position = new Vector3( 5.0f, 0.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 2.0f;
		
		m_obstaclePieces[1] = new ObstaclePiece();
		m_obstaclePieces[1].AddObstacle( newBox );

		
		
		newBox = new KillBox();
		newBox.m_Position = new Vector3( 15.0f, 3.0f, 0.0f );
		newBox.m_Width = 2.0f;
		newBox.m_Height = 6.0f;
		
		m_obstaclePieces[2] = new ObstaclePiece();
		m_obstaclePieces[2].AddObstacle( newBox );

		
		newSphere = new KillSphere();
		newSphere.m_Position = new Vector3( 6.0f, 1.0f, 0.0f );
		newSphere.m_Radius = 3.0f;
		
		m_obstaclePieces[2].AddObstacle( newSphere );
	}
}
