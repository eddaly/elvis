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
	public float m_LoopPoint;
			
	public enum CueType
	{
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
	}
	
	// Update is called once per frame
	void Update ()
	{
	}// Update	
	
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
