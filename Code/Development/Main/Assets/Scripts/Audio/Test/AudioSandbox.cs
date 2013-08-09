using UnityEngine;
using System.Collections;

public class AudioSandbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		/*
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyBeat");
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyHalfBeat");
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyQuarterBeat");
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// OnGUI is called once per frame
	void OnGUI () {
		
		// Master music segments
		GUI.Box(new Rect(10,10,170,150), "Master Segments");
		
		if(GUI.Button(new Rect(20,40,150,22), "Toggle Intro_01"))
		{
			RL.m_MusicPlayer.ToggleSegment("VLV_Intro_01", EPMusicPlayer.Flags.IsMaster );		
		}
		
		if(GUI.Button(new Rect(20,70,150,22), "Toggle Intro_02"))
		{
			RL.m_MusicPlayer.ToggleSegment("VLV_Intro_02", EPMusicPlayer.Flags.IsMaster );
		}	
		
		if(GUI.Button(new Rect(20,100,150,22), "Toggle Main_01"))
		{
			RL.m_MusicPlayer.ToggleSegment("VLV_Main_01", EPMusicPlayer.Flags.IsMaster );
		}	
		
		if(GUI.Button(new Rect(20,130,150,22), "Toggle Outro_01"))
		{
			RL.m_MusicPlayer.ToggleSegment("VLV_Outro_01", EPMusicPlayer.Flags.IsMaster, EPMusicSegment.CueType.BAR );
		}
		
		// Stings
		GUI.Box (new Rect(200,10,170,150), "Stings");
		
		if(GUI.Button(new Rect(210,40,150,22), "Trigger Pickup_01"))
		{
			RL.m_SoundController.PlaySting("Pickup_01");		
		}
		
		if(GUI.Button(new Rect(210,70,150,22), "Grid: 1/1"))
		{
			RL.m_SoundController.m_StingGrid = EPSoundController.StingGrid.BEAT;
		}
		
		if(GUI.Button(new Rect(210,100,150,22), "Grid: 1/2"))
		{
			RL.m_SoundController.m_StingGrid = EPSoundController.StingGrid.HALFBEAT;	
		}
		
		if(GUI.Button(new Rect(210,130,150,22), "Grid: 1/4"))
		{
			RL.m_SoundController.m_StingGrid = EPSoundController.StingGrid.QUARTERBEAT;	
		}
		
		// Fades
		GUI.Box(new Rect(200,170,170,150), "Fades");
		
		if(GUI.Button(new Rect(210,200,150,22), "Vol To 0"))
		{
			RL.m_MusicPlayer.m_MasterSegment.SetFade(0.1f, 1.0f, 3.0f, true);		
		}
		
		if(GUI.Button(new Rect(210,230,150,22), "Pitch To 0.1"))
		{
			RL.m_MusicPlayer.m_MasterSegment.SetFade(1.0f, 0.1f, 3.0f, true);		
		}
		
		if(GUI.Button(new Rect(210,260,150,22), "Vol/Pitch Combined"))
		{
			RL.m_MusicPlayer.m_MasterSegment.SetFade(0.0f, 0.1f, 5.0f, true);		
		}
		
		if(GUI.Button(new Rect(210,290,150,22), "Vol/Pitch Fade All"))
		{
			RL.m_MusicPlayer.FadeAll(0.0f, 0.1f, 5.0f, true);		
		}
		
		
		// Layers
		GUI.Box(new Rect(10,170,170,150), "Layers");
		
		if(GUI.Button(new Rect(20,200,150,22), "Running Percussion"))
		{
			RL.m_MusicPlayer.ToggleSegment("VLV_BongoLoop");		
		}
		
		// Other controls
		GUI.Box (new Rect(390,10,170,150), "Misc");
		
		if(GUI.Button(new Rect(400,40,150,22), "Pause"))
		{
			RL.m_SoundController.Pause();		
		}
		
		if(GUI.Button(new Rect(400,70,150,22), "Resume"))
		{
			RL.m_SoundController.Resume();		
		}
		
		if(GUI.Button(new Rect(400,100,150,22), "GlobalMusicVolume +"))
		{
			float vol = RL.m_SoundController.m_MixGroupVolumes[(int)EPSoundController.MixGroup.MUSIC];
			
			if ( vol < 1.0f)
			{
				vol += 0.1f;
				RL.m_SoundController.SetMixGroupVolume(EPSoundController.MixGroup.MUSIC, vol );
				Debug.Log("Music MixGroup Volume: " + RL.m_SoundController.m_MixGroupVolumes[(int)EPSoundController.MixGroup.MUSIC]);
			}
		}
		
		if(GUI.Button(new Rect(400,130,150,22), "GlobalMusicVolume -"))
		{
			float vol = RL.m_SoundController.m_MixGroupVolumes[(int)EPSoundController.MixGroup.MUSIC];
			if ( vol > 0 )
			{
				vol -= 0.1f;
				RL.m_SoundController.SetMixGroupVolume(EPSoundController.MixGroup.MUSIC, vol );
				Debug.Log("Music MixGroup Volume: " + RL.m_SoundController.m_MixGroupVolumes[(int)EPSoundController.MixGroup.MUSIC]);
			}
		}
		
		// Game actions
		GUI.Box (new Rect(390,170,170,150), "Game Events");
		
		if(GUI.Button(new Rect(400,200,150,22), "Jump"))
		{
			RL.m_MusicPlayer.FadeAll( 0.3f, 1.0f, 1.0f, false );
			RL.m_MusicPlayer.Fade( "VLV_BongoLoop", 0.0f, 1.0f, 0.5f, false );
			RL.m_SoundController.Play("VLV_Flight");
			RL.m_SoundController.SetFade("VLV_Flight", 1.0f, 1.5f, 0.2f, false);
		}
		
		if(GUI.Button(new Rect(400,230,150,22), "Land"))
		{
			RL.m_MusicPlayer.FadeAll( 1.0f, 1.0f, 1.0f, false );
			RL.m_MusicPlayer.Fade( "VLV_BongoLoop", 1.0f, 1.0f, 0.5f, false );
			RL.m_SoundController.SetFade("VLV_Flight", 0.0f, 1.0f, 0.5f, false);
		}
	}// OnGUI
}
