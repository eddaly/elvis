//-----------------------------------------------------------------------------
// File: ParticleEmitter.cs
//
// Desc:	Controls a bunch of particles of the same type
//
// Note:	Users should set the public parameters of the emitter themselves 
//			directly through their reference before calling Initialise() to
//			set up.
//
//			Users must then call Play() to begin emitting.
//
//			If m_FireAndForget is true, the emitter will destroy itself once
//			all the particles have reached the end of their lifetime. Otherwise
//			the user will need to call destroy the game object themsevles.
//
//			It's shit. I'll build it up as I go along. I'm belatedly staying
//			away from Shuriken FX.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ParticleDefinition
{
	public bool m_Active;
	
	public float m_StartTime;
	public float m_EndTime;
	
	public float m_IntialRotation;
	public float m_RotationOverTime;
	
	public float m_InitialScale;
	public float m_ScaleOverTime;
}

public class ParticleFXEmitter : MonoBehaviour 
{
	public int m_MaxParticles;
	
	public float m_ParticlesPerSecond;
	public float m_ParticleSpawnPeriod;
	float m_particlesToSpawn;
	
	public int m_InstantSpawnParticles;
	
	BatchedQuadDef[] m_particleQuads;
	ParticleDefinition[] m_particleDefs;
	int m_particleDefSearchCursor;

	public bool m_FireAndForget;
	public float m_TimeToForget;
	bool m_running;
	
	public bool m_Additive = true;
	public int m_Texture;
	
	public float m_ParticleLifetimeMin;
	public float m_ParticleLifetimeMax;
	
	public float m_emitterStartTime;
	public float m_emitterEndTime;
	
	public float m_InitialRotationMin;
	public float m_InitialRotationMax;
	public float m_RotationOverTimeMin;
	public float m_RotationOverTimeMax;
	public int m_RotationPower;
	
	public float m_InitialScaleMin;
	public float m_InitialScaleMax;
	public float m_ScaleOverTimeMin;
	public float m_ScaleOverTimeMax;
	public int m_ScalePower;

	public Color m_Colour;
	public Color m_ColourOverTime;
	public int m_ColourPower;

	
	void Start()
	{
	}
	
	public void Initialise()
	{
		m_particleQuads = new BatchedQuadDef[m_MaxParticles];
		m_particleDefs = new ParticleDefinition[m_MaxParticles];
			
		for( int p = 0; p<m_MaxParticles; p++ )
		{
			m_particleQuads[p] = null;
			m_particleDefs[p] = new ParticleDefinition();
			m_particleDefs[p].m_Active = false;
		}

		m_particlesToSpawn = 0;
		m_particleDefSearchCursor = 0;
		m_running = false;
	}

	public void Play()
	{
		m_emitterStartTime = GlobalData.Get.m_GlobalTime;
		
		if( m_FireAndForget )
			m_emitterEndTime = m_emitterStartTime + m_TimeToForget + m_ParticleLifetimeMax;
		else
			m_emitterEndTime = 999999.0f;
		
		for( int p = 0; p<m_InstantSpawnParticles; p++ )
		{
			int idx = getParticleDefinition();

			if( idx >= 0 && !m_particleDefs[idx].m_Active )
				emitParticle( idx, m_emitterStartTime );				
		}
				
		m_running = true;
	}
	
	public void Stop()
	{
		for( int p = 0; p<m_MaxParticles; p++ )
		{
			ParticleDefinition def = m_particleDefs[p];
			
			if( def.m_Active )
			{
				def.m_Active = false;
				PrimitiveLibrary.Get.ReleaseQuadDefinition( m_particleQuads[p] );
			}
		}
		
		m_particlesToSpawn = 0;
		m_running = false;
	}
	
	void Update()
	{
		if( !m_running )
			return;

		//	I'm making emitters able to instantly fire of a bunch of particles, and also
		//	for them to fire off particles over time.
		//
		//	Also, non fireandforget emitters can instantly fire off particles and then
		//	remain active, waiting for another Play command to fire off more.
		
		float time = GlobalData.Get.m_GlobalTime;
		float dTime = GlobalData.Get.m_GlobalDTime;
		
		//	All emitters stay alive now
//		if( time < m_emitterStartTime || time > m_emitterEndTime )
//			destroyMyself();

		//	If this emitter emits particles over time
		if( m_ParticlesPerSecond > 0.0f )
		{
			//	If this is fire and forget, stop emitting in enough time to let
			//	let the currently active ones fade out before we destroy ourself
			if( !m_FireAndForget || time < m_emitterStartTime + m_TimeToForget )
			{
				//	If we should only spawn particles for a finite period of time
				if( m_ParticleSpawnPeriod > 0.0f )
				{
					if( time < m_emitterStartTime + m_ParticleSpawnPeriod )
						m_particlesToSpawn += dTime*m_ParticlesPerSecond;
				}
				else
				{
					m_particlesToSpawn += dTime*m_ParticlesPerSecond;
				}
			}
			
			//	Now spawn the particles that are ready to go
			while( m_particlesToSpawn > 1.0f )
			{
				int idx = getParticleDefinition();
	
				if( idx >= 0 && !m_particleDefs[idx].m_Active )
					emitParticle( idx, time );				
				
				m_particlesToSpawn -= 1.0f;
			}
		}
				
		for( int p = 0; p<m_MaxParticles; p++ )
		{
			ParticleDefinition def = m_particleDefs[p];
			
			if( def.m_Active )
			{
				SetParticleQuad( def, m_particleQuads[p], time );				
			}
		}
	}

	int getParticleDefinition()
	{
		if( !m_particleDefs[m_particleDefSearchCursor].m_Active )
			return m_particleDefSearchCursor;

		//	Need to search on
		int startPoint = m_particleDefSearchCursor++;
		
		if( m_particleDefSearchCursor >= m_MaxParticles )
			m_particleDefSearchCursor = 0;
		
		while( startPoint != m_particleDefSearchCursor )
		{
			if( !m_particleDefs[m_particleDefSearchCursor].m_Active )
				return m_particleDefSearchCursor;
			
			if( ++m_particleDefSearchCursor >= m_MaxParticles )
				m_particleDefSearchCursor = 0;
		}
		
		return -1;
	}
	
	void emitParticle( int index, float time )
	{	
		//	Try to get a quad from the particleFX batch
		if( m_Additive )
			m_particleQuads[index] = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.FX_BATCH );
		else
			m_particleQuads[index] = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.FX_ALPHA_BATCH );

		//	If we didn't get one, then don't emit
		if( m_particleQuads[index] == null )
			return;
		
		m_particleQuads[index].m_Position = transform.position;
		m_particleQuads[index].m_TextureIdx = m_Texture;
		
		ParticleDefinition def = m_particleDefs[index];
		def.m_Active = true;
		
		def.m_StartTime = time;
		def.m_EndTime = time + Random.Range( m_ParticleLifetimeMin, m_ParticleLifetimeMax );
	
		def.m_IntialRotation = Random.Range( m_InitialRotationMin, m_InitialRotationMax );
		def.m_RotationOverTime = Random.Range( m_RotationOverTimeMin, m_RotationOverTimeMax );
	
		def.m_InitialScale = Random.Range( m_InitialScaleMin, m_InitialScaleMax );
		def.m_ScaleOverTime = Random.Range( m_ScaleOverTimeMin, m_ScaleOverTimeMax );
	}
	
	void SetParticleQuad( ParticleDefinition def, BatchedQuadDef quad, float time )
	{
		float normalTime = time - def.m_StartTime;
		normalTime /= (def.m_EndTime - def.m_StartTime);
		
		if( normalTime < 0.0f )
		{
			def.m_Active = false;
			PrimitiveLibrary.Get.ReleaseQuadDefinition( quad );
			return;
		}
		
		bool releaseParticle = false;
		if( normalTime > 1.0f )
		{
			normalTime = 1.0f;
			releaseParticle = true;
		}
		
		float rotationTime = normalTime;
		if( m_RotationPower < 0 )
		{
			rotationTime = 1.0f - rotationTime;
			for( int p = 0; p<-m_RotationPower; p++ )
				rotationTime *= rotationTime;
			rotationTime = 1.0f - rotationTime;
		}
		else if( m_RotationPower > 0 )
		{
			for( int p = 0; p<m_RotationPower; p++ )
				rotationTime *= rotationTime;
		}		
		float rotation = def.m_IntialRotation + rotationTime*def.m_RotationOverTime;
		
		float scaleTime = normalTime;
		if( m_ScalePower < 0 )
		{
			scaleTime = 1.0f - scaleTime;
			for( int p = 0; p<-m_ScalePower; p++ )
				scaleTime *= scaleTime;
			scaleTime = 1.0f - scaleTime;
		}
		else if( m_ScalePower > 0 )
		{
			for( int p = 0; p<m_ScalePower; p++ )
				scaleTime *= scaleTime;
		}				
		float scale = def.m_InitialScale + scaleTime*def.m_ScaleOverTime;
		
		float colourTime = normalTime;
		if( m_ColourPower < 0 )
		{
			colourTime = 1.0f - colourTime;
			for( int p = 0; p<-m_ColourPower; p++ )
				colourTime *= colourTime;
			colourTime = 1.0f - colourTime;
		}
		else if( m_ColourPower > 0 )
		{
			for( int p = 0; p<m_ColourPower; p++ )
				colourTime *= colourTime;
		}				
		Color col = m_Colour + colourTime*m_ColourOverTime;
		
		quad.m_Position = transform.position;
		quad.m_Rotation = rotation;
		quad.m_Scale = new Vector2( scale, scale );
		quad.m_Colour = col;
		
		if( releaseParticle )
		{
			def.m_Active = false;
			PrimitiveLibrary.Get.ReleaseQuadDefinition( quad );
			return;
		}
	}
	
	void OnDestroy()
	{
		releaseQuads();
	}
	
	void releaseQuads()
	{
		for( int p = 0; p<m_MaxParticles; p++ )
		{
			if( m_particleDefs[p].m_Active )
				PrimitiveLibrary.Get.ReleaseQuadDefinition( m_particleQuads[p] );
		}
	}
	
	void destroyMyself()
	{
		Destroy( this );
	}
}
