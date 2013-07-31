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
		m_flipflopTrack = 0;
	}
	
	public void UpdatePlayMode() 
	{
	}
	
	
	
	public int NextPiece()
	{
		int nextPiece = m_flipflopTrack;
		m_flipflopTrack = 2 - m_flipflopTrack;
		
		return nextPiece;
	}
}
