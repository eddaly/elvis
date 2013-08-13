//-----------------------------------------------------------------------------
// File: EnvironmentManager.cs
//
// Desc:	Creates and contains the master list of environment pieces
//
//			An environment piece is a GameObject which acts as a parent
//
// Magic ratio = 0.55
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EnvironmentManager : MonoBehaviour 
{
	const int kMaxPieces = 50;
	EnvironmentPiece[,] m_environmentPieces = 
		new EnvironmentPiece[(int)EntityDefs.EnvLayer.NUM, kMaxPieces];
	
	string[] m_parentObjectNames = new string[(int)(EntityDefs.EnvLayer.NUM)];
	string[] m_childObjectNames = new string[(int)(EntityDefs.EnvLayer.NUM)];
	
	
	
	void Start() 
	{
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.SKYBOX)] = "SkyBoxes";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.SKYBOX)] = "SkyBox";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.BACKGROUND)] = "BackgroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.BACKGROUND)] = "Piece";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.MIDGROUND)] = "MidgroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.MIDGROUND)] = "Piece";
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.FOREGROUND)] = "ForegroundPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.FOREGROUND)] = "Piece";		
		m_parentObjectNames[(int)(EntityDefs.EnvLayer.OVERLAY)] = "OverlayPieces";
		m_childObjectNames[(int)(EntityDefs.EnvLayer.OVERLAY)] = "Piece";		
		
		PopulatePieceArray();
	}
	
	void Update() 
	{	
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			if( RL.m_Sequencer.m_ShowEnvLayers[l] )
			{
				for( int p = 0; p<kMaxPieces; p++ )
				{
					if( m_environmentPieces[l, p] != null && m_environmentPieces[l, p].gameObject.activeSelf )
						m_environmentPieces[l, p].DebugRender();
				}
			}
		}
	}

	
	
	//-----------------------------------------------------------------------------
	// Method:	ValidatePieceIdx()
	// Desc:	Bounds check for piece indices.
	public void ValidatePieceIdx( int layer, ref int piece_idx )
	{
		if( piece_idx < 0 )				piece_idx = 0;
		if( piece_idx >= kMaxPieces )	piece_idx = kMaxPieces - 1;
		
		while( m_environmentPieces[layer, piece_idx] == null && piece_idx > 0 )
			piece_idx--;
	}

	
	//-----------------------------------------------------------------------------
	// Method:	HideAllPieces()
	public void HideAllPieces()
	{
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			for( int p = 0; p<kMaxPieces; p++ )
			{
				if( m_environmentPieces[l, p] != null )
					m_environmentPieces[l, p].gameObject.SetActive( false );
			}
		}
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	ShowPiece()
	public void ShowPiece( int layer, int piece_idx )
	{
		m_environmentPieces[layer, piece_idx].gameObject.SetActive( true );
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	HidePiece()
	public void HidePiece( int layer, int piece_idx )
	{
		m_environmentPieces[layer, piece_idx].gameObject.SetActive( false );
	}

	
	//-----------------------------------------------------------------------------
	// Method:	UpdateMasterDistance()
	public void UpdateMasterDistance( int layer, int piece_idx )
	{
		float distanceOffset = m_environmentPieces[layer, piece_idx].m_Distance - RL.m_Sequencer.m_MasterDistance; 
		m_environmentPieces[layer, piece_idx].transform.localPosition =
			new Vector3( distanceOffset, 0.0f, 0.0f );
	}

	
	//-----------------------------------------------------------------------------
	// Method:	SetPieceDistance()
	public void SetPieceDistance( int layer, int piece_idx, float distance )
	{
		m_environmentPieces[layer, piece_idx].m_Distance = distance;
		
		float distanceOffset = distance - RL.m_Sequencer.m_MasterDistance; 
		m_environmentPieces[layer, piece_idx].transform.localPosition =
			new Vector3( distanceOffset, 0.0f, 0.0f );
	}

	
	//-----------------------------------------------------------------------------
	// Method:	GetPieceDistance()
	public float GetPieceDistance( int layer, int piece_idx )
	{
		return m_environmentPieces[layer, piece_idx].m_Distance;
	}
	
	
	//-----------------------------------------------------------------------------
	// Method:	GetPieceWidth()
	public float GetPieceWidth( int layer, int piece_idx )
	{
		return m_environmentPieces[layer, piece_idx].m_Width;
	}
	
	//-----------------------------------------------------------------------------
	// Method:	PopulatePieceArray()
	// Desc:	Scans the GameObjects beneath us and gather the array of
	//			EnvironmentPiece objects as we find them
	public void PopulatePieceArray()
	{		
		int childIndex = 0;
		Transform childTransform;
					
		//	Clear out the current piece references
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			for( int p = 0; p<kMaxPieces; p++ )
				m_environmentPieces[l, p] = null;
		}
		
		//	Scan children for the top-level layer objects
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			Transform layerParent = transform.FindChild( m_parentObjectNames[l] );
			childIndex = 0;
			
			if( layerParent != null )
			{
				childTransform = layerParent.FindChild( m_childObjectNames[l] + childIndex.ToString() );
				while( childTransform != null )
				{
					EnvironmentPiece pieceComponent = childTransform.GetComponent<EnvironmentPiece>();
					
					if( pieceComponent != null )
						m_environmentPieces[l, childIndex] = pieceComponent;
					else
					{
						Debug.Log( "!** No EnvironmentPiece on object in " + m_parentObjectNames[l] 
							+ " layer - " + childTransform.name );
					}
						
					childIndex++;
					childTransform = layerParent.FindChild( m_childObjectNames[l] + childIndex.ToString() );
				}
			}
		}
		
//		logEnvironmentPieces();
	}
	
	void logEnvironmentPieces()
	{
		string piecesLog = "Environment Pieces:\n\n";
		piecesLog += "*********************\n";
		
		for( int l = 0; l<(int)(EntityDefs.EnvLayer.NUM); l++ )
		{
			piecesLog += "    " + m_parentObjectNames[l] + ":\n";

			if( m_environmentPieces[l, 0] == null )
				piecesLog += "        empty\n";
			else
			{
				for( int p = 0; p<kMaxPieces; p++ )
				{
					if( m_environmentPieces[l, p] != null )
					piecesLog += "        " + m_environmentPieces[l, p].gameObject.name + "\n";
				}
			}
			
			piecesLog += "\n";
		}
				
		piecesLog += "*********************\n";
		Debug.Log( piecesLog );
	}
}
