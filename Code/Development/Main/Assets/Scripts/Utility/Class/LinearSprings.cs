using UnityEngine;
using System.Collections;

public class LinearFloatSpring
{
	public float m_Pos;
	public float m_Vel;
	public float m_Target;
	public float m_ApproachTime;		//	Seconds to reach the target
	
	public void Initialise()
	{
		ClearAll();
	}
	
	public void ClearAll() 
	{
		m_Pos = 0.0f;
		m_Vel = 0.0f;
		m_Target = 0.0f;
		m_ApproachTime = 0.1f;
	}
	
	public void SetAll( float new_pos, float new_target, float new_approach_time )
	{
		m_Pos = new_pos;
		m_Target = new_target;
		m_ApproachTime = new_approach_time;
		
		float dist = m_Target - m_Pos;
		m_Vel = dist/m_ApproachTime;
	}

	public void SetTarget( float new_target )
	{
		m_Target = new_target;

		float dist = m_Target - m_Pos;
		m_Vel = dist/m_ApproachTime;
	}
	
	public void Update( float d_time ) 
	{
		float initialDiff = m_Target - m_Pos;
		m_Pos += m_Vel*d_time;
		float newDiff = m_Target - m_Pos;
		
		bool snap = false;
		if( initialDiff < 0.0f )
		{
			if( newDiff >= 0.0f )
				snap = true;
		}
		else
		{
			if( newDiff < 0.0f )
				snap = true;
		}
		
		//	If we've hit the target
		if( snap )
		{
			m_Pos = m_Target;
			m_Vel = 0.0f;
		}
	}
}

public class LinearVector3Spring
{
	private LinearFloatSpring m_xSpring = new LinearFloatSpring();
	private LinearFloatSpring m_ySpring = new LinearFloatSpring();
	private LinearFloatSpring m_zSpring = new LinearFloatSpring();
	
	public Vector3 m_Pos;
	public Vector3 m_Target;
	public Vector3 m_ApproachTime;
	
	public void Initialise()
	{
		ClearAll();
	}
	
	public void ClearAll()
	{
		m_Pos = new Vector3( 0.0f, 0.0f, 0.0f );
		m_xSpring.m_Vel = 0.0f;
		m_ySpring.m_Vel = 0.0f;
		m_zSpring.m_Vel = 0.0f;
		m_Target = new Vector3( 0.0f, 0.0f, 0.0f );
		m_ApproachTime = new Vector3( 0.0f, 0.0f, 0.0f );
		
		m_xSpring.ClearAll();
		m_ySpring.ClearAll();
		m_zSpring.ClearAll();
	}
	
	public void Set( Vector3 new_pos, Vector3 new_target, Vector3 new_approach_time )
	{
		m_Pos = new_pos;
		m_Target = new_target;
		m_ApproachTime = new_approach_time;

		m_xSpring.SetAll( m_Pos.x, m_Target.x, m_ApproachTime.x );
		m_ySpring.SetAll( m_Pos.y, m_Target.y, m_ApproachTime.y );
		m_zSpring.SetAll( m_Pos.z, m_Target.z, m_ApproachTime.z );
	}
	public void Set( Vector3 new_pos, Vector3 new_target )
	{
		m_Pos = new_pos;
		m_Target = new_target;

		m_xSpring.SetAll( m_Pos.x, m_Target.x, m_ApproachTime.x );
		m_ySpring.SetAll( m_Pos.y, m_Target.y, m_ApproachTime.y );
		m_zSpring.SetAll( m_Pos.z, m_Target.z, m_ApproachTime.z );
	}

	public void SetApproachTime( Vector3 new_approach_time )
	{
		m_ApproachTime = new_approach_time;
		m_xSpring.m_ApproachTime = m_ApproachTime.x;
		m_ySpring.m_ApproachTime = m_ApproachTime.y;
		m_zSpring.m_ApproachTime = m_ApproachTime.z;
	}
	
	public void Update( float d_time ) 
	{
		m_xSpring.Update( d_time );
		m_ySpring.Update( d_time );
		m_zSpring.Update( d_time );
		
		m_Pos.x = m_xSpring.m_Pos;
		m_Pos.y = m_ySpring.m_Pos;
		m_Pos.z = m_zSpring.m_Pos;
	}
}