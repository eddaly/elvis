//-----------------------------------------------------------------------------
// File: BackgroundPiece.cs
//
// Desc:	Script applied to the parent object of any of the different
//			types of background pieces.
//
//			Provides information about the dimensions of the piece and a place
//			for the game management objects to manipulate the piece itself.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EnvironmentPiece : MonoBehaviour 
{
	public float m_Width = 20.0f;
	
	public EntityDefs.EnvLayer m_Type;
	
	void Start() 
	{
	}
	
	void Update() 
	{	
	}
}
