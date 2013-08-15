//--------------------------------------------------------------------------------------
// File: AudioLauncher.cs
//
// Desc:	Sets up EPSoundController and EPMusicPlayer in the ReferenceLibrary
//			and loads specified sound banks
//
//			Destroys itself after launching
//
// Copyright Echo Peak Ltd 2013
//--------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class AudioLauncher : MonoBehaviour {
	
	public GameObject[] m_soundBanks;
	
	// Use this for initialization
	void Start ()
	{
		SetReferences();
		LoadSoundBanks();
		
		//RL.m_SoundController.populateLists();
		
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	void SetReferences()
	{
		if ( RL.m_SoundController == null )
		{
			RL.m_SoundController = ((GameObject)Instantiate(Resources.Load("SoundController"))).GetComponent<EPSoundController>();
			RL.m_SoundController.m_StingGrid = EPSoundController.StingGrid.QUARTERBEAT;	
			RL.m_MusicPlayer = RL.m_SoundController.gameObject.GetComponent<EPMusicPlayer>();
			//DontDestroyOnLoad(RL.m_SoundController);
		}
	}
	
	void LoadSoundBanks()
	{
		foreach ( GameObject sb in m_soundBanks )
		{
			// Only load soundbanks which aren't already loaded - check name in instantiated form "name(Clone)"
			if ( GameObject.Find( sb.name + "(Clone)") == null && GameObject.Find( sb.name ) == null )
			{
				Debug.Log("Sound bank " + sb.name + " not already loaded. Loading.");
				((GameObject)Instantiate(sb)).transform.parent = RL.m_SoundController.transform;
			}
			else
				Debug.Log ("Sound bank " + sb.name + " is already loaded.");
			
			// Move any local soundbanks into the SoundController hierarchy
			if ( GameObject.Find( sb.name ) != null )
				sb.transform.parent = RL.m_SoundController.transform;
		}
	}
}
