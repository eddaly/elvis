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
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update Lists after loading / unloading has finished
		RL.m_MusicPlayer.UpdateSegmentList();
		RL.m_SoundController.populateLists();
		
		// Destroy the launcher once complete
		Destroy(this.gameObject);
	}
	
	void SetReferences()
	{
		if ( RL.m_SoundController == null )
		{
			RL.m_SoundController = ((GameObject)Instantiate(Resources.Load("SoundController"))).GetComponent<EPSoundController>();
			RL.m_MusicPlayer = RL.m_SoundController.gameObject.GetComponent<EPMusicPlayer>();
			
			// TODO: Tidy this up later, shouldn't really be set here
			RL.m_SoundController.m_StingGrid = EPSoundController.StingGrid.QUARTERBEAT;	
		}
	}
	
	void LoadSoundBanks()
	{
		// Unload any banks that are no longer required  (count backwards so list is not reordered while iterating)
		for ( int n = RL.m_SoundController.m_SoundBankList.Count - 1; n >= 0; n-- )
		{
			GameObject sb = RL.m_SoundController.m_SoundBankList[n];
			bool sbRequired = false;
			for ( int i = 0; i < m_soundBanks.Length; i++ )
			{
				if ( m_soundBanks[i].name == sb.name || ( m_soundBanks[i].name  + "(Clone)" ) == sb.name )
				{
					//Debug.Log(m_soundBanks[i].name + " is needed, don't unload");
					sbRequired = true;
				}
			}
			if ( !sbRequired )
			{
				//Debug.Log("Unloading soundbank: " + sb);
				RL.m_SoundController.m_SoundBankList.RemoveAt(n);
				DestroyObject(sb);
			}
		}
		
		// Load any banks that aren't already loaded
		foreach ( GameObject sb in m_soundBanks )
		{
			// Only load soundbanks which aren't already loaded - check name in instantiated form "name(Clone)"
			if ( GameObject.Find( sb.name + "(Clone)") == null && GameObject.Find( sb.name ) == null )
			{
				//Debug.Log("Sound bank " + sb.name + " not already loaded. Loading.");			
				GameObject newBank = (GameObject)Instantiate(sb);
				newBank.transform.parent = RL.m_SoundController.transform;
				RL.m_SoundController.m_SoundBankList.Add (newBank);
			}
			else
			{
				//Debug.Log ("Sound bank " + sb.name + " is already loaded.");
			}
			
			// Move any local soundbanks into the SoundController hierarchy
			if ( GameObject.Find( sb.name ) != null )
			{
				sb.transform.parent = RL.m_SoundController.transform;
				RL.m_SoundController.m_SoundBankList.Add (sb);
			}
		}
		
	}
}
