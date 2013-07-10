//-----------------------------------------------------------------------------
// File: ObstaclePiece.cs
//
// Desc:	A collection of obstacles representing a full piece for the
//			foreground manager to use to construct the course.
//
//			ObstaclePieces have a distance offset so that they can be scrolled
//			as the player moves through the course and should be a minimum of
//			20 metres wide so no more than 2 obstacle pieces are visible at 
//			any time.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclePiece
{
	public bool m_Active = false;
	
	public float m_Width = 20.0f;
	
	public float m_Distance = 0.0f;
	public float m_yOffset = 0.0f;
	
	List<Obstacle> m_obstacleList = new List<Obstacle>();

	
	public void AddObstacle( Obstacle new_obstacle )
	{
		new_obstacle.m_Parent = this;
		m_obstacleList.Add( new_obstacle );
	}
	
	public bool CollideWithBox( Vector3 box_center, float box_width, float box_height )
	{
		foreach( Obstacle obstacle in m_obstacleList )
		{
			if( obstacle.CollideWithBox( box_center, box_width, box_height ) )
				return true;
		}
		
		return false;
	}
	
	public bool PlatformCollide( Vector3 box_center, float box_height, Vector3 movement_vector, ref Vector3 collision_point )
	{
		foreach( Obstacle obstacle in m_obstacleList )
		{
			if( obstacle.PlatformCollide( box_center, box_height, movement_vector, ref collision_point ) )
				return true;
		}
		
		return false;
	}
	
	public void DebugRender()
	{
		foreach( Obstacle obstacle in m_obstacleList )
		{
			obstacle.DebugRender();
		}
	}
}
