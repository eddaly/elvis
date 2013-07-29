//-----------------------------------------------------------------------------
// File: Distance.cs
//
// Desc:	Shows the distance the player has currently run for
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Distance : MonoBehaviour 
{
	FontNumber m_fontNumber;
	
	void Start() 
	{
		m_fontNumber = gameObject.AddComponent<FontNumber>();
		
		m_fontNumber.m_Number = 0;
		m_fontNumber.m_DisplayLeadingDigits = false;
		m_fontNumber.m_NumDigits = 5;
		m_fontNumber.m_ScaleX = 100.0f;
		m_fontNumber.m_ScaleY = 100.0f;
		m_fontNumber.m_Spacing = 64.0f;
		m_fontNumber.m_Layer = gameObject.layer;
		m_fontNumber.m_Colour = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
		
		m_fontNumber.InitialiseDigits();
	}
	
	void Update() 
	{
		m_fontNumber.m_Number = (int)RL.m_Sequencer.m_MasterDistance;
	}
	
	void OnDestroy()
	{
		m_fontNumber.Release();
	}
}
