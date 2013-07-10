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
		m_flipflopTrack = 1;
	}
	
	public void UpdatePlayMode() 
	{
	}
	
	public int NextPiece()
	{
		m_flipflopTrack++;
		if( m_flipflopTrack == 3 )
			m_flipflopTrack = 0;

		return m_flipflopTrack;
	}
}
