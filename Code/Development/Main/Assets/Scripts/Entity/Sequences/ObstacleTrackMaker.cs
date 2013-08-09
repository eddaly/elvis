//-----------------------------------------------------------------------------
// File: ObstacleTrackMaker.cs
//
// Desc:	Class that decides on which obstacle piece to present next to the
//			player. 
//
//			As the game proceeds in play mode, when an obstacle piece goes off 
//			the screen to the left, the SequenceManager will call the 
//			ObstacleTrackMaker for the index of the next required piece.
//
//			The decision is made based on external and internal game state.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ObstacleTrackMaker
{
	enum PieceType
	{
		BLANK,
		SMALL_JUMPS,
		COIN_SECTION,
		LAYERS,
		SET_PIECE
	}
	
	int m_flipflopTrack;
	
	const int kMaxPieces = 200;
	int[] m_pieceList = new int[kMaxPieces];
	int m_pieceListCursor = 0;
	
	public void StartPlayMode() 
	{
		m_flipflopTrack = 0;
		m_pieceListCursor = 0;
		
		randomisePieceList();
	}
	
	
	public void UpdatePlayMode() 
	{
	}
	
	public void ResetForLevel()
	{
		StartPlayMode();
	}
	
	bool coino = false;
	
	void randomisePieceList()
	{
		//	50m - blank
		//	170m - 4x30m pieces - coind, jump, jump, coin
		//	270m - 100m layered piece
		//	300m - 30m coin piece
		//	370m - 70m set piece
		//	400m - 30m jump piece
		//	470m - 70m set piece
		//	500m - 30m coin piece
		//	Repeat...
		
		int index = 0;
		for( int section = 0; section<10; section++ )
		{
			float speedNormal = ((float)section)/10.0f;
			
			m_pieceList[index++] = getPieceIndex( PieceType.BLANK, speedNormal );
			
			m_pieceList[index++] = getPieceIndex( PieceType.COIN_SECTION, speedNormal );
			m_pieceList[index++] = getPieceIndex( PieceType.SMALL_JUMPS, speedNormal );
			m_pieceList[index++] = getPieceIndex( PieceType.SMALL_JUMPS, speedNormal );
			m_pieceList[index++] = getPieceIndex( PieceType.COIN_SECTION, speedNormal );
			
			m_pieceList[index++] = getPieceIndex( PieceType.LAYERS, speedNormal );
			
			m_pieceList[index++] = getPieceIndex( PieceType.COIN_SECTION, speedNormal );
			
			m_pieceList[index++] = getPieceIndex( PieceType.SET_PIECE, speedNormal );
			m_pieceList[index++] = getPieceIndex( PieceType.SMALL_JUMPS, speedNormal );
			m_pieceList[index++] = getPieceIndex( PieceType.SET_PIECE, speedNormal );
			
			m_pieceList[index++] = getPieceIndex( PieceType.COIN_SECTION, speedNormal );
		}
		
		//	Make sure of blanks till the end
		while( index < kMaxPieces )
			m_pieceList[index++] = 0;
	}
	
	int getPieceIndex( PieceType type, float speed_normal )
	{
		if( speed_normal < 0.0f )
			speed_normal = RL.m_Sequencer.m_SpeedNormal;
	
		//	Piece0 - blank
		//	Piece1 to 9 - small jumps
		//	Piece10 to 19 - coin sections
		//	Piece20 to 22 - slow layers
		//	Piece23 to 25 - medium layers
		//	Piece26 to 28 - fast layers
		//	Piece29 to 32 - slow set pieces
		//	Piece33 to 36 - medium set pieces
		//	Piece37 to 40 - fast set pieces
		int newPiece = 0;
		
		switch( type )
		{
		case PieceType.BLANK:
			newPiece = 0;
			break;
		case PieceType.SMALL_JUMPS:
			newPiece = Random.Range( 1, 10 );
			break;
		case PieceType.COIN_SECTION:
			newPiece = Random.Range( 10, 20 );
			break;
		case PieceType.LAYERS:
			if( speed_normal < 0.34f )
				newPiece = Random.Range( 20, 23 );
			else if( speed_normal > 0.67f )
				newPiece = Random.Range( 23, 26 );
			else
				newPiece = Random.Range( 26, 29 );
			
			break;
		case PieceType.SET_PIECE:
			if( speed_normal < 0.34f )
				newPiece = Random.Range( 29, 33 );
			else if( speed_normal > 0.67f )
				newPiece = Random.Range( 33, 36 );
			else
				newPiece = Random.Range( 36, 40 );
			
			break;
		}
		
		Debug.Log( "returning " + newPiece.ToString() );
		
		return newPiece;
	}
	
	
	public int NextPiece()
	{
		int nextPiece;
		
		////////////////////////////////
		//	FLIP FLOP TEST
		////////////////////////////////
		nextPiece = m_flipflopTrack;
		m_flipflopTrack = 2 - m_flipflopTrack;
		
//		return nextPiece;
		////////////////////////////////
		//	FLIP FLOP TEST
		////////////////////////////////
		

		////////////////////////////////
		//	PIECE LIST STYLE
		////////////////////////////////
		nextPiece = m_pieceList[m_pieceListCursor];
		
		m_pieceListCursor++;
		if( m_pieceListCursor >= kMaxPieces )
			m_pieceListCursor = 0;
		
		return nextPiece;
		
		/*
		float dist = RL.m_Sequencer.m_MasterDistance;
		
		//	See if we need to insert the blank track - we do this on every speedup
		//	and at the start - so every 500 meters until 4000
		if( dist < 4300.0f )
		{
			float rangedDist = dist;
			while( rangedDist > 500.0f )
				rangedDist -= 500.0f;
			
			float distToZero = rangedDist;
			if( distToZero > 250.0f )
				distToZero = 500.0f - distToZero;
			
			if( distToZero < 20.0f )
			{
				coino = false;
				return Random.Range( 0, 20 );
			}

			//	So, we don't need to insert a blank
		
		
			//	See if we will insert a layered piece. This happens at 230 into each 500m block
			float distToLayered = Mathf.Abs( 230.0f - rangedDist );
			
			if( distToLayered < 20.0f )
			{
				//	See which layered piece to place
				float cycle = dist/1000.0f;
				
				if( cycle < 1.0f )
					return 20;
				else if( cycle < 2.0f )
					return 21;
				else
					return 22;
			}
		}
		
		
		coino = !coino;
		
		if( coino )
			return Random.Range( 12, 20 );
		else
			return Random.Range( 0, 10 );	
			*/	
	}
}
