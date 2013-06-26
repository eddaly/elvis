using UnityEngine;
using System.Collections;

public sealed class PerformanceAnalysis 
{
	//	A static instance is only instantiated when first accessed at runtime
	static readonly PerformanceAnalysis instance = new PerformanceAnalysis();
	public static PerformanceAnalysis Get
	{
		get
		{
			 return instance;
		}
	}
	
	System.Diagnostics.Stopwatch m_StopWatch = null;
	System.TimeSpan m_oldTime;
	
	public void SpitOutTiming( System.String name, bool show_timing )
	{
		if( m_StopWatch == null )
		{
			m_StopWatch = new System.Diagnostics.Stopwatch();			
			m_StopWatch.Start();
		}
		
		System.TimeSpan ts = m_StopWatch.Elapsed - m_oldTime;
						
		if( show_timing )
		{
			Debug.Log( name + ": " + System.String.Format("{0:00}:{1:00}:{2:00}.{3:000}", 
	                    ts.Hours, ts.Minutes, ts.Seconds, 
	                    ts.Milliseconds) );
		}
		else
		{
			Debug.Log( name );
		}
		
		m_oldTime = m_StopWatch.Elapsed;
	}
}
