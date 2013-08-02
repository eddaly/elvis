//-----------------------------------------------------------------------------
// File: Collisions.cs
//
// Desc:	Shows the number of times the player has fatally collided with
//			something
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour 
{
	FontNumber m_fontNumber;
	
	int m_collisions;
	bool m_oldColliding;
	
	void Start() 
	{
		m_fontNumber = gameObject.AddComponent<FontNumber>();
		
		m_fontNumber.m_Number = 0;
		m_fontNumber.m_DisplayLeadingDigits = false;
		m_fontNumber.m_NumDigits = 4;
		m_fontNumber.m_ScaleX = 100.0f;
		m_fontNumber.m_ScaleY = 100.0f;
		m_fontNumber.m_Spacing = 64.0f;
		m_fontNumber.m_Layer = gameObject.layer;
		m_fontNumber.m_Colour = new Color( 1.0f, 0.0f, 0.0f, 1.0f );
		
		m_fontNumber.InitialiseDigits();
		
		
		m_collisions = 0;
		m_oldColliding = false;
	}
	
	void Update() 
	{		
		bool newColliding = RL.m_Player.m_Colliding;
		
		if( newColliding && !m_oldColliding )
		{
			m_collisions++;
			m_fontNumber.m_Number = m_collisions;
		}
			
		m_oldColliding = newColliding;
	}
	
	void OnDestroy()
	{
		m_fontNumber.Release();
	}
}
