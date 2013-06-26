//-----------------------------------------------------------------------------
// File: SkyBoxManager.cs
//
// Desc:	Helper class for Elvis skybox objects.
//
//			In edit mode the manager forces the positions of all skyboxes 
//			below it (skyboxes must be named SkyBox1, SkyBox2, etc) to fit with
//			the Elvis layering system. The user can then move between them
//			with the inspector variables.
//
//			In play mode the skyboxes are put back at y = 0, and the current
//			one is selected by the BackgroundTrackMaker.
//
//			Place all objects for the skyboxes below their holding GameObject
//			(SkyBox1, SkyBox2, etc).
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour 
{
	void Start () 
	{
	}
	
	void Update () 
	{	
	}
}
