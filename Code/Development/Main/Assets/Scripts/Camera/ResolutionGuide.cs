//-----------------------------------------------------------------------------
// File: ResolutionGuide.cs
//
// Desc:	Helper class for editing in-game scenes.
//
//			Renders debug lines to screen as a guide to how the different
//			device resolutions will render. Should be attached to the main
//			camera.
//
//			Assumes that the most square aspect ratio is 4:3
//
//			Active Area:
//				Top 20% lines out of bounds
//				Bottom 10% lines out of bounds
//
//			Camera position
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ResolutionGuide : MonoBehaviour 
{
	public int m_ResolutionX = 1024;
	public int m_ResolutionY = 768;
	
	void Start() 
	{
//		m_ratio = 
	}
	
	void Update() 
	{	
	}
}
