//-----------------------------------------------------------------------------
// File: PrototypeConfiguration.cs
//
// Desc:	Central control for altering various aspects of the game, used to
//			try different gameplay mechanics and level designs while we
//			continue the prototype process.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class PrototypeConfiguration : MonoBehaviour 
{
	public enum GameTypes
	{
		LARGE_FULL_COLLISION,
		SMALL_FRONT_COLLISION,
		JOYRIDE_STYLE,
		USER_CUSTOM
	}
	public GameTypes m_Prototype = GameTypes.LARGE_FULL_COLLISION;
	
	
	public enum TrackTypes
	{
		ANALOGUE_JUMPS1,
		EASY_JUMPS1,
		FULL_SCREEN_OBSTACLES,
		MULTI_TRACK
	}
	public TrackTypes m_TrackType = TrackTypes.ANALOGUE_JUMPS1;
	
	
	public enum CollisionTypes
	{
		FULL_COLLISION,
		FRONT_COLLISION,
		PLACE_COLLISION
	}
	public CollisionTypes m_CollisionType = CollisionTypes.FULL_COLLISION;
	
	
	public enum JumpTypes
	{
		DIGITAL,
		FAST_DIGITAL,
		ANALOGUE,
		HIGH_ANALOGUE
	}
	public JumpTypes m_JumpType = JumpTypes.DIGITAL;
	
	
	public float m_viewDistance = 1.0f;
	
	void Start () 
	{
		forceParameters();
	}
	
	void Update () 
	{
		forceParameters();
	}
	
	void forceParameters()
	{
		switch( m_Prototype )
		{
		case GameTypes.LARGE_FULL_COLLISION:
			m_TrackType = TrackTypes.ANALOGUE_JUMPS1;
			m_CollisionType = CollisionTypes.FULL_COLLISION;
			m_JumpType = JumpTypes.DIGITAL;
			m_viewDistance = 1.0f;
			break;
		case GameTypes.SMALL_FRONT_COLLISION:
			m_TrackType = TrackTypes.EASY_JUMPS1;
			m_CollisionType = CollisionTypes.FRONT_COLLISION;
			m_JumpType = JumpTypes.FAST_DIGITAL;
			m_viewDistance = 2.0f;
			break;
		case GameTypes.JOYRIDE_STYLE:
			break;
		case GameTypes.USER_CUSTOM:
			break;
		}
	}
}
