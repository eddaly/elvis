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
	//public EPMusicGlobals.MIDIpitch m_RootNote = EPMusicGlobals.MIDIpitch.C_4;
	
	public enum PlayOrder {
		RANDOM,
		CYCLE
	}
	
	// Private variables
	int m_Size;
	int m_PlayIndex = -1;
	
	// Fade controls
	[HideInInspector]
	public bool m_Fading = false;
	bool m_StopAfterFade = false;
	float m_FadeStartVol;
	float m_FadeEndVol;
	float m_FadeStartPitch;
	float m_FadeEndPitch;
	float m_FadeStartTime;
	float m_FadeDuration;
	
	// Use this for initialization
	void Start ()
	{
		GetSize();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( m_Fading )
		{
			//Debug.Log("Fading");
			ApplyFade();
		}
	}// Update
	
	void GetSize()
	{
		m_Size = m_Sources.GetLength(0);
	}
	
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
			Debug.LogWarning ("Invalid PlayOrder");
			m_PlayIndex = 0;
		}
	}
	
	// Overrides for EPSoundEvent functions
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
	
	public override void SetVolume( float volume )
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source.isPlaying )
			{
				source.volume = volume;
			}
		}
	}
	
	public override void SetPitch( float pitch )
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source.isPlaying )
			{
				source.pitch = pitch;
			}
		}
	}
	
	public override float GetVolume()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
			{
				return source.volume;
			}
		}
		return 0.0f;
	}
	
	public override float GetPitch()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
			{
				return source.pitch;
			}
		}
		return 0.0f;
	}
	
	public override void SetFade( float endVol, float endPitch, float duration, bool isFadeOut )
	{
		m_FadeStartTime = Time.time;
		m_FadeDuration = duration;
		m_FadeStartVol = GetVolume();
		m_FadeStartPitch = GetPitch();
		m_FadeEndVol = endVol;
		m_FadeEndPitch = endPitch;
		m_StopAfterFade = isFadeOut;
		m_Fading = true;
	}
	
	public override void ApplyFade()
	{
			float fadeClock = Time.time - m_FadeStartTime;
			if ( fadeClock < m_FadeDuration )
			{
				if ( m_FadeStartVol != m_FadeEndVol )
				{
					float vol = m_FadeStartVol + (( fadeClock / m_FadeDuration ) * ( m_FadeEndVol - m_FadeStartVol ));
					SetVolume ( vol );
				}
				if ( m_FadeStartPitch != m_FadeEndPitch )
				{
					float pitch = m_FadeStartPitch + (( fadeClock / m_FadeDuration ) * ( m_FadeEndPitch - m_FadeStartPitch ));
					SetPitch ( pitch );
				}
			}
			else if ( !m_StopAfterFade )
			{
				SetVolume ( m_FadeEndVol );
				SetPitch ( m_FadeEndPitch );
				m_Fading = false;
			}
			else
			{
				Stop ();
				m_Fading = false;
			
				// Restore original pitch/volume after fade out
				SetVolume ( m_FadeStartVol );
				SetPitch ( m_FadeStartPitch );
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
