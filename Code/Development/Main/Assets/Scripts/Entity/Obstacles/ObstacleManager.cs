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
		scanForPieces();	
	}
	
	void Update() 
	{	
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
				m_obstaclePieces[p].gameObject.SetActive( false );
		}
	}

	
	//-----------------------------------------------------------------------------
	// Method:	HideAllPiecesBut()
	// Desc:	Slightly odd workaround because of objects being removed from 
	//			execute list in edit mode
	public void HideAllPiecesBut( int piece1, int piece2 )
	{
		for( int p = 0; p<kMaxPieces; p++ )
		{
			if( m_obstaclePieces[p] != null )
			{
				if( p != piece1 && p != piece2 )
					m_obstaclePieces[p].gameObject.SetActive( false );
			}
		}
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	ShowPiece()
	public void ShowPiece( int piece_idx )
	{
		m_obstaclePieces[piece_idx].gameObject.SetActive( true );
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	HidePiece()
	public void HidePiece( int piece_idx )
	{
		m_obstaclePieces[piece_idx].gameObject.SetActive( false );
	}

	
	//-----------------------------------------------------------------------------
	// Method:	UpdateMasterDistance()
	public void UpdateMasterDistance( int piece_idx )
	{
		float distanceOffset = m_obstaclePieces[piece_idx].m_Distance - RL.m_Sequencer.m_MasterDistance; 
		m_obstaclePieces[piece_idx].transform.localPosition = new Vector3( distanceOffset, 0.0f, 0.0f );
	}

	
	//-----------------------------------------------------------------------------
	// Method:	SetPieceDistance()
	public void SetPieceDistance( int piece_idx, float distance )
	{
		m_obstaclePieces[piece_idx].m_Distance = distance;
		
		float distanceOffset = distance - RL.m_Sequencer.m_MasterDistance; 
		m_obstaclePieces[piece_idx].transform.localPosition = new Vector3( distanceOffset, 0.0f, 0.0f );
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
		scanForPieces();
	}
	
	//-----------------------------------------------------------------------------
	// Method:	scanForPieces()
	// Desc:	Scan through all children objects looking for ObstaclePiece script
	//			objects. Add these into our list of ObstaclePieces
	void scanForPieces()
	{
		//	Clear our references
		for( int p = 0; p<kMaxPieces; p++ )
			m_obstaclePieces[p] = null;
		
		//	For now we're going to force the naming convention "Piece0", "Piece2", etc.	
		Transform layerParent = transform.FindChild( "ObstaclePieces" );
		int childIndex = 0;
		
		if( layerParent != null )
		{
			Transform childTransform = layerParent.FindChild( "Piece" + childIndex.ToString() );
			while( childTransform != null )
			{
				ObstaclePiece pieceComponent = childTransform.GetComponent<ObstaclePiece>();
				
				if( pieceComponent != null )
					m_obstaclePieces[childIndex] = pieceComponent;
				else
					Debug.Log( "!** No ObstaclePiece on object - " + childTransform.name );
					
				childIndex++;
				childTransform = layerParent.FindChild( "Piece" + childIndex.ToString() );
			}
		}
		
		logObstaclePieces();
	}
	
	void logObstaclePieces()
	{
		string piecesLog = "Obstacle Pieces:\n\n";
		piecesLog += "*********************\n";
		
		if( m_obstaclePieces[0] == null )
			piecesLog += "        empty\n";
		else
		{
			for( int p = 0; p<kMaxPieces; p++ )
			{
				if( m_obstaclePieces[p] != null )
					piecesLog += "        " + m_obstaclePieces[p].gameObject.name + "\n";
			}
		}
		
		piecesLog += "\n";
				
		piecesLog += "*********************\n";
		Debug.Log( piecesLog );
	}
}
