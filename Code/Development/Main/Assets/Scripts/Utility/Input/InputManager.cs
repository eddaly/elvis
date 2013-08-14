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

	bool m_downDetected = false;
	int m_detectDelay = 0;
	int m_downDetectDelay = 2;
	Vector2 m_swipeStart;
	
	public bool[] m_touchInput = new bool[2];
	public bool[] m_oldTouchInput = new bool[2];
	public bool[] m_touchPressed = new bool[2];

	
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
		
		m_touchInput[0] = false;
		m_touchInput[1] = false;
		
		m_oldTouchInput[0] = false;
		m_oldTouchInput[1] = false;
		
		m_touchPressed[0] = false;
		m_touchPressed[1] = false;
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
	
	
	public void UpdateTouch()
	{
		bool forcePress = false;
		
		for( int t = 0; t<2; t++ )
			m_touchPressed[t] = false;
		
        if (Input.touchCount > 0) 
        {
            Touch touch = Input.touches[0];
        
            switch (touch.phase) 
            {
            case TouchPhase.Began:
				m_downDetected = false;
			
                m_swipeStart = touch.position;
				
				m_touchInput[0] = false;
				m_touchInput[1] = false;
				
				m_detectDelay = m_downDetectDelay;
				m_downDetectDelay = 2;
                break;
            
            case TouchPhase.Moved:
				if( touch.position.y - m_swipeStart.y < -5.0f )
				{
					m_downDetected = true;
					m_touchInput[1] = true;
				}
				else if( !m_downDetected && m_detectDelay-- < 0 )
				{
					m_touchInput[0] = true;
				}
                break;

			case TouchPhase.Stationary:
				if( !m_downDetected && m_detectDelay-- < 0 )
				{
					m_touchInput[0] = true;
				}
                break;
				
            case TouchPhase.Ended:
				m_touchInput[0] = false;
				m_touchInput[1] = false;

				if( !m_downDetected )
					forcePress = true;
				
				m_downDetected = false;
                break;
            }
			
			for( int t = 0; t<2; t++ )
			{
				if( !m_oldTouchInput[t] && m_touchInput[t] )
					m_touchPressed[t] = true;
				else
					m_touchPressed[t] = false;
								
				m_oldTouchInput[t] = m_touchInput[t];
			}			
        }
		
		if( forcePress )
			m_touchPressed[0] = true;
		
		if( RL.m_Player.m_highAnalogueTimer > 0.0f )
			m_touchPressed[1] = false;
	}
	
	public bool GetPressed( int idx )
	{
		if( Application.isEditor )
			return m_FirePressed[idx];
		else
			return m_touchPressed[idx];
	}
	
	public bool GetDown( int idx )
	{
		if( Application.isEditor )
			return m_FireDown[idx];
		else
			return m_touchInput[idx];
	}
}
