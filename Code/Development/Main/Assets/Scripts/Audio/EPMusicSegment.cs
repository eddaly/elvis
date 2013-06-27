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
	
	bool m_Queued = false;
	float m_QueueTime;
		
	public enum CueType
	{
		INSTANT,
		GRID,
		POINT
	}
	
	// Use this for initialization
	void Start ()
	{
		m_QueueTime = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ( m_Queued && m_QueueTime >= 0 )
		{
			float now = EPMusicPlayer.Get().m_MasterSegment.GetTime();
			
			// Hi-resolution sync
			float triggerDelta = m_QueueTime - now;
			
			if ( triggerDelta < EPMusicPlayer.Get().m_syncRes )
			{
				SetTime( m_QueueTime );
				this.m_Sources[0].PlayDelayed( triggerDelta );
				m_Queued = false;
				Debug.Log ("Playing at " + now + " + " + triggerDelta + " delay");
			}
		}
	}// Update	
	
	public override void Play ()
	{
		base.Play ();
	}
	
	
	// Member functions
	public void PlayQueued ()
	{
		if ( m_CueType == CueType.POINT )
		{
			m_QueueTime = m_CuePoint;
			m_Queued = true;
		}
		else if ( m_CueType == CueType.GRID )
		{
			m_QueueTime = GetNextGrid();
			m_Queued = true;
		}
	}
	
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
	
	float GetNextGrid()
	{
		float next;
		float now = EPMusicPlayer.Get().m_MasterSegment.GetTime();
		float beat = 60.0f / m_BPM;
		float grid = beat * m_GridSize;
		//Debug.Log("Grid size: " + grid);
		
		next = ( (int)( now / grid ) + 1 ) * grid;
		//Debug.Log ("Now: " + now + " Next: " + next);
		
		return next;
	}
}
