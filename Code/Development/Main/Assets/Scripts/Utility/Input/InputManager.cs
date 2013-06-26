//-----------------------------------------------------------------------------
// File: InputManager.cs
//
// Desc:	Abstraction layer for recieving input
//
// Note:	Abstract singleton class - not attached to a GameObject
//			Needs to find a camera object called "MainCamera"
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public sealed class InputManager
{	
	private static InputManager instance = new InputManager();
	public static InputManager Get
	{
		get { return instance; }
	}
	public void Poke() {}

	
	public Vector3 m_MousePos;
	public Vector3 m_MouseWorldPos;
	
	public bool[] m_FireDown = new bool[2];
	public bool[] m_FirePressed = new bool[2];
	public bool[] m_FireReleased = new bool[2];

	bool[] m_oldFireDown = new bool[2];
	
	Camera m_mainCamera;

	public InputManager()
	{
		initialise();
	}
	
	void initialise()
	{
		GameObject cameraObject = GameObject.Find( "MainCamera" );
		if( cameraObject != null )
			m_mainCamera = cameraObject.camera;
		else
			m_mainCamera = null;
	}
	
	public void Update()
	{	
		m_MousePos = Input.mousePosition;
		m_MousePos.z = 0.0f;

		if( m_mainCamera != null )
			m_MouseWorldPos = m_mainCamera.ScreenToWorldPoint( m_MousePos );//*32.0f;
		else
			m_MouseWorldPos = m_MousePos;
		
		for( int f = 0; f<2; f++ )
		{
			m_FireDown[f] = Input.GetMouseButton( f );
			
			if( !m_oldFireDown[f] && m_FireDown[f] )
			{
				m_FirePressed[f] = true;
			}
			else
				m_FirePressed[f] = false;
			
			if( m_oldFireDown[f] && !m_FireDown[f] )
				m_FireReleased[f] = true;
			else
				m_FireReleased[f] = false;
			
			m_oldFireDown[f] = m_FireDown[f];
		}
	}
}
