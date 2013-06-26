//-----------------------------------------------------------------------------
// File: GlobalData.cs
//
// Desc:	Singleton class for holding global, per-session data
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public sealed class GlobalData
{
	//	A static instance is only instantiated when first accessed at runtime
	static readonly GlobalData instance = new GlobalData();
	public static GlobalData Get
	{
		get { return instance; }
	}
	public void Poke() {}

	public bool m_PCInput = true;
	public bool m_DisableOverrides = false;
	
	
	public float m_GlobalTime = 0.0f;
	public float m_GlobalDTime = 0.0f;
	
	
	//-----------------------------------------------------------------------------
	// Constructor
	//
	// Desc:	Just reset all the variables - we shouldn't use dynamic data
	GlobalData()
	{
		ResetData();
	}
	
	//-----------------------------------------------------------------------------
	// Method:	ResetData()
	// Desc:	Clears everything back to default values
	public void ResetData()
	{
		ClearScores();
	}
	
	//-----------------------------------------------------------------------------
	// Method:	ClearScores()
	// Desc:	Only clears the scores - leaves other data intact
	public void ClearScores()
	{
	}	
}
