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
		TITLE,
		START,
		INTRO,
		RUNNING,
		COLLIDED,
		SUMMARY
	}
	public GameState m_CurrentState = GameState.TITLE; 
	
	float m_stateTime = 0.0f;
	
	void Awake()
	{
		Application.targetFrameRate = 60;
	}
	
	void Start() 
	{
		GlobalData.Get.m_GlobalTime = 0.0f;
		
		m_CurrentState = GameState.TITLE;
		m_stateTime = 0.0f;
	}
	
	void Update() 
	{	
		GlobalData.Get.m_GlobalDTime = 1/60.0f;//Time.deltaTime;
		GlobalData.Get.m_GlobalTime += GlobalData.Get.m_GlobalDTime;
		
		m_stateTime += GlobalData.Get.m_GlobalDTime;
		
		InputManager.Get.Update();		
		if( !Application.isEditor )
			InputManager.Get.UpdateTouch();
		
		switch( m_CurrentState )
		{
		case GameState.TITLE:			
			if( InputManager.Get.GetPressed( 0 ) )
				changeState( GameState.START );
			break;
			
		case GameState.START:
			RL.m_Sequencer.ResetForLevel();
			RL.m_Player.ResetForLevel();
			changeState( GameState.INTRO );			
			break;
			
		case GameState.INTRO:
			if( RL.m_Sequencer.m_MasterDistance > 25.0f )
				changeState( GameState.RUNNING );			
			break;
			
		case GameState.RUNNING:
			break;
			
		case GameState.COLLIDED:
			if( m_stateTime > 3.0f )
				changeState( GameState.TITLE );			
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
		case GameState.TITLE:
			RL.m_EntryScreen.m_TransitionTarget = 1.0f;
			break;
			
		case GameState.START:
			RL.m_EntryScreen.m_TransitionTarget = 0.0f;
			break;
			
		case GameState.INTRO:			
			// Start the music & queue next segment
			RL.m_MusicPlayer.PlaySegment ("VLV_Intro_01", EPMusicPlayer.Flags.IsMaster, EPMusicSegment.CueType.INSTANT );
			RL.m_MusicPlayer.PlaySegment ("VLV_BongoLoop", EPMusicPlayer.Flags.None, EPMusicSegment.CueType.INSTANT );		
			break;
			
		case GameState.RUNNING:
			// Queue the music
			RL.m_MusicPlayer.PlaySegment ("VLV_Main_01", EPMusicPlayer.Flags.IsMaster, EPMusicSegment.CueType.END );			
			break;
			
		case GameState.COLLIDED:
			// Play collision sound effect
			RL.m_SoundController.Play("Collide_01");
			
			// End the music
			RL.m_MusicPlayer.PlaySegment ("VLV_Outro_01", EPMusicPlayer.Flags.IsMaster, EPMusicSegment.CueType.GRID );
			RL.m_MusicPlayer.StopSegment ("VLV_BongoLoop");			
			break;
			
		case GameState.SUMMARY:
			break;
		}
		
		m_CurrentState = new_state;
		m_stateTime = 0.0f;
	}
}
