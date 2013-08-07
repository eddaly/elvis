//-----------------------------------------------------------------------------
// File: MainLoop.cs
//
// Desc:	Overall game state manager
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class MainLoop : MonoBehaviour 
{
	public enum GameState
	{
		START,
		INTRO,
		RUNNING,
		COLLIDED,
		SUMMARY
	}
	public GameState m_CurrentState = GameState.START; 
	
	void Start() 
	{
	}
	
	void Update() 
	{	
		switch( m_CurrentState )
		{
		case GameState.START:
			RL.m_Sequencer.ResetForLevel();
			changeState( GameState.INTRO );
			break;
		case GameState.INTRO:
			if( RL.m_Sequencer.m_MasterDistance > 25.0f )
				changeState( GameState.RUNNING );
			break;
		case GameState.RUNNING:
			break;
		case GameState.COLLIDED:
			break;
		case GameState.SUMMARY:
			break;
		}
	}

	public void PlayerCollideToStop()
	{
		if( m_CurrentState == GameState.RUNNING )
			changeState( GameState.COLLIDED );
	}
	
	void changeState( GameState new_state )
	{
		switch( new_state )
		{
		case GameState.START:
			break;
		case GameState.INTRO:
			break;
		case GameState.RUNNING:
			break;
		case GameState.COLLIDED:
			break;
		case GameState.SUMMARY:
			break;
		}
		
		m_CurrentState = new_state;
	}
}
