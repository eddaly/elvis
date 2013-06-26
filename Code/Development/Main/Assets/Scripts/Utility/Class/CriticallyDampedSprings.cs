//-----------------------------------------------------------------------------
// File: CriticallyDampedSpring.cs
//
// Desc:	Spring that converges on a target value within a specified time
//			without overshooting.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;


public class CriticallyDampedFloatSpring
{
	public float m_Pos;
	public float m_Vel;
	public float m_Target;
	public float m_ApproachTime;
	
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
	
	public void SetAll( float new_pos, float new_vel, float new_target, float new_approach_time )
	{
		m_Pos = new_pos;
		m_Vel = new_vel;
		m_Target = new_target;
		m_ApproachTime = new_approach_time;
	}	
	
	public void Update( float d_time ) 
	{
		if( d_time > 0.001f )
		{
			float currentToTarget = m_Target - m_Pos;
		    float springForce = currentToTarget*m_ApproachTime;
		    float dampingForce = -m_Vel *2*Mathf.Sqrt( m_ApproachTime );
		    float force = springForce + dampingForce;
		    m_Vel += force*d_time;
		    float displacement = m_Vel*d_time;
		    m_Pos += displacement;				
		}
	}
	
	public bool Snap( float snap_dist )
	{
		float diff = m_Target - m_Pos;
		
		if( diff < snap_dist && diff > -snap_dist )
		{
			m_Pos = m_Target;
			return true;
		}
		
		return false;
	}
	
	public void DeadStop()
	{
		m_Vel = 0.0f;
	}
}

public class CriticallyDampedVector3Spring
{
	private CriticallyDampedFloatSpring m_xSpring = new CriticallyDampedFloatSpring();
	private CriticallyDampedFloatSpring m_ySpring = new CriticallyDampedFloatSpring();
	private CriticallyDampedFloatSpring m_zSpring = new CriticallyDampedFloatSpring();
	
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
	
	public void SetPos( Vector3 new_pos )
	{
		m_Pos = new_pos;
		m_xSpring.m_Pos = m_Pos.x;
		m_ySpring.m_Pos = m_Pos.y;
		m_zSpring.m_Pos = m_Pos.z;
	}

	public void SetTarget( Vector3 new_target )
	{
		m_Target = new_target;
		m_xSpring.m_Target = m_Target.x;
		m_ySpring.m_Target = m_Target.y;
		m_zSpring.m_Target = m_Target.z;
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
		if( d_time > 0.001f )
		{
			m_xSpring.Update( d_time );
			m_ySpring.Update( d_time );
			m_zSpring.Update( d_time );
		}
		
		m_Pos.x = m_xSpring.m_Pos;
		m_Pos.y = m_ySpring.m_Pos;
		m_Pos.z = m_zSpring.m_Pos;
	}
	
	public void DeadStop()
	{
		m_xSpring.m_Vel = 0.0f;
		m_ySpring.m_Vel = 0.0f;
		m_zSpring.m_Vel = 0.0f;
	}
}