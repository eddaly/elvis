//-----------------------------------------------------------------------------
// File: EPSoundController.cs
//
// Desc:	Class to control playback of EPSoundSequence and EPSound objects
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EPSoundController : MonoBehaviour 
{
	public bool m_PopulateList = false;
	
	public List<EPSoundEvent> m_EPSoundEventList = new List<EPSoundEvent>();
	public List<string> m_EPSoundNames = new List<string>();
	
	[HideInInspector]
	public bool m_flipRigs = false;
	[HideInInspector]
	public float m_GlobalSFXVolume = 1.0f;
	[HideInInspector]
	public float m_GlobalMusicVolume = 1.0f;
	[HideInInspector]
	public float m_SFXVolMaster = 1.0f;
	[HideInInspector]
	public float m_MusicVolMaster = 0.6f;
	[HideInInspector]
	public float m_DuckingAmount = 1.0f;
	
	//	Pseudo-singleton pattern
	private static EPSoundController ms_soundController = null;
    public static EPSoundController Get()
    {
		//	Cache the object to avoid frequent string lookups
		if( ms_soundController != null )
			return ms_soundController;

		GameObject soundControllerObject = GameObject.Find( "SoundController" );
        if( soundControllerObject == null )
        {
            Console.WriteLine( "!** No sound controller object found (SoundController)" );
        }
        else
        {
            ms_soundController = (EPSoundController)soundControllerObject.GetComponent( typeof(EPSoundController) );
        }
		
		if( ms_soundController != null )
	        return ms_soundController;
		
		Console.WriteLine( "!** Couldn't get EPSoundController component" );
		return null;
    }
	
	
	// Use this for initialization
	void Start()
	{	
		m_DuckingAmount = 1.0f;

		populateLists();
		
		m_GlobalSFXVolume = m_SFXVolMaster;
		m_GlobalMusicVolume = m_MusicVolMaster;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_PopulateList )
		{
			populateLists();
			
			m_PopulateList = false;
		}
	}
	
	public void Play( string sound_name )
	{
		int soundIdx = GetIndex ( sound_name );
		Play ( soundIdx );
	}
	
	public void Play( int sound_idx )
	{
		m_EPSoundEventList[sound_idx].Play ();
	}
	
	public void Play( string sound_name, float volume, float pitch )
	{
		int soundIdx = GetIndex( sound_name );
		Play(soundIdx, volume, pitch);
		Console.WriteLine(Channels.PC, "Play sound: " + sound_name);
	}
	
	public void Play( int sound_idx, float volume, float pitch )
	{
		m_EPSoundEventList[sound_idx].Play( volume, pitch );
	}
	
	public void StopAll()
	{	
		foreach (EPSoundEvent sound in m_EPSoundEventList)
		{
			sound.Stop();
		}
	}
	
	public void Pause()
	{
		foreach (EPSoundEvent sound in m_EPSoundEventList)
		{
			sound.Pause();
		}
	}
	
	public void Resume()
	{
		foreach (EPSoundEvent sound in m_EPSoundEventList)
		{
			sound.Resume ();
		}
	}
	
	int populateLists()
	{
		ClearLists();
		
		EPSoundEvent[] events = GetComponentsInChildren<EPSoundEvent>();
		foreach( EPSoundEvent child in events )
		{
			if ( ValidName( child.name ) )
			{
				m_EPSoundEventList.Add( child.GetComponent<EPSoundEvent>() );
				m_EPSoundNames.Add( child.name );
			}
			else
			{
				ClearLists();
				return -1;
			}
		}
		
		Console.WriteLine("Sound lists updated.");
		return 0;
	}
	
	void ClearLists()
	{
		m_EPSoundEventList.Clear();
		m_EPSoundNames.Clear ();
	}
	
	bool ValidName(string name)
	{
		if (m_EPSoundNames.Contains(name))
		{
			Console.WriteLine(Channels.General | Channels.Error, "Duplicate name found: " + name + ". List cannot contain duplicates");
			return false;
		}
		else
		{
			return true;
		}
	}
	
	public int GetIndex( string sound_name )
	{
		for( int idx = 0; idx<m_EPSoundNames.Count; idx++ )
		{
			//Console.WriteLine ("Index: " + idx);
			if( sound_name == m_EPSoundNames[idx] )
				return idx;
		}
		
		Console.WriteLine(Channels.General | Channels.Warning, "Sound name not found: " + sound_name);
		return 0;
	}
	
	
	//Set volume on all sound
	public void UpdateVolume(float newVolume)
	{
		foreach (EPSoundEvent sound in m_EPSoundEventList)
		{
			sound.UpdateVolume(newVolume);
		}
	}
}
