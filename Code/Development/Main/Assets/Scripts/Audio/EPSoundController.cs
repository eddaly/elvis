//-----------------------------------------------------------------------------
// File: EPSoundController.cs
//
// Desc:	Class to control playback of EPSoundSequence and EPSound objects
//
// Copyright Echo Peak Ltd 2012
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class EPSoundController : MonoBehaviour 
{
	public bool m_PopulateList = false;
	
	public List<EPSoundEvent> m_EPSoundEventList = new List<EPSoundEvent>();
	public List<string> m_EPSoundNames = new List<string>();
	
	[HideInInspector]
	public float[] m_MixGroupVolumes;
	[HideInInspector]
	public float m_GlobalSFXVolume = 1.0f;
	[HideInInspector]
	public float m_GlobalMusicVolume = 1.0f;
	[HideInInspector]
	public float m_GlobalVOVolume = 1.0f;
	[HideInInspector]
	public float m_DuckingAmount = 1.0f;
	[HideInInspector]
	public List<EPSoundEvent> m_StingQueue = new List<EPSoundEvent>();
	
	public enum StingGrid
	{
		BEAT,
		HALFBEAT,
		QUARTERBEAT
	}
	
	[HideInInspector]
	public StingGrid m_StingGrid = StingGrid.HALFBEAT;
	
	public enum MixGroup
	{
		SFX,
		MUSIC,
		VO,
		COUNT
	}
	
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
            Debug.Log( "!** No sound controller object found (SoundController)" );
        }
        else
        {
            ms_soundController = (EPSoundController)soundControllerObject.GetComponent( typeof(EPSoundController) );
        }
		
		if( ms_soundController != null )
	        return ms_soundController;
		
		Debug.Log( "!** Couldn't get EPSoundController component" );
		return null;
    }
	
	// Use this for initialization
	void Start()
	{
		populateLists();
		
		m_MixGroupVolumes = new float[(int)MixGroup.COUNT];
		
		m_MixGroupVolumes[(int)MixGroup.SFX] = 1.0f;
		m_MixGroupVolumes[(int)MixGroup.MUSIC] = 1.0f;
		m_MixGroupVolumes[(int)MixGroup.VO] = 1.0f;
		
		/// Listen for EPMusicPlayer beat notifications
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyBeat");
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyHalfBeat");
		NotificationCenter.DefaultCenter.AddObserver(this, "NotifyQuarterBeat");
		//*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( m_PopulateList )
		{
			populateLists();			
			m_PopulateList = false;
		}


		/*// Play sound with spacebar
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Play("Motion_special_rise_01");
		}//*/
		
		/*// Play sound with return
		if (Input.GetKeyUp(KeyCode.Return))
		{
			Play("Motion_long_super_01");
		}//*/
		
		/*
		// Pause sounds
		if (Input.GetKeyUp (KeyCode.P))
			Pause();
		
		// Resume sounds
		if (Input.GetKeyUp (KeyCode.R))
			Resume();
		*/
	}
	
	// Play sound by name
	public void Play( string sound_name )
	{
		int soundIdx = GetIndex ( sound_name );
		Play ( soundIdx );
	}
	
	public void Play( string sound_name, float volume, float pitch )
	{
		int soundIdx = GetIndex( sound_name );
		Play( soundIdx, volume, pitch );
		//Debug.Log("Play sound: " + sound_name);
	}
	
	// Play sound by list ID
	public void Play( int sound_idx )
	{
		m_EPSoundEventList[sound_idx].Play ();
	}
	
	public void Play( int sound_idx, float volume, float pitch )
	{
		m_EPSoundEventList[sound_idx].Play( volume, pitch );
	}
	
	// Play sound by object reference
	public void Play( EPSoundEvent sound )
	{
		sound.Play( 1.0f, 1.0f );
	}
	
	public void Play( EPSoundEvent sound, float volume, float pitch )
	{
		sound.Play( volume, pitch );
	}
	
	// Queue sounds in the Sting queue
	public void PlaySting ( string sound_name )
	{
		int soundIdx = GetIndex ( sound_name );
		
		if ( EPMusicPlayer.Get ().m_MasterSegment == null || !EPMusicPlayer.Get().m_MasterSegment.IsPlaying() )
			Play ( soundIdx );
		else
			AddToStingQueue ( soundIdx );
	}
	
	public void AddToStingQueue ( int sound_idx )
	{
		if ( !m_StingQueue.Contains(m_EPSoundEventList[sound_idx]) )
		{
			m_StingQueue.Add(m_EPSoundEventList[sound_idx]);
		}
	}
	
	public void ClearStingQueue ()
	{
		for ( int i = 0; i < m_StingQueue.Count; i++ )
		{
			m_StingQueue[i].Play();
		}
		m_StingQueue.Clear();
	}
	
	public void SetFade ( string sndName, float endVol, float endPitch, float duration, bool isFadeOut )
	{
		m_EPSoundEventList[GetIndex( sndName )].SetFade( endVol, endPitch, duration, isFadeOut );
	}
	
	// Do stuff on beat notifications from EPMusicPlayer
	public void NotifyBeat ()
	{
		if ( m_StingGrid == StingGrid.BEAT )
			ClearStingQueue();
	}
	
	public void NotifyHalfBeat ()
	{
		if ( m_StingGrid == StingGrid.HALFBEAT )
			ClearStingQueue();
	}
	
	public void NotifyQuarterBeat ()
	{
		if ( m_StingGrid == StingGrid.QUARTERBEAT )
			ClearStingQueue();
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
	
	public void SetMixGroupVolume ( MixGroup grp, float vol )
	{
		m_MixGroupVolumes[(int)grp] = vol;
		foreach ( EPSoundEvent snd in m_EPSoundEventList )
		{
			snd.SetVolume ( snd.GetEventVolume() );
		}
	}
	
	// Functions for handling lists and sound name string checking
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
		
		Debug.Log("Sound lists updated.");
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
			Debug.Log("Duplicate name found: " + name + ". List cannot contain duplicates");
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
			//Debug.Log ("Index: " + idx);
			if( sound_name == m_EPSoundNames[idx] )
				return idx;
		}
		
		Debug.Log("Sound name not found");
		return 0;
	}
}
