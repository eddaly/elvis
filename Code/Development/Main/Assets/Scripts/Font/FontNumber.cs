//-----------------------------------------------------------------------------
// File: FontNumber.cs
//
// Desc:	Bitmap based numbers-only font renderer
//
// Copyright Echo Peak Ltd 2012
// <Charles James>
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class FontNumber : MonoBehaviour 
{
	public bool m_EditorManaged = false;
	
	public int m_Number;
	int m_oldNumber;
	public int m_NumDigits;
		
	public bool m_DisplayLeadingDigits;
	
	public bool m_Percentage;
	BatchedQuadDef m_percentageQuad;
	public bool m_Multiplier;
	BatchedQuadDef m_multiplierQuad;
	public bool m_Colon;
	BatchedQuadDef m_colonQuad;
	
	public float m_ScaleX;
	float m_oldScaleX;
	public float m_ScaleY;
	float m_oldScaleY;
	
	public Vector3 m_Offset;
	
	public float m_Spacing;
	public int m_Anchor;
	
	public Color m_Colour = new Color( 1.0f, 1.0f, 1.0f, 0.0f );
	
	BatchedQuadDef[] m_digits;

	const float k_texWidth = 1664.0f;
	
	public bool m_ForcePaint = false;
	public int m_Layer = 8;

	
	void Start () 
	{
		if( m_EditorManaged )
			InitialiseDigits();
	}
	
	public void InitialiseDigits()
	{
		m_digits = new BatchedQuadDef[m_NumDigits];
		
		for( int d = 0; d<m_NumDigits; d++ )
		{
			m_digits[d] = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.NUMBERS_BATCH );
			
			if( m_digits[d] != null )
			{
				m_digits[d].m_Colour = m_Colour;
				m_digits[d].m_Scale = new Vector3( m_ScaleX, m_ScaleY, 0.0f );			
				m_digits[d].m_Active = true;
				m_digits[d].m_TextureIdx = 0;
			}
		}
		
		if( m_Percentage )
		{
			m_percentageQuad = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.NUMBERS_BATCH );
			if( m_percentageQuad != null )
			{	
				m_percentageQuad.m_Colour =  m_Colour;
				m_percentageQuad.m_Scale = new Vector3( m_ScaleX, m_ScaleY, 0.0f );
				m_percentageQuad.m_TextureIdx = (int)(TextureAtlas.Numbers.NUM_PERCENT);
				m_percentageQuad.m_Active = true;
			}
		}
		else
			m_percentageQuad = null;
		
		if( m_Multiplier )
		{
			m_multiplierQuad = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.NUMBERS_BATCH );
			if( m_multiplierQuad != null )
			{
				m_multiplierQuad.m_Colour =  m_Colour;
				m_multiplierQuad.m_Scale = new Vector3( m_ScaleX, m_ScaleY, 0.0f );
				m_multiplierQuad.m_TextureIdx = (int)(TextureAtlas.Numbers.NUM_TIMES);
				m_multiplierQuad.m_Active = true;
			}
		}
		else
			m_multiplierQuad = null;
		
		if( m_Colon )
		{
			m_colonQuad = PrimitiveLibrary.Get.GetQuadDefinition( PrimitiveLibrary.QuadBatch.NUMBERS_BATCH );
			if( m_colonQuad != null )
			{	
				m_colonQuad.m_Colour =  m_Colour;
				m_colonQuad.m_Scale = new Vector3( m_ScaleX, m_ScaleY, 0.0f );
				m_colonQuad.m_TextureIdx = (int)(TextureAtlas.Numbers.NUM_COLON);
				m_colonQuad.m_Active = true;
			}
		}
		else
			m_colonQuad = null;
		
		m_oldScaleX = m_ScaleX;
		m_oldScaleY = m_ScaleY;
		
		//	Make sure it gets at least 1 update
		m_oldNumber = -999999;
	}
	
	void Update () 
	{
		if( m_oldNumber != m_Number || m_ForcePaint )
		{
			m_ForcePaint = false;
			
			int[] currentDigits = new int[m_NumDigits];
			
			int number = m_Number;
			for( int d = m_NumDigits - 1; d>=0; d-- )
			{
				int newNumber = number/10;
				
				currentDigits[d] = number - newNumber*10;
				number = newNumber;
			}
			
			int numDigitsToDisplay = m_NumDigits;
			
			if( !m_DisplayLeadingDigits )
			{
				int firstSignificantDigit = 0;
				
				while( firstSignificantDigit < m_NumDigits && currentDigits[firstSignificantDigit] == 0 )
					firstSignificantDigit++;
				
				numDigitsToDisplay = m_NumDigits - firstSignificantDigit;
				if( numDigitsToDisplay < 1 )
					numDigitsToDisplay = 1;
			}
			
			int numGliphs = numDigitsToDisplay;
			if( m_percentageQuad != null || m_multiplierQuad != null || m_colonQuad != null )
				numGliphs++;
			
			float baseX = 0.0f;
			
			//	Anchor == 1 means number is centered
			if( m_Anchor == 1 )
				baseX = ((float)numGliphs*m_Spacing)*-0.5f;
			
			//	Anchor == 2 means number is right aligned
			if( m_Anchor == 2 )
				baseX = -(float)numGliphs*m_Spacing;
			
			if( m_multiplierQuad != null )
			{
				//	Multiplier symbol comes first
				Vector3 pos = new Vector3(baseX + m_ScaleX*0.5f, 0.0f, 0.0f );
				m_multiplierQuad.m_Position = pos + m_Offset + transform.position;
				
				baseX += m_Spacing;
			}
			
			//	Make all digits invisible
			for( int d = 0; d<m_NumDigits; d++ )
			{			
				if( m_digits[d] != null )
					m_digits[d].m_Active = false;
			}
	
			//	Now display the digits we want
			for( int d = m_NumDigits - numDigitsToDisplay; d<m_NumDigits; d++ )
			{
				if( m_digits[d] != null )
				{
					Vector3 digitPos = new Vector3(baseX + m_ScaleX*0.5f, 0.0f, 0.0f );
					m_digits[d].m_Position = digitPos + m_Offset + transform.position;
					
					baseX += m_Spacing;

					m_digits[d].m_Colour = m_Colour;
					m_digits[d].m_TextureIdx = currentDigits[d];
					m_digits[d].m_Active = true;
				}
			}

			if( m_percentageQuad != null )
			{
				//	Percentage symbol comes last
				Vector3 pos = new Vector3(baseX + m_ScaleX*0.5f, 0.0f, 0.0f );
				m_percentageQuad.m_Position = pos + m_Offset + transform.position;
			}			

			if( m_colonQuad != null )
			{
				//	Colon symbol comes last
				Vector3 pos = new Vector3(baseX + m_ScaleX*0.5f, 0.0f, 0.0f );
				m_colonQuad.m_Position = pos + m_Offset + transform.position;
			}			
		}
		
		//	Check to see if we have to rescale
		if( m_oldScaleX != m_ScaleX || m_oldScaleY != m_ScaleY )
		{
			Vector3 scale = new Vector3( m_ScaleX, m_ScaleY, 0.0f );
			
			for( int d = 0; d<m_NumDigits; d++ )
			{
				if( m_digits[d] != null )
					m_digits[d].m_Scale = scale;
			}

			if( m_multiplierQuad != null )
				m_multiplierQuad.m_Scale = scale;

			if( m_percentageQuad != null )
				m_percentageQuad.m_Scale = scale;

			if( m_colonQuad != null )
				m_colonQuad.m_Scale = scale;
		}
		
		m_oldScaleX = m_ScaleX;
		m_oldScaleY = m_ScaleY;

		m_oldNumber = m_Number;
	}
	
	public void Hide()
	{
		for( int d = 0; d<m_NumDigits; d++ )
		{
			if( m_digits != null )
			{
				if( m_digits[d] != null )
					m_digits[d].m_Active = false;
			}
		}
		
		if( m_percentageQuad != null )
			m_percentageQuad.m_Active = false;
		
		if( m_multiplierQuad != null )
			m_multiplierQuad.m_Active = false;

		if( m_colonQuad != null )
			m_colonQuad.m_Active = false;
	}
	
	public void Show()
	{
		for( int d = 0; d<m_NumDigits; d++ )
		{
			if( m_digits != null )
			{
				if( m_digits[d] != null )
					m_digits[d].m_Active = true;
			}
		}
		
		if( m_percentageQuad != null )
			m_percentageQuad.m_Active = true;
		
		if( m_multiplierQuad != null )
			m_multiplierQuad.m_Active = true;

		if( m_colonQuad != null )
			m_colonQuad.m_Active = true;
		
		m_ForcePaint = true;
	}
	
	public void Release()
	{
		for( int d = 0; d<m_NumDigits; d++ )
			PrimitiveLibrary.Get.ReleaseQuadDefinition( m_digits[d] );
		
		if( m_percentageQuad != null )
			PrimitiveLibrary.Get.ReleaseQuadDefinition( m_percentageQuad );
		
		if( m_multiplierQuad != null )
			PrimitiveLibrary.Get.ReleaseQuadDefinition( m_multiplierQuad );

		if( m_colonQuad != null )
			PrimitiveLibrary.Get.ReleaseQuadDefinition( m_colonQuad );
	}
	
	void OnDestroy()
	{
		if( m_EditorManaged )
			Release();
	}
}
