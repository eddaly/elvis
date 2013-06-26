//-----------------------------------------------------------------------------
// File: EPSound.cs
//
// Desc:	Class to wrap AudioSources with additional functionality and play
//			them
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class EPSound : EPSoundEvent {
	
	// Public variables visible in Inspector
	public AudioSource[] m_Sources = new AudioSource[1];
	public float m_Volume = 1.0f;
	public float m_VolumeVariance;
	public float m_Pitch = 1.0f;
	public float m_PitchVariance;
	public PlayOrder m_PlayOrder;
	
	public enum PlayOrder {
		RANDOM,
		CYCLE
	}
	
	// Private variables
	int m_Size;
	int m_PlayIndex = -1;
	
	// Use this for initialization
	void Start ()
	{
		m_Size = m_Sources.GetLength(0);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}// Update
	
	void m_UpdatePlayIndex()
	{
		// Play Order
		if (m_PlayOrder == PlayOrder.CYCLE)
		{
			m_PlayIndex++;
			
			if (m_PlayIndex >= m_Size)
			{
				m_PlayIndex = 0;
			}
		}
		else if (m_PlayOrder == PlayOrder.RANDOM)
		{
			m_PlayIndex = (int)(Random.Range(0,m_Size-1));
			//Console.WriteLine ("playIndex = " + m_PlayIndex);
		}
		else
		{
			EchoDebugLog.LogWarning ("Invalid PlayOrder");
			m_PlayIndex = 0;
		}
	}
	
	
	public override void Play()
	{
		Play (1.0f, 1.0f);
	}

	public override void Play ( float volume, float pitch )
	{
		m_UpdatePlayIndex();
		
		// Set Volume & Pitch
		m_Sources[m_PlayIndex].volume = volume * ( m_Volume + ( (m_VolumeVariance * Random.value) - (m_VolumeVariance / 2) ) );
		m_Sources[m_PlayIndex].pitch = pitch * ( m_Pitch + ( (m_PitchVariance * Random.value) - (m_PitchVariance / 2) ) );
		
		m_Sources[m_PlayIndex].volume *= EPSoundController.Get().m_GlobalSFXVolume;
		
		// Play Sound
		m_Sources[m_PlayIndex].Play();

		//Console.WriteLine("Playing sound " + (m_PlayIndex+1) + " of " + m_Size);
	}
	
	public override void Stop()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null )
			{
				source.Stop();
			}
		}
	}
	
	public override void Pause()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
			{
				source.Pause();
			}
		}
	}
	
	public override void UpdateVolume(float newVolume)
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
			{
				source.volume = newVolume;
			}
		}
	}
	
	public override void Resume()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.time != 0 )
			{
				source.Play();
			}
		}
	}
	
	public override bool IsPlaying()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
				return true;
		}
		return false;
	}
	
	public override float time()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
				return source.time;
		}
		return 0.0f;
	}
	
	public override void time( float time )
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
				source.time = time;
		}
	}
}
