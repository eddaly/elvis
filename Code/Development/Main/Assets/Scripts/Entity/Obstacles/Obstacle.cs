//-----------------------------------------------------------------------------
// File: Obstacle.cs
//
// Desc:	An individual obstacle. Essentially a collider and a renderer.
//
//			For derived classes, the managing object should set up the public
//			variables as they need before calling Initialise()
//
// Note:	Assumes m_Parent is not null - obstacles should always be part of
//			an obstacle piece.
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class Obstacle 
{
	public Vector3 m_Position = new Vector3( 0.0f, 0.0f, 0.0f );
	
	public ObstaclePiece m_Parent = null;
	
	public enum Type
	{
		KILL_SPHERE = 0,
		KILL_BOX,
		
		PLATFORM,
	}
	public Type m_Type = Type.KILL_SPHERE;

	
	public virtual void Initialise() {}
	
	public virtual void DebugRender() {}
	
	public virtual bool CollideWithBox( Vector3 box_center, float box_width, float box_height )
	{ 
		return false; 
	}
	
	public virtual bool PlatformCollide( Vector3 box_center, float box_height, Vector3 movement_vector, ref Vector3 collision_point )
	{
		return false;
	}
	
	public float distanceOffset()
	{
		return m_Parent.m_Distance - ReferenceLibrary.m_SequenceManager.m_MasterDistance; 
	}
}

public class KillSphere : Obstacle
{
	public float m_Radius = 1.0f;
	
	
	public override void DebugRender()
	{
		Vector3 currentPos = m_Position + new Vector3( distanceOffset(), m_Parent.m_yOffset, 0.0f );
		
		DebugRenderHelpers.DrawFilledCircle( currentPos, m_Radius, new Color( 1.0f, 1.0f, 0.0f, 0.75f ) );
	}
	
	public override bool CollideWithBox( Vector3 box_center, float box_width, float box_height )
	{
		//	We're assuming the box is axis aligned here...
		
		//	Find the closest point within the rectangle to the center of the circle
		float closestX = Mathf.Clamp( m_Position.x, box_center.x - box_width*0.5f, box_center.x + box_width*0.5f );
		float closestY = Mathf.Clamp( m_Position.y, box_center.y - box_height*0.5f, box_center.y + box_height*0.5f );
		
		//	Now find the squared distance between this point and the circle center
		float xDist = closestX - m_Position.x;
		xDist *= xDist;
		float yDist = closestY - m_Position.y;
		yDist *= yDist;
		float sqDist = xDist + yDist;
		
		//	If this is less than the squared radius, then the box penetrates the circle
		if( sqDist < m_Radius*m_Radius )
			return true;
		else
			return false;
	}
}

public class KillBox : Obstacle
{
	public float m_Width = 1.0f;
	public float m_Height = 1.0f;
	
	
	public override void DebugRender()
	{
		Vector3 currentPos = m_Position + new Vector3( distanceOffset(), m_Parent.m_yOffset, 0.0f );
		
		DebugRenderHelpers.DrawFilled2DBoxC( currentPos, m_Width, m_Height, new Color( 1.0f, 1.0f, 0.0f, 0.75f ) );
	}
	
	public override bool CollideWithBox( Vector3 box_center, float box_width, float box_height )
	{
		//	Axis aligned boxes again...
		float halfMyWidth = m_Width*0.5f;
		float halfMyHeight = m_Height*0.5f;
		float halfBoxWidth = box_width*0.5f;
		float halfBoxHeight = box_height*0.5f;
		
		//	Process of elimination - if box is too far to the right on x axis there's no collision
		if( m_Position.x + halfMyWidth < box_center.x - halfBoxWidth )		return false;
		
		//	Too far to the left
		if( m_Position.x - halfMyWidth > box_center.x + halfBoxWidth )		return false;

		//	Too far up on the y axis
		if( m_Position.y + halfMyHeight < box_center.y - halfBoxHeight )	return false;
		
		//	Too far down
		if( m_Position.y - halfMyHeight > box_center.y + halfBoxHeight )	return false;
		
		//	If we got this far, then there's some overlap
		return true;
	}
}

public class Platform : Obstacle
{
	public float m_Width = 1.0f;
	
	public override void DebugRender()
	{
		Vector3 currentPos = m_Position + new Vector3( distanceOffset(), m_Parent.m_yOffset, 0.0f );
		
		DebugRenderHelpers.DrawLine( currentPos - new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
			currentPos + new Vector3( m_Width*0.5f, 0.0f, 0.0f ), 
			new Color( 0.0f, 0.0f, 1.0f, 0.75f ) );
	}	

	public override bool PlatformCollide( Vector3 box_center, float box_height, Vector3 movement_vector, ref Vector3 collision_point )
	{
		//	Do line intersection on the line of movement of the center-bottom point on the box (call it x1, y1 to x2, y2)
		//	and the line of the platform (call it x3, y3 to x4, y3).
		//	Simplifying assumptions are that the platform is flat on the x-axis and that x3 < x4
		
		//	Shift the box line down so it's in 'platform-space' on the y-axis
		float x1 = box_center.x;
		float y1 = box_center.y - box_height*0.5f - m_Position.y;
		
		float x2 = x1 + movement_vector.x;
		float y2 = y1 + movement_vector.y;
		
		float x3 = m_Position.x - m_Width*0.5f;		
		float x4 = x3 + m_Width;		
		
		//	This is like an axis-aligned bounding box check now with the platform box being infinitely thin on the y-axis
		if( y1 < 0.0f && y2 < 0.0f )
			return false;
		if( y1 > 0.0f && y2 > 0.0f )
			return false;
		if( x1 < x3 && x2 < x3 )
			return false;
		if( x1 > x4 && x2 > x4 )
			return false;
		
		//	There are still cases left...
		//	Now find the normalised point of intersection using the gradient of the box line,
		//	testing when it crosses the y == 0 line
		float dy = Mathf.Abs( y2 - y1 );
		float normalIntersection = Mathf.Abs( y1 )/dy;
		
		//	Therefore find the actual point of intersection on the x-axis
		float intersectX = x1 + normalIntersection*(x2 - x1);
		
		//	See if this is within the platform line segment
		if( intersectX < x3 || intersectX > x4 )
			return false;
		
		//	We have an intersection, find the point and put it back in world-space
		collision_point.x = intersectX;
		collision_point.y = m_Position.y;
		collision_point.z = 0.0f;
		
		return true;
	}
}