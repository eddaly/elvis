using UnityEngine;
using System.Collections;

public class EPMusicPlayer : MonoBehaviour {

	public EPMusicSegment[] m_Segments = new EPMusicSegment[1];
	public float m_BPM;
	public float m_ClockSeconds;
	public float m_ClockBeats;
	public float m_LoopBeats;
	
	public EPMusicSegment m_MasterSegment = null;
	
	EPMusicSegment m_QueuedSegment = null;
	bool m_QueuedMaster;
	float m_QueueTime = -1;
	
	public float m_loopCountdown;
	public float m_syncRes = 0.0333f;
	float m_syncResBeats;
	
	int m_LastNotifiedBeat = 0;
	int m_LastNotifiedHalfBeat = 0;
	int m_LastNotifiedQuarterBeat = 0;
	
	public enum Flags
	{
		None = 0,
		IsMaster = 1,
		Cue_Grid = 2,
		Cue_Bar = 4,
		Cue_Point = 8,
		Cue_End = 16
	}
	
	//	Pseudo-singleton pattern
	private static EPMusicPlayer ms_musicPlayer = null;
    public static EPMusicPlayer Get()
    {
		//	Cache the object to avoid frequent string lookups
		if( ms_musicPlayer != null )
			return ms_musicPlayer;

		GameObject musicPlayerObject = GameObject.Find( "SoundController" );
        if( musicPlayerObject == null )
        {
            Debug.Log( "!** No Music Player object found (SoundController)" );
        }
        else
        {
            ms_musicPlayer = (EPMusicPlayer)musicPlayerObject.GetComponent( typeof(EPMusicPlayer) );
        }
		
		if( ms_musicPlayer != null )
	        return ms_musicPlayer;
		
		Debug.Log( "!** Couldn't get EPMusicPlayer component" );
		return null;
    }
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateMasterLoop();
		UpdateClock();
		
		if ( m_MasterSegment != null )
		{
			DoNotifyBeat();
			DoNotifyHalfBeat();
			DoNotifyQuarterBeat();
		}
		
		if ( m_QueuedSegment != null )
		{
			ProcessSegmentQueue();
		}
		
		DebugInputs();
	}
	
	// Play functions
	public void PlaySegment ( string segName, Flags flags = Flags.None )
	{
		int i = GetSegmentIndex( segName );
		if ( i >= 0 )
			PlaySegment ( i, flags );
	}
	
	public void PlaySegment ( int i, Flags flags = Flags.None )
	{
		EPMusicSegment seg = m_Segments[i];
		
		if ( seg != null )
		{
			if ( m_MasterSegment == null )
			{
				SetMaster(seg);
				seg.Play();
			}
			else if ( seg.m_CueType == EPMusicSegment.CueType.INSTANT )
			{
				seg.SetTimeSamples( m_MasterSegment.GetTimeSamples() );
				
				if ( ( flags & Flags.IsMaster ) == Flags.IsMaster )
				{
					SetMaster(seg);
				}
				
				seg.Play();
				//Debug.Log("Play segment " + i );
			}
			else
			{
				if ( seg.m_CueType == EPMusicSegment.CueType.POINT )
				{
					m_QueueTime = seg.m_CuePoint;
				}
				else if ( seg.m_CueType == EPMusicSegment.CueType.GRID )
				{
					m_QueueTime = GetNextGrid();
				}
				else if ( seg.m_CueType == EPMusicSegment.CueType.BAR )
				{
					m_QueueTime = GetNextBar();
				}
				else if ( seg.m_CueType == EPMusicSegment.CueType.END )
				{
					m_QueueTime = m_MasterSegment.m_LoopPoint;
					//Debug.Log ("m_QueueTime = " + m_QueueTime);
				}
				else
				{
					Debug.Log("Can't queue segment, no Cue Type defined");
				}
				
				m_QueuedSegment = seg;
				
				if ( ( flags & Flags.IsMaster ) == Flags.IsMaster )
				{
					m_QueuedMaster = true;
				}
					
				//Debug.Log("Queue segment " + i );
			}
		}
	}
	
	// Toggle (play) functions
	public void ToggleSegment ( string segName, Flags flags = Flags.None  )
	{
		int i = GetSegmentIndex( segName );
		if ( i >= 0 )
			ToggleSegment ( i, flags );
	}
	
	public void ToggleSegment ( int i, Flags flags = Flags.None  )
	{
		if ( !m_Segments[i].IsPlaying() )
			{
				//Debug.Log("Starting");
				PlaySegment(i, flags);
			}
			else
			{
				StopSegment(i);
			}
	}
	
	// Stop functions
	public void StopSegment ( string segName )
	{
		int i = GetSegmentIndex( segName );
		if ( i >= 0 )
			StopSegment ( i );
	}
	
	public void StopSegment( int i )
	{
		EPMusicSegment seg = m_Segments[i];
		
		if ( seg != null )
		{
			seg.Stop();
			//Debug.Log("Stop segment " + i );
			if ( seg == m_MasterSegment )
				m_MasterSegment = null;
		}
		
		ClearQueue();
	}	
	
	// TogglePause functions
	public void TogglePauseSegment ( string segName )
	{
		int i = GetSegmentIndex( segName );
		if ( i >= 0 )
			TogglePauseSegment ( i );
	}
	
	public void TogglePauseSegment ( int i )
	{
		if ( !m_Segments[i].IsPlaying() )
			{
				PlaySegment(i);
			}
			else
			{
				PauseSegment(i);
			}
	}
	
	// Pause functions
	public void PauseSegment ( string segName )
	{
		int i = GetSegmentIndex( segName );
		if ( i >= 0 )
			PauseSegment ( i );
	}
	
	public void PauseSegment( int i )
	{
		EPMusicSegment seg = m_Segments[i];
		
		if ( seg != null )
		{
			seg.Pause();
			//Debug.Log("Pause segment " + i );
		}
	}
	
	// Deal with queued segments
	public void ProcessSegmentQueue ()
	{
		if ( m_QueueTime >= 0 )
		{
			//Debug.Log ("m_QueueTime = " + m_QueueTime);
			float now = m_MasterSegment.GetTime();
			
			// Hi-resolution sync
			float triggerDelta = m_QueueTime - now;
			
			if ( triggerDelta < m_syncRes )
			{
				m_QueuedSegment.SetTime( m_QueueTime % m_QueuedSegment.m_LoopPoint );
				m_QueuedSegment.m_Sources[0].PlayDelayed( triggerDelta );
				//Debug.Log ("Playing at " + now + " + " + triggerDelta + " delay");
				
				if ( m_QueuedMaster == true )
				{
					//Debug.Log ("Stop master now");
					m_MasterSegment.Stop();
					SetMaster(m_QueuedSegment);
					m_QueuedSegment = null;
				}
			}
		}
	}
	
	
	// Get segment index from name, used for play/toggle/pause functions
	public int GetSegmentIndex ( string segName )
	{
		for ( int i = 0; i < m_Segments.Length; i++ )
		{
			if ( m_Segments[i].name == segName )
			{
				return i;
			}
		}
		Debug.Log("Segment " + segName + " not found");
		return -1;
	}
	
	public void StopAll()
	{
		foreach ( EPMusicSegment segment in m_Segments )
		{
			segment.Stop();
		}
	}
	
	public void PlayOneShot(string name, float pitchOffset)
	{
		PitchTranslateRelative(pitchOffset);
		EPSoundController.Get().Play(name, 1.0f, 1.0f);
	}
	
	float PitchTranslateRelative(float semitones)
	{
		float outPitch = Mathf.Pow( 1.05946f, semitones );
		return outPitch;
	}
	
	void UpdateClock()
	{
		if ( m_MasterSegment != null )
		{
			m_ClockSeconds = m_MasterSegment.GetTime();
			m_ClockBeats = m_ClockSeconds * ( m_BPM / 60.0f );
			m_syncResBeats = m_syncRes * ( m_BPM / 60.0f );
		}
		else
		{
			m_ClockSeconds = 0.0f;
			m_ClockBeats = 0.0f;
			m_syncResBeats = 0.0f;
		}
	}
	
	// Calculate whether we're within SyncRes duration of next loop point, if so play with delay set to countdown time
	void UpdateMasterLoop()
	{
		if ( m_MasterSegment == null || !m_MasterSegment.IsPlaying() )
		{
			m_MasterSegment = GetMasterSegment();
		}
		
		if ( m_MasterSegment != null )
		{
			m_LoopBeats = m_MasterSegment.m_LoopPoint * m_BPM/60;
			
			m_loopCountdown = m_MasterSegment.m_LoopPoint - m_MasterSegment.GetTime();
			
			if ( m_loopCountdown < m_syncRes )
			{
				//Debug.Log ("Loop frame @ " + m_loopCountdown);
				RetriggerSegments(m_loopCountdown);
			}
			
			m_BPM = m_MasterSegment.m_BPM;
		}
	}
	
	// Retrigger all currently playing segments, with delay argument for tight sync
	void RetriggerSegments( float delay)
	{
		foreach ( EPMusicSegment seg in m_Segments )
		{
			if ( seg.IsPlaying() )
			{				
				//Debug.Log ("Playing " + seg + " with delay: " + delay);
				
				seg.m_Sources[0].PlayDelayed(delay);
				seg.SetTimeSamples(0);
			}
		}
		if ( m_QueuedSegment != null )
		{
			if ( m_QueuedMaster )
			{
				m_MasterSegment.Stop();
				SetMaster ( m_QueuedSegment );
			}
			m_QueuedSegment.m_Sources[0].PlayDelayed(delay);
			m_QueuedSegment.SetTimeSamples(0);
			ClearQueue();
		}
	}
	
	// Set a segment as MasterSegment
	public void SetMaster( EPMusicSegment seg )
	{
		m_MasterSegment = seg;
	}
	
	// Return the MasterSegment object
	EPMusicSegment GetMasterSegment()
	{
		if ( m_MasterSegment == null )
		{
			foreach ( EPMusicSegment seg in m_Segments )
			{
				if ( seg.IsPlaying() )
					return seg;
			}
			return null;
		}
		return m_MasterSegment;
	}
	
	public bool IsPlaying()
	{
		if ( m_MasterSegment != null && m_MasterSegment.IsPlaying() )
		{
			return true;
		}
		return false;
	}
	
	// Broadcast every beat
	void DoNotifyBeat()
	{
		//int i = (int)m_ClockBeats;
		
		// Send notification early if close to beat boundary
		int i = (int)( ( m_ClockBeats ) + ( m_syncResBeats / 2 ) );
		
		if ( i != m_LastNotifiedBeat )
		{
			NotificationCenter.DefaultCenter.PostNotification(this, "NotifyBeat");
		}
		
		m_LastNotifiedBeat = i;
	}
	
	// Broadcast every half beat
	void DoNotifyHalfBeat()
	{
		// Send notification early if close to beat boundary
		int i = (int)( ( m_ClockBeats * 2 ) + ( m_syncResBeats / 2 ) );
		
		if ( i != m_LastNotifiedHalfBeat )
		{
			NotificationCenter.DefaultCenter.PostNotification(this, "NotifyHalfBeat");
		}
		
		m_LastNotifiedHalfBeat = i;
	}
	
	// Broadcast every half beat
	void DoNotifyQuarterBeat()
	{
		// Send notification early if close to beat boundary
		int i = (int)( ( m_ClockBeats * 4 ) + ( m_syncResBeats / 2 ) );
		
		if ( i != m_LastNotifiedQuarterBeat )
		{
			NotificationCenter.DefaultCenter.PostNotification(this, "NotifyQuarterBeat");
		}
		
		m_LastNotifiedQuarterBeat = i;
	}
		// Move all of these to the player
	float GetNextGrid()
	{
		float next;
		float now = m_MasterSegment.GetTime();
		float beat = 60.0f / m_BPM;
		float grid = beat * ( 4 / m_MasterSegment.m_GridSize );
		//Debug.Log("Grid size: " + grid);
		
		next = ( (int)( now / grid ) + 1 ) * grid;
		//Debug.Log ("Now: " + now + " Next: " + next);
		
		return next;
	}
	
	float GetNextBar()
	{
		float next;
		float now = m_MasterSegment.GetTime();
		float beat = 60.0f / m_BPM;
		float bar = beat * m_MasterSegment.m_TimeSignatureUpper;
		//Debug.Log("Grid size: " + grid);
		
		next = ( (int)( now / bar ) + 1 ) * bar;
		//Debug.Log ("Now: " + now + " Next: " + next);
		
		return next;
	}
	
	public void ClearQueue()
	{
		m_QueuedSegment = null;
		m_QueuedMaster = false;
	}
	
	void DebugInputs()
	{
		// Debug inputs
		/*//
		if ( Input.GetKeyDown (KeyCode.Q) )
		{
			ToggleSegment(0);
		}
		if ( Input.GetKeyDown (KeyCode.W) )
		{
			ToggleSegment(1);
		}
		if ( Input.GetKeyDown (KeyCode.E) )
		{
			ToggleSegment(2);
		}
		if ( Input.GetKeyDown (KeyCode.R) )
		{
			StopAll();
		}//*/
	}
}
