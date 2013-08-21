//-----------------------------------------------------------------------------
// File: ReservoirBlast.cs
//
// Desc:	Creates the shiney effect that happens when a succesfull action
//			shatters at the end of the phrase.
//
// Copyright Echo Peak Ltd 2012
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ReservoirBlast : MonoBehaviour 
{	
	ParticleFXEmitter m_glow = null;	
	ParticleFXEmitter m_shine = null;
	ParticleFXEmitter m_rings = null;

	public bool m_TestInitialise = false;
	public bool m_TestPlay = false;
	public bool m_TestRelease = false;
	
	void Start() 
	{
	}
	
	public void Initialise()
	{
		float shineMin = 0.8f;
		float shineMax = 1.0f;
		float shineGrowMin = 2.0f;
		float shineGrowMax = 2.5f;
		float shineLife = 0.3f;		

		float shineRotMin = -Mathf.PI/6.0f;
		float shineRotMax = Mathf.PI/6.0f;		
		
		m_glow = null;		
		m_shine = null;
		m_rings = null;
		
		m_glow = gameObject.AddComponent<ParticleFXEmitter>();

		m_glow.m_MaxParticles = 1;
		
		m_glow.m_ParticlesPerSecond = 0.0f;
		m_glow.m_ParticleSpawnPeriod = 0.0f;
			
		m_glow.m_InstantSpawnParticles = 1;
	
		m_glow.m_FireAndForget = false;
		m_glow.m_TimeToForget = 0.0f;
		
		m_glow.m_Texture = (int)TextureAtlas.ParticleFX.GLOW;
		
		m_glow.m_ParticleLifetimeMin = 0.1f;
		m_glow.m_ParticleLifetimeMax = 0.1f;
		
		m_glow.m_InitialRotationMin = 0.0f;
		m_glow.m_InitialRotationMax = 0.0f;
		m_glow.m_RotationOverTimeMin = 0.0f;
		m_glow.m_RotationOverTimeMax = 0.0f;
		m_glow.m_RotationPower = 0;
		
		m_glow.m_InitialScaleMin = 2.0f;
		m_glow.m_InitialScaleMax = 2.0f;
		m_glow.m_ScaleOverTimeMin = 0.0f;
		m_glow.m_ScaleOverTimeMax = 0.0f;
		m_glow.m_ScalePower = 0;
	
		m_glow.m_Colour = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		m_glow.m_ColourOverTime = new Color(-1.0f, -1.0f, -1.0f, -1.0f);
		m_glow.m_ColourPower = -2;

		m_glow.Initialise();
		

		m_shine = gameObject.AddComponent<ParticleFXEmitter>();

		m_shine.m_MaxParticles = 3;
		
		m_shine.m_ParticlesPerSecond = 0.0f;
		m_shine.m_ParticleSpawnPeriod = 0.0f;
			
		m_shine.m_InstantSpawnParticles = 3;
	
		m_shine.m_FireAndForget = false;
		m_shine.m_TimeToForget = 0.0f;
		
		m_shine.m_Texture = (int)TextureAtlas.ParticleFX.SHINE;
		
		m_shine.m_ParticleLifetimeMin = shineLife;
		m_shine.m_ParticleLifetimeMax = shineLife;
		
		m_shine.m_InitialRotationMin = 0.0f;
		m_shine.m_InitialRotationMax = Mathf.PI*2.0f;
		m_shine.m_RotationOverTimeMin = shineRotMin;
		m_shine.m_RotationOverTimeMax = shineRotMax;
		m_shine.m_RotationPower = 2;
		
		m_shine.m_InitialScaleMin = shineMin;
		m_shine.m_InitialScaleMax = shineMax;
		m_shine.m_ScaleOverTimeMin = shineGrowMin;
		m_shine.m_ScaleOverTimeMax = shineGrowMax;
		m_shine.m_ScalePower = -2;
	
		m_shine.m_Colour = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		m_shine.m_ColourOverTime = new Color(-1.0f, 0.0f, 0.0f, -1.0f);
		m_shine.m_ColourPower = 1;

		m_shine.Initialise();

				
				
		m_rings = gameObject.AddComponent<ParticleFXEmitter>();

		m_rings.m_MaxParticles = 2;
		
		m_rings.m_ParticlesPerSecond = 0.0f;
		m_rings.m_ParticleSpawnPeriod = 0.0f;
			
		m_rings.m_InstantSpawnParticles = 2;
	
		m_rings.m_FireAndForget = false;
		m_rings.m_TimeToForget = 0.0f;
		
		m_rings.m_Texture = (int)TextureAtlas.ParticleFX.GLOW_RING;
		
		m_rings.m_ParticleLifetimeMin = 0.75f;
		m_rings.m_ParticleLifetimeMax = 0.75f;
		
		m_rings.m_InitialRotationMin = 0.0f;
		m_rings.m_InitialRotationMax = 0.0f;
		m_rings.m_RotationOverTimeMin = 0.0f;
		m_rings.m_RotationOverTimeMax = 0.0f;
		m_rings.m_RotationPower = 0;
		
		m_rings.m_InitialScaleMin = 2.5f;
		m_rings.m_InitialScaleMax = 2.5f;
		m_rings.m_ScaleOverTimeMin = 3.0f;
		m_rings.m_ScaleOverTimeMax = 3.0f;
		m_rings.m_ScalePower = -1;

		m_rings.m_Colour = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		m_rings.m_ColourOverTime = new Color(-1.0f, 0.0f, 0.0f, -1.0f);
		
		m_rings.m_ColourPower = -1;

		m_rings.Initialise();
	}
	
	public void Play()
	{
		if( m_glow != null )
			m_glow.Play();

		if( m_shine != null )
			m_shine.Play();
		
		if( m_rings != null )
			m_rings.Play();
	}

	public void Stop()
	{
		if( m_glow != null )
			m_glow.Stop();

		if( m_shine )
			m_shine.Stop();
		
		if( m_rings )
			m_rings.Stop();
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
		if( m_glow != null )
			Destroy( m_glow );
		m_glow = null;
		
		if( m_shine != null )
			Destroy( m_shine );
		m_shine = null;
		
		if( m_rings != null )
			Destroy( m_rings );
		m_rings = null;
	}
}
