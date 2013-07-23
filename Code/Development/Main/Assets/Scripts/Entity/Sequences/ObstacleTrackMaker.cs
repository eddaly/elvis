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
	int m_flipflopTrack;
	
	public void StartPlayMode() 
	{
		m_flipflopTrack = 5;
		
		if( RL.m_Prototype.m_TrackType == PrototypeConfiguration.TrackTypes.MULTI_TRACK )
			m_flipflopTrack = Random.Range( 10, 15 );
	}
	
	public void UpdatePlayMode() 
	{
	}
	
	
	
	public int NextPiece()
	{
		int nextPiece = 0;
		
		if( RL.m_Prototype.m_TrackType == PrototypeConfiguration.TrackTypes.ANALOGUE_JUMPS1 )
			nextPiece = analogueJumps1Track();

		if( RL.m_Prototype.m_TrackType == PrototypeConfiguration.TrackTypes.MULTI_TRACK )
			nextPiece = multiTrack();
		
		Debug.Log( nextPiece.ToString() );
		
		return nextPiece;
	}
	
	int analogueJumps1Track()
	{
		if( m_flipflopTrack == 1 )
		{
			m_flipflopTrack = 2;
			return m_flipflopTrack;
		}
		
		if( Random.Range( 0.0f, 1.0f ) > 0.85f )
		{
			//	Do a long track piece
			m_flipflopTrack = Random.Range( 10, 15 );			
		}
		else
		{
			//	Do a long track piece
			m_flipflopTrack = Random.Range( 0, 10 );			
			
			while( m_flipflopTrack == 2 )
				m_flipflopTrack = Random.Range( 0, 10 );			
		}
		
		return m_flipflopTrack;
		
/*		m_flipflopTrack++;
		
		if( m_flipflopTrack > 5 )
		{
			//	Just done a random track, so reset
			m_flipflopTrack = 0;
		}
		else
		{
			if( m_flipflopTrack == 5 )
			{
				//	Do a random track
				m_flipflopTrack = Random.Range( 5, 10 );
			}
		}

		return m_flipflopTrack;*/
	}
	
	int multiTrack()
	{
		int nextPiece = Random.Range( 10, 15 );
		
		while( nextPiece == m_flipflopTrack )
			nextPiece = Random.Range( 10, 15 );
		
		m_flipflopTrack = nextPiece;
		return m_flipflopTrack;
	}
}
