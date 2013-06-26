using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EPSoundScript : EPSoundEvent {
	
	// EPSoundTrigger
	public class EPSoundScriptTrigger
	{
		public string m_soundName;
		public float m_time;
		public int m_layer;
		public float m_volume;
		public float m_pitch;
		
		// Constructor
		public EPSoundScriptTrigger(string soundName, float time, int layer, float volume, float pitch)
		{
			m_soundName = soundName;
			m_time = time;
			m_layer = layer;
			m_volume = volume;
			m_pitch = pitch;
		}
	}
	
	// Create a list of EPSoundTriggers
	public List<EPSoundScriptTrigger> m_TriggerList = new List<EPSoundScriptTrigger>();
	
	// A float to hold the timestamp of the last update, so we can evaluate whether an event should have occurred
	float m_startTime = 0.0f;
	float m_lastUpdate = -0.1f;
	bool m_isActive = false;
	
	// Use this for initialization
	void Start ()
	{
		// Trigger list here
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( m_isActive )
		{
			float seqTime = GlobalData.Get.m_GlobalTime - m_startTime; // Placeholder
			
			for ( int i = 0; i < m_TriggerList.Count; i++)
			{	
				float t = m_TriggerList[i].m_time;
				
				// Is the sound in the active time range?
				if ( t <= seqTime && t > m_lastUpdate )
				{
					Console.WriteLine (seqTime + ": " + m_TriggerList[i].m_soundName);
					EPSoundController.Get().Play( m_TriggerList[i].m_soundName, m_TriggerList[i].m_volume, m_TriggerList[i].m_pitch );
				}
			}
			
			if (seqTime >= m_TriggerList[m_TriggerList.Count-1].m_time)
			{
				m_isActive = false;
				//Console.WriteLine ("Event finished");
			}
			
			// Set m_lastUpdate to the current time, ready for the next update
			m_lastUpdate = seqTime;
		}		
	}
	
	public override void Play ()
	{
		Play (1.0f , 1.0f);
	}
	
	public override void Play ( float volume, float pitch )
	{
		m_startTime = GlobalData.Get.m_GlobalTime;
		m_lastUpdate = -0.1f;
		m_isActive = true;
	}
	
	public override void Stop ()
	{
		m_isActive = false;
	}
	
	public override bool IsPlaying()
	{
		if (m_isActive)
			return true;
		else
			return false;
	}
}
