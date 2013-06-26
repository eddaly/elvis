//-----------------------------------------------------------------------------
// File: GizmoHelpers.cs
//
// Desc:	Gizmo renderers
//
// Copyright Echo Peak Ltd 2013
// <Charles James>
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class GizmoHelpers
{
	public static void RayTest( Vector3 start_point, Vector3 direction )
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay( start_point, direction );
	}
	
	public static void Cone( 	Vector3 start_point, float start_radius, Vector3 tangent,
								Vector3 end_point, float end_radius )
	{
		Vector3 direction = end_point - start_point;
		direction.Normalize();
		
		Vector3 cross = Vector3.Cross( direction, tangent );
		
		int circleSections = 20;
		int sectionsPerSpoke = 2;
		
		float angle = 0.0f;
		float dAngle = (Mathf.PI*2.0f)/(float)circleSections;
		
		for( int s = 0; s<circleSections; s++ )
		{
			Vector3 start1 = 	start_radius*tangent*Mathf.Cos( angle ) + 
								start_radius*cross*Mathf.Sin( angle );
			start1 += start_point;
			Vector3 start2 = 	start_radius*tangent*Mathf.Cos( angle + dAngle ) + 
								start_radius*cross*Mathf.Sin( angle + dAngle );
			start2 += start_point;

			Vector3 end1 = 		end_radius*tangent*Mathf.Cos( angle ) + 
								end_radius*cross*Mathf.Sin( angle );
			end1 += end_point;
			Vector3 end2 = 		end_radius*tangent*Mathf.Cos( angle + dAngle ) + 
								end_radius*cross*Mathf.Sin( angle + dAngle );
			end2 += end_point;
			
			Gizmos.DrawLine( start1, start2 );
			Gizmos.DrawLine( end1, end2 );
			
			if( s % sectionsPerSpoke == 0 )
				Gizmos.DrawLine( start1, end1 );
			
			angle += dAngle;
		}
	}
}
