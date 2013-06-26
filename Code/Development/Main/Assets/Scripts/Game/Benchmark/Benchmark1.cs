//-----------------------------------------------------------------------------
// File: Benchmark1.cs
//
// Desc:	Example scene script to prove, test, and optimise the framework.
//
//			Just a silly force propagation field
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Benchmark1 : MonoBehaviour 
{
	const int kNumPulsersX = 30;
	const int kNumPulsersY = 30;
	
	BatchedQuadDef m_cursor;
	BatchedQuadDef[,] m_pulsers = new BatchedQuadDef[kNumPulsersX, kNumPulsersY];
	
	float[,] m_translationField = new float[kNumPulsersX, kNumPulsersY];
	float[,] m_velocityField = new float[kNumPulsersX, kNumPulsersY];
	float[,] m_accelerationField = new float[kNumPulsersX, kNumPulsersY];
	
	Color[] m_colourWheel = new Color[3000];
	
	void Start () 
	{
		m_cursor = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.FX_BATCH );
		
		m_cursor.m_TextureIdx = (int)(TextureAtlas.ParticleFX.SHINE);
		
		m_cursor.m_Scale.x = 2.0f;
		m_cursor.m_Scale.y = 2.0f;

		m_cursor.m_Colour = new Color( 0.2f, 0.9f, 1.0f, 1.0f );
		
		
		for( int x = 0; x<kNumPulsersX; x++ )
		{
			for( int y = 0; y<kNumPulsersY; y++ )
			{
				m_pulsers[x, y] = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.FX_BATCH );

				m_pulsers[x, y].m_TextureIdx = (int)(TextureAtlas.ParticleFX.GLOW);
				
				m_pulsers[x, y].m_Position.x = (float)x;
				m_pulsers[x, y].m_Position.y = (float)y;
				m_pulsers[x, y].m_Position.z = 0.0f;
				
				m_pulsers[x, y].m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
				
				m_translationField[x, y] = 0.0f;
				m_velocityField[x, y] = 0.0f;
				m_accelerationField[x, y] = 0.0f;				
			}
		}
		
		
		float colourAngle;
		float r, g, b;
		for( int c = 0; c<3000; c++ )
		{
			colourAngle = ((float)c)*0.01f;
			r = Mathf.Abs( Mathf.Sin( colourAngle ) ); 			
			g = Mathf.Abs( Mathf.Sin( colourAngle + Mathf.PI*0.666f ) ); 			
			b = Mathf.Abs( Mathf.Sin( colourAngle + Mathf.PI*1.333f) );			
			
			m_colourWheel[c] = new Color( r, g, b, 1.0f );
		}
	}
	
	void Update() 
	{
		InputManager.Get.Update();
		
		Vector3 cursorPos = InputManager.Get.m_MouseWorldPos;
		m_cursor.m_Position = cursorPos;
		
		
		calculateForces();
		applyMouse();
		integrateForces( 1/60.0f );
		
		setQuads();		
	}
	
	void OnDestroy()
	{
		PrimitiveLibrary.Get.ReleaseQuadDefinition( m_cursor );
		m_cursor = null;
		
		for( int x = 0; x<kNumPulsersX; x++ )
		{
			for( int y = 0; y<kNumPulsersY; y++ )
			{
				PrimitiveLibrary.Get.ReleaseQuadDefinition( m_pulsers[x, y] );
				m_pulsers[x, y] = null;
			}
		}
	}
	
	void calculateForces()
	{
		float forceInput = 0.0f;
		float baseHeight = 0.0f;
		int neighbourX = 0;
		int neighbourY = 0;

		//	Use a simple 3x3 tap grid
		const float tensionFactor = 25.0f;
		const float springFactor = 5.0f;
		const float orthogonalForce = 0.15f;
		const float diagonalForce = 0.1f;
		
		for( int x = 0; x<kNumPulsersX; x++ )
		{
			for( int y = 0; y<kNumPulsersY; y++ )
			{
				forceInput = 0.0f;
				baseHeight = m_translationField[x, y];
				
				//	Bottom left
				neighbourX = x - 1;
				if( neighbourX < 0 )	neighbourX = x + 1;
				neighbourY = y - 1;
				if( neighbourY < 0 )	neighbourY = y + 1;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*diagonalForce;

				//	Left
				neighbourY = y;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*orthogonalForce;
				
				//	Top left
				neighbourY = y + 1;
				if( neighbourY == kNumPulsersY )	neighbourY = y - 1;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*diagonalForce;
				
				//	Top
				neighbourX = x;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*orthogonalForce;

				//	Top right
				neighbourX = x + 1;
				if( neighbourX == kNumPulsersX )	neighbourX = x - 1;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*diagonalForce;

				//	Right
				neighbourY = y;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*orthogonalForce;

				//	Bottom right
				neighbourY = y - 1;
				if( neighbourY < 0 )	neighbourY = y + 1;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*diagonalForce;

				//	Bottom
				neighbourX = x;
				forceInput += ( m_translationField[neighbourX, neighbourY] - baseHeight)*orthogonalForce;

				
				m_accelerationField[x, y] = forceInput*tensionFactor - 
					springFactor*baseHeight;
			}
		}
	}
	
	void integrateForces( float d_time )
	{
		for( int x = 0; x<kNumPulsersX; x++ )
		{
			for( int y = 0; y<kNumPulsersY; y++ )
			{
				m_velocityField[x, y] += m_accelerationField[x, y]*d_time;
				m_translationField[x, y] += m_velocityField[x, y]*d_time;
				
				//	Damping
				m_translationField[x, y] *= 0.995f;
//				m_velocityField[x, y] *= 0.99f;				
//				m_accelerationField[x, y] *= 0.99f;
			}
		}
	}
	
	void setQuads()
	{
		float baseHeight;
		float baseMulti;
		int colourIndex;
		
		for( int x = 0; x<kNumPulsersX; x++ )
		{
			for( int y = 0; y<kNumPulsersY; y++ )
			{
				baseHeight = m_translationField[x, y];

				colourIndex = (int)(baseHeight*750.0f + 750.0f);
				if( colourIndex < 0 )	colourIndex = 0;
				if( colourIndex > 2999 )	colourIndex = 2999;
				m_pulsers[x, y].m_Colour = m_colourWheel[colourIndex];
				
				if( colourIndex < 0 )
					Debug.Log( colourIndex );
				if( colourIndex > 3000 )
					Debug.Log( colourIndex );
				
				if( baseHeight > 0.1f )
					baseHeight = 0.1f;
				
//				baseHeight = 1.0f - baseHeight;
//				baseHeight *= baseHeight;
//				baseHeight = 1.0f - baseHeight;
				
				baseMulti = 1.0f - baseHeight/20.0f;
				baseHeight *= 1.0f + baseMulti*5.0f;
				
//				baseHeight *= 10.0f;
//				if( baseHeight > 1.0f )
//					baseHeight = 1.0f;
				
				m_pulsers[x, y].m_Scale.x = 0.0f + baseHeight*5.0f;
				
				m_pulsers[x, y].m_Rotation = baseHeight*6.0f;
			}
		}
	}
	
	void applyMouse()
	{
		if( !InputManager.Get.m_FireDown[0] )
			return;
		
		int mouseX = (int)(m_cursor.m_Position.x);
		int mouseY = (int)(m_cursor.m_Position.y);
		
		if( mouseX < 0 )				mouseX = 0;
		if( mouseX > kNumPulsersX - 1 )	mouseX = kNumPulsersX - 1;
		if( mouseY < 0 )				mouseY = 0;
		if( mouseY > kNumPulsersY - 1 )	mouseY = kNumPulsersY - 1;
		
		if( m_translationField[mouseX, mouseY] < 0.9f )
			m_translationField[mouseX, mouseY] += 0.1f;
	}
}
