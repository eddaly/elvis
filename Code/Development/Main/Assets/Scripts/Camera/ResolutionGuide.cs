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


[ExecuteInEditMode]
public class ResolutionGuide : MonoBehaviour 
{
	public bool m_ShowGuides = true;
	
	void Start() 
	{
	}
	
	void Update() 
	{	
		if( m_ShowGuides )
			drawResolutionGuides();
	}

	void drawResolutionGuides()
	{
		float alpha = 0.5f;
		
		Vector3 activeAreaOffset = 
			new Vector3( EntityDefs.gActiveAreaXOffset, EntityDefs.gActiveAreaYOffset, 0.0f );
		
		float viewWidth = EntityDefs.gActiveAreaWidth;
		float viewHalfWidth = EntityDefs.gActiveAreaWidth*0.5f;
		
		//	The different aspect ratios are set up so that each one has the same visible horizontal distance.
		//	More of the viewport is cropped from the top than the bottom to try and keep the balance correct.
		//	The active area fits inside the widest aspect ratio leaving room for the HUD at the top.
		
		//	4:3
		float ratio = 3.0f/4.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth, 
			new Color( 1.0f, 0.0f, 0.0f, alpha ) );

		//	3:2
		ratio = 2.0f/3.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.95f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 1.0f, 0.2f, 0.2f, alpha ) );

		//	16:9
		ratio = 9.0f/16.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.9f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 1.0f, 0.4f, 0.4f, alpha ) );
		
		//	Active area
		ratio = 1.0f/2.0f;
		DebugRenderHelpers.Draw2DBox( new Vector3( -viewHalfWidth, ratio*viewHalfWidth*0.8f, 0.0f ) + activeAreaOffset, viewWidth, ratio*viewWidth,
			new Color( 0.0f, 1.0f, 0.0f, alpha ) );
		
		//	Just draw world-space active area for now
		DebugRenderHelpers.DrawLine( 
			new Vector3( -20.0f, 0.5f*viewHalfWidth*0.8f, 0.0f ) + activeAreaOffset,
			new Vector3( 100.0f, 0.5f*viewHalfWidth*0.8f, 0.0f ) + activeAreaOffset,
			new Color( 0.0f, 1.0f, 0.0f, alpha ) );
		
		DebugRenderHelpers.DrawLine( 
			new Vector3( -20.0f, 0.5f*viewHalfWidth*0.8f - viewHalfWidth, 0.0f ) + activeAreaOffset,
			new Vector3( 100.0f, 0.5f*viewHalfWidth*0.8f -  viewHalfWidth, 0.0f ) + activeAreaOffset,
			new Color( 0.0f, 1.0f, 0.0f, alpha ) );
		
		//	Some distance markers
		float dist = RL.m_Sequencer.m_MasterDistance;
		int iDist = (int)(dist/10.0f);
		dist -= (float)iDist*10.0f;
		
		for( int d = -20; d<100; d += 2 )
		{
			float markSize = 0.25f;
			if( d%10 == 0 )
				markSize = 0.5f;
			
			DebugRenderHelpers.DrawLine( 
				new Vector3( (float)d - dist, 0.5f*viewHalfWidth*0.8f + markSize*0.5f, 0.0f ) + activeAreaOffset,
				new Vector3( (float)d - dist, 0.5f*viewHalfWidth*0.8f - markSize*0.5f, 0.0f ) + activeAreaOffset,
				new Color( 0.0f, 1.0f, 0.0f, alpha ) );
			
			DebugRenderHelpers.DrawLine( 
				new Vector3( (float)d - dist, 0.5f*viewHalfWidth*0.8f + markSize*0.5f - viewHalfWidth, 0.0f ) + activeAreaOffset,
				new Vector3( (float)d - dist, 0.5f*viewHalfWidth*0.8f - markSize*0.5f - viewHalfWidth, 0.0f ) + activeAreaOffset,
				new Color( 0.0f, 1.0f, 0.0f, alpha ) );
		}
	} 
}
