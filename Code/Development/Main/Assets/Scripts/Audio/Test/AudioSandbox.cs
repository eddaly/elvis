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
		GUI.Box(new Rect(10,10,170,180), "Master Segments");
		
		if(GUI.Button(new Rect(20,40,150,22), "Toggle Intro_01"))
		{
			EPMusicPlayer.Get().ToggleSegment("VLV_Intro_01", EPMusicPlayer.Flags.IsMaster );		
		}
		
		if(GUI.Button(new Rect(20,70,150,22), "Toggle Intro_02"))
		{
			EPMusicPlayer.Get().ToggleSegment("VLV_Intro_02", EPMusicPlayer.Flags.IsMaster );
		}	
		
		if(GUI.Button(new Rect(20,100,150,22), "Toggle Main_01"))
		{
			EPMusicPlayer.Get().ToggleSegment("VLV_Main_01", EPMusicPlayer.Flags.IsMaster );
		}	
		
		if(GUI.Button(new Rect(20,130,150,22), "Toggle Outro_01"))
		{
			EPMusicPlayer.Get().ToggleSegment("VLV_Outro_01", EPMusicPlayer.Flags.IsMaster );
		}
		
		// Stings
		GUI.Box (new Rect(200,10,170,150), "Stings");
		
		if(GUI.Button(new Rect(210,40,150,22), "Trigger Pickup_01"))
		{
			EPSoundController.Get ().PlaySting("Pickup_01");		
		}
		
		if(GUI.Button(new Rect(210,70,150,22), "Grid: 1/1"))
		{
			EPSoundController.Get ().m_StingGrid = EPSoundController.StingGrid.BEAT;
		}
		
		if(GUI.Button(new Rect(210,100,150,22), "Grid: 1/2"))
		{
			EPSoundController.Get ().m_StingGrid = EPSoundController.StingGrid.HALFBEAT;	
		}
		
		if(GUI.Button(new Rect(210,130,150,22), "Grid: 1/4"))
		{
			EPSoundController.Get ().m_StingGrid = EPSoundController.StingGrid.QUARTERBEAT;	
		}
	}// OnGUI
}
