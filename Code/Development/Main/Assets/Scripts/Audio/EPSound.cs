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
	public EPSoundController.MixGroup m_MixGroup;
	public double m_MinRepeatDelay = 0;
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
	double m_FadeStartTime;
	double m_FadeDuration;
	
	double m_LastTriggerTime = 0;
	
	// Use this for initialization
	void Start ()
	{
		GetSize();
		m_MixGroup = EPSoundController.MixGroup.SFX;
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
		Play (1.0f, 1.0f, 0.0f);
	}
	
	public override void Play( float volume, float pitch )
	{
		Play ( volume, pitch, 0.0f );
	}
	
	public override void PlayScheduled ( double delay )
	{
		Play ( 1.0f, 1.0f, delay );
	}
	
	public override void PlayScheduled ( float volume, float pitch, double delay )
	{
		Play ( volume, pitch, delay );
	}

	public override void Play ( float volume, float pitch, double delay )
	{
		if ( m_MinRepeatDelay == 0 || AudioSettings.dspTime - m_LastTriggerTime > m_MinRepeatDelay )
		{
			m_UpdatePlayIndex();
			
			// Set Volume & Pitch
			m_Sources[m_PlayIndex].volume = volume * ( m_Volume + ( (m_VolumeVariance * Random.value) - (m_VolumeVariance / 2) ) );
			m_Sources[m_PlayIndex].pitch = pitch * ( m_Pitch + ( (m_PitchVariance * Random.value) - (m_PitchVariance / 2) ) );
			
			float globalVol = RL.m_SoundController.m_GlobalSFXVolume;
			
			// Adjust for Global / MixGroup volumes
			if ( m_MixGroup == EPSoundController.MixGroup.MUSIC )
				globalVol = RL.m_SoundController.m_GlobalMusicVolume;
					
			m_Sources[m_PlayIndex].volume *= ( RL.m_SoundController.m_MixGroupVolumes[(int)m_MixGroup] * globalVol );
			
			// Play Sound
			if ( delay > 0 )
				m_Sources[m_PlayIndex].PlayScheduled( delay );
			else
				m_Sources[m_PlayIndex].Play();
			
			m_LastTriggerTime = ( AudioSettings.dspTime + delay );
			
			//Console.WriteLine("Playing sound " + (m_PlayIndex+1) + " of " + m_Size);
		}
		else
		{
			//Debug.Log("Too soon " + this.name);
		}
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
				//Debug.Log("Pausing: " + source.name);
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
				//Debug.Log("Resuming: " + source.name);
			}
		}
	}
	
	public override void SetVolume( float volume )
	{
		m_Volume = volume;
		
		foreach ( AudioSource source in m_Sources )
		{
			source.volume = m_Volume * RL.m_SoundController.m_MixGroupVolumes[(int)m_MixGroup];
		}
	}
	
	public override void SetPitch( float pitch )
	{
		m_Pitch = pitch;
		
		foreach ( AudioSource source in m_Sources )
		{
			source.pitch = m_Pitch;
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
	
	public override float GetEventVolume()
	{
		return m_Volume;
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
	
	public override void SetFade( float endVol, float endPitch, double duration, bool isFadeOut )
	{
		m_FadeStartTime = AudioSettings.dspTime;
		m_FadeDuration = duration;
		m_FadeStartVol = GetEventVolume();
		m_FadeStartPitch = GetPitch();
		m_FadeEndVol = endVol;
		m_FadeEndPitch = endPitch;
		m_StopAfterFade = isFadeOut;
		m_Fading = true;
	}
	
	public override void ApplyFade()
	{
			double fadeClock = AudioSettings.dspTime - m_FadeStartTime;
			if ( fadeClock < m_FadeDuration )
			{
				if ( m_FadeStartVol != m_FadeEndVol )
				{
					float vol = m_FadeStartVol + ((float)( fadeClock / m_FadeDuration ) * ( m_FadeEndVol - m_FadeStartVol ));
					SetVolume ( vol );
				}
				if ( m_FadeStartPitch != m_FadeEndPitch )
				{
					float pitch = m_FadeStartPitch + ((float)( fadeClock / m_FadeDuration ) * ( m_FadeEndPitch - m_FadeStartPitch ));
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
	
	public override double time()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
				return source.time;
		}
		return 0.0f;
	}
	
	public override void time( double time )
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null && source.isPlaying )
				source.time = (float)time;
		}
	}
}
