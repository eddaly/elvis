using UnityEngine;
using System.Collections;

public class EPMusicSegment : EPSound {
	
	public float m_BPM = 120.0f;
	public int m_TimeSignatureUpper = 4;
	public int m_TimeSignatureLower = 4;
	public int m_GridSize = 4;
	public bool m_CuePointSync = true;
	public CueType m_CueType;
	public float m_CuePoint;
	public bool m_Looping = true;
	public float m_LoopPoint;
	public float m_Duration;
			
	public enum CueType
	{
		NONE,
		INSTANT,
		GRID,
		BAR,
		POINT,
		END
	}
	
	// Use this for initialization
	void Start ()
	{
		if ( m_Sources[0] == null )
		{
			if ( this.audio != null )
				m_Sources[0] = this.audio;
			else
				Debug.Log("No valid audiosource available in EPMusicSegment " + this.name);
		}
		
		m_Duration = m_Sources[0].clip.length;
		m_MixGroup = EPSoundController.MixGroup.MUSIC;
		
		if ( m_LoopPoint == 0 )
				m_LoopPoint = m_Duration;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( m_Fading )
		{
			ApplyFade();
		}
	}// Update	
	
	// Overrides
	public override void Play()
	{
		base.Play();
	}
	
	public void PlayDelayed(float delay)
	{
		m_Sources[0].PlayDelayed(delay);
	}
	
	public override void Stop()
	{
		foreach ( AudioSource source in m_Sources )
		{
			if ( source != null )
			{
				source.Stop();
				if ( this == RL.m_MusicPlayer.m_MasterSegment )
				{
					RL.m_MusicPlayer.m_MasterSegment = null;
				}
				source.time = 0;
			}
		}
	}
	
	
	// Member functions
	public float GetTime()
	{
		if ( m_Sources[0] != null )
				return m_Sources[0].time;
		return -1.0f;
	}
	
	public int GetTimeSamples()
	{
		if ( m_Sources[0] != null )
				return m_Sources[0].timeSamples;
		return -1;
	}
	
	public void SetTime( float t )
	{
		m_Sources[0].time = t;
	}
	
	public void SetTimeSamples( int t )
	{
		m_Sources[0].timeSamples = t;
	}
	
}
