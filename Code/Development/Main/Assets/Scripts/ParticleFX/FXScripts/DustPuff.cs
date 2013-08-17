//-----------------------------------------------------------------------------
// File: DustPuff.cs
//
// Desc:	A little smokey puff of dust for impacts
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class DustPuff : MonoBehaviour 
{	
	ParticleFXEmitter m_puffs = null;	

	public bool m_TestInitialise = false;
	public bool m_TestPlay = false;
	public bool m_TestRelease = false;
	
	void Start() 
	{
	}
	
	public void Initialise()
	{
		m_puffs = gameObject.AddComponent<ParticleFXEmitter>();

		m_puffs.m_MaxParticles = 10;
		
		m_puffs.m_ParticlesPerSecond = 0.0f;
		m_puffs.m_ParticleSpawnPeriod = 0.0f;
			
		m_puffs.m_InstantSpawnParticles = 10;
	
		m_puffs.m_FireAndForget = false;
		m_puffs.m_TimeToForget = 0.0f;
		
		m_puffs.m_Additive = false;
		m_puffs.m_Texture = (int)TextureAtlas.ParticleFXAlpha.DUST;
		
		m_puffs.m_ParticleLifetimeMin = 0.1f;
		m_puffs.m_ParticleLifetimeMax = 0.3f;
		
		m_puffs.m_InitialRotationMin = 0.0f;
		m_puffs.m_InitialRotationMax = Mathf.PI*2.0f;
		m_puffs.m_RotationOverTimeMin = -Mathf.PI*0.5f;
		m_puffs.m_RotationOverTimeMax = Mathf.PI*0.5f;
		m_puffs.m_RotationPower = 3;
		
		m_puffs.m_InitialScaleMin = 0.1f;
		m_puffs.m_InitialScaleMax = 0.15f;
		m_puffs.m_ScaleOverTimeMin = 0.01f;
		m_puffs.m_ScaleOverTimeMax = 0.05f;
		m_puffs.m_ScalePower = 0;
	
		m_puffs.m_Colour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		m_puffs.m_ColourOverTime = new Color(-0.2f, -0.2f, -0.2f, -1.0f);
		m_puffs.m_ColourPower = -2;

		m_puffs.Initialise();
	}
	
	public void Play()
	{
		if( m_puffs != null )
			m_puffs.Play();
	}

	public void Stop()
	{
		if( m_puffs != null )
			m_puffs.Stop();
	}
	
	void Update()
	{	
		if( m_TestInitialise )
		{
			Initialise();
			m_TestInitialise = false;
		}

		if( m_TestPlay )
		{
			Play();
			m_TestPlay = false;
		}
		
		if( m_TestRelease )
		{
			Release();
			m_TestRelease = false;
		}
	}
	
	public void Release()
	{
		if( m_puffs != null )
			Destroy( m_puffs );
		m_puffs = null;
	}
}
