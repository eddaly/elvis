//-----------------------------------------------------------------------------
// File: EntryScreen.cs
//
// Desc: 	Transitioning screen for the in-game to hide level starts and add
//			a click-through event to replays
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class EntryScreen : MonoBehaviour 
{
	GameObject m_backPlane;
	
	bool m_transitioningIn = false;
	
	float m_transitionValue = 1.0f;
	public float m_TransitionTarget = 1.0f;
	public float m_TransitionSpeed = 4.0f;
	
	void Start() 
	{
		m_backPlane = transform.FindChild( "Plane" ).gameObject;	
	}
	
	void Update() 
	{
		if( m_backPlane == null )
			return;

		if( m_transitionValue < m_TransitionTarget )
		{
			m_transitioningIn = true;
			
			m_transitionValue += Time.deltaTime*m_TransitionSpeed;
			if( m_transitionValue > m_TransitionTarget )
				m_transitionValue = m_TransitionTarget;
		}
		
		if( m_transitionValue > m_TransitionTarget )
		{
			m_transitioningIn = false;
			
			m_transitionValue -= Time.deltaTime*m_TransitionSpeed;
			if( m_transitionValue < m_TransitionTarget )
				m_transitionValue = m_TransitionTarget;
		}
		
		if( m_transitioningIn )
			setTransitionOut();
		else
			setTransitionIn();
			
	}
	
	void setTransitionOut()
	{
		Vector3 newPos = m_backPlane.transform.localPosition;
		newPos.x = -1524.0f + m_transitionValue*m_transitionValue*m_transitionValue*2548.0f;
		
		m_backPlane.transform.localPosition = newPos;
	}
	
	void setTransitionIn()
	{
		Vector3 newPos = m_backPlane.transform.localPosition;
		newPos.x = -1524.0f + m_transitionValue*m_transitionValue*m_transitionValue*2548.0f;
		
		m_backPlane.transform.localPosition = newPos;
	}
	
	void cycleAnimation()
	{
	}
}
