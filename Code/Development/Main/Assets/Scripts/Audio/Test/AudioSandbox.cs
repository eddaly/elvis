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
			EPMusicPlayer.Get().ToggleSegmentMaster("VLV_Intro_01");		
		}
		
		if(GUI.Button(new Rect(20,70,150,22), "Toggle Intro_02"))
		{
			EPMusicPlayer.Get().ToggleSegmentMaster("VLV_Intro_02");
		}	
		
		if(GUI.Button(new Rect(20,100,150,22), "Toggle Main_01"))
		{
			EPMusicPlayer.Get().ToggleSegmentMaster("VLV_Main_01");
		}	
		
		if(GUI.Button(new Rect(20,130,150,22), "Toggle Outro_01"))
		{
			EPMusicPlayer.Get().ToggleSegmentMaster("VLV_Outro_01");
		}
		
		// Stings
		GUI.Box (new Rect(200,10,170,150), "Stings");
		
		if(GUI.Button(new Rect(210,40,150,22), "Trigger Pickup_01"))
		{
			EPSoundController.Get ().PlaySting("Pickup_01");		
		}
		
	}// OnGUI
}
