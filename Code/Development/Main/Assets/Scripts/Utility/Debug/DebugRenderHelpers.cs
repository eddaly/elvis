//-----------------------------------------------------------------------------
// File: DebugRendererHelpers.cs
//
// Desc:	Shorthand for drawing more complicated shapes with debug renderer
//
// Copyright Echo Peak Ltd 2013
//-----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class DebugRenderHelpers
{
	public static void DrawLine( Vector3 start, Vector3 end, Color colour )
	{
		Debug.DrawLine( start, end, colour );
	}
	
	public static void Draw2DBox( Vector3 top_left, float width, float height, Color colour )
	{
		Debug.DrawLine( top_left, top_left + new Vector3( width, 0.0f, 0.0f ), colour );
		Debug.DrawLine( top_left, top_left + new Vector3( 0.0f, -height, 0.0f ), colour );
		Debug.DrawLine( 
			top_left + new Vector3( width, 0.0f, 0.0f ), 
			top_left + new Vector3( width, -height, 0.0f ),
			colour );
		Debug.DrawLine( 
			top_left + new Vector3( 0.0f, -height, 0.0f ), 
			top_left + new Vector3( width, -height, 0.0f ),
			colour );
	}	

	public static void DrawFilled2DBoxC( Vector3 center, float width, float height, Color colour )
	{
		Vector3 topLeft = new Vector3( center.x - width*0.5f, center.y + height*0.5f, center.z );
		
		Debug.DrawLine( topLeft, topLeft + new Vector3( width, 0.0f, 0.0f ), colour );
		Debug.DrawLine( topLeft, topLeft + new Vector3( 0.0f, -height, 0.0f ), colour );
		Debug.DrawLine( topLeft + new Vector3( width, 0.0f, 0.0f ), topLeft + new Vector3( width, -height, 0.0f ), colour );
		Debug.DrawLine( topLeft + new Vector3( 0.0f, -height, 0.0f ), topLeft + new Vector3( width, -height, 0.0f ), colour );
		
		Debug.DrawLine( topLeft, topLeft + new Vector3( width, -height, 0.0f ),	colour );
		Debug.DrawLine( topLeft + new Vector3( 0.0f, -height, 0.0f ), topLeft + new Vector3( width, 0.0f, 0.0f ), colour );
		
		Debug.DrawLine( topLeft + new Vector3( width*0.5f, 0.0f, 0.0f ), topLeft + new Vector3( width*0.5f, -height, 0.0f ), colour );
		Debug.DrawLine( topLeft + new Vector3( 0.0f, -height*0.5f, 0.0f ), topLeft + new Vector3( width, -height*0.5f, 0.0f ), colour );
	}	
	
	public static void DrawFilledCircle( Vector3 center, float radius, Color colour )
	{
		const int numEdges = 17;
		float angle = 0.0f;
		float dAngle = (Mathf.PI*2.0f)/(float)numEdges;
		
		Vector3 previousPoint = center + new Vector3( radius, 0.0f, 0.0f );
		Vector3 currentPoint;
		
		for( int e = 0; e<numEdges; e++ )
		{
			angle += dAngle;
			
			currentPoint = center + new Vector3( radius*Mathf.Cos( angle ), radius*Mathf.Sin( angle ), 0.0f );
			
			Debug.DrawLine( previousPoint, currentPoint, colour );
			Debug.DrawLine( center, currentPoint, colour );
			
			previousPoint = currentPoint;
		}
	}
}
