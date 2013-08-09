//-----------------------------------------------------------------------------
// File: EnvironmentTrackMaker.cs
//
// Desc:	Class that decides on which environment piece to present next to
//			the player. 
//
//			As the game proceeds in play mode, when an piece goes off the 
//			screen to the left, the SequenceManager will call the 
//			EnvironmentTrackMaker for the index of the next required piece.
//
//			The decision is made based on external and internal game state.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EnvironmentTrackMaker
{
	int[] m_flipflopTrack = new int[(int)EntityDefs.EnvLayer.NUM];
	
	public void StartPlayMode() 
	{
		for( int f = 0; f<(int)EntityDefs.EnvLayer.NUM; f++ )
			m_flipflopTrack[f] = 0;
	}
	
	public void UpdatePlayMode() 
	{
	}
	
	public void ResetForLevel()
	{
		StartPlayMode();
	}
		
	public int NextPiece( int layer )
	{
		int nextPiece;
		
		if( layer == (int)EntityDefs.EnvLayer.OVERLAY )
		{
			nextPiece = Random.Range( 0, 6 );
		}
		else
		{
			nextPiece = m_flipflopTrack[layer];
			m_flipflopTrack[layer] = 1 - m_flipflopTrack[layer];
		}
		
		return nextPiece;
	}	
}
