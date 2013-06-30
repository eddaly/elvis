using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FastGUIScrollWindow : FastGUIElement
{
	private Vector2 lastPos, velocity, movement;
#if UNITY_IPHONE || UNITY_ANDROID
	private bool autoscroll = false;
#endif
	private bool scrollStarted = false;
	private Vector4 atlasRectMaxNormalised;
	private List <FastGUIElement> children;
	
	// Make a ScrollWindow, will immediately render
	public FastGUIScrollWindow (
		Vector2 screenPos,			// Position on screen (relative to Position)
		Vector4 atlasRectWindow, 	// Atlas rectangle window (x,y is top-leftx, z,w is width and height) in pixels
		Vector4 atlasRectMax, 		// Atlas rectangle for the maximum scrollable area
		Position pos = Position.TOPLEFT) : base (screenPos, atlasRectWindow, pos)
	{
		atlasRectMaxNormalised = new Vector4 (
			atlasRectMax.x / originalAtlasPixelsWidth,
			atlasRectMax.y / originalAtlasPixelsHeight, 
			atlasRectMax.z / originalAtlasPixelsWidth, 
			atlasRectMax.w / originalAtlasPixelsHeight);
		children = new List<FastGUIElement>();
	}
	
	// Add an Element to the ScrollWindow at the provided position
	public void Add (FastGUIElement child,
		Vector2 pos)		// x,y is top-left in pixels
	{
		children.Add (child);
		child.quad.m_Position =  new Vector3 (
			quad.m_Position.x - width/2 + child.width/2 + pos.x,
			quad.m_Position.y + height/2 - child.height/2 - pos.y, 0);
		child.UpdatedQuad ();
	}
	
	// Update should be called once per frame to update
	public void Update ()
	{
		//***if in area			
		
		// If just touched the screen
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			autoscroll = false;				// Stop previous autoscroll
#else
		if (Input.GetMouseButtonDown (0))
		{
#endif
			if (getTouchPos (out lastPos))	// Record initial position, used to set initial velocity of autoscroll
				scrollStarted = screenRect.Contains (lastPos);
		}
		
		// If moved the touch
#if UNITY_IPHONE || UNITY_ANDROID
		if (scrollStarted && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
#else
		if (scrollStarted && Input.GetMouseButton (0))
#endif
		{
			Vector2 touchPos;
			getTouchPos (out touchPos);
#if UNITY_IPHONE || UNITY_ANDROID
			velocity = Input.GetTouch(0).deltaPosition / Input.GetTouch(0).deltaTime; // Velocity for autoscroll (touch only)
#endif
			movement = touchPos - lastPos;	// Movement in frame
			lastPos = touchPos;

			// Move the UVs
			Vector4 m4 = new Vector4 (movement.x, 0, movement.x, 0); // X-axis only
			m4 *= 3f;					// Scale to scroll with finger position (set with trial and error on iPhone5!)
			if (m4.magnitude > 10)		// Unless barely moving (at least with touch)
				scrollUVs (m4);
		}
		
		// If touch ended start autoscroll
#if UNITY_IPHONE || UNITY_ANDROID
		if (scrollStarted && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			autoscroll = true;
			movement = Vector2.zero;
		}
		
		// Autoscroll
		if (autoscroll)
		{
			Vector3 v4 = new Vector4 (velocity.x, 0, velocity.x, 0);	// X-axis only
			v4 *= Time.deltaTime;	// Scale scroll speed to framerate
			scrollUVs (v4);
			// Decelerate until stop
			velocity = velocity * .9f;
			if (velocity.magnitude  < Mathf.Epsilon)
			{
				autoscroll = false;
			}
		}
#endif
	}
	
	// Scroll the UVs, with clamping, and of child elements
	private void scrollUVs (Vector4 v)
	{
		Vector4 vNormalised = new Vector4 (
				v.x / originalAtlasPixelsWidth, v.y / originalAtlasPixelsHeight,
				v.z / originalAtlasPixelsWidth, v.w / originalAtlasPixelsHeight);
		
		// Move the UVs
		frontendAtlas.m_UVSet[textureIdx] -= vNormalised;
		
		// Clamp
		if (frontendAtlas.m_UVSet[textureIdx].x < atlasRectMaxNormalised.x)
		{
			v.x -= (atlasRectMaxNormalised.x - frontendAtlas.m_UVSet[textureIdx].x) * originalAtlasPixelsWidth;
			frontendAtlas.m_UVSet[textureIdx].x = atlasRectMaxNormalised.x;
			frontendAtlas.m_UVSet[textureIdx].z = atlasRectMaxNormalised.x + widthNormalised;
		}
		else if (frontendAtlas.m_UVSet[textureIdx].z > atlasRectMaxNormalised.z)
		{
			v.x += (frontendAtlas.m_UVSet[textureIdx].z - atlasRectMaxNormalised.z) * originalAtlasPixelsWidth;
			frontendAtlas.m_UVSet[textureIdx].x = atlasRectMaxNormalised.z - widthNormalised;
			frontendAtlas.m_UVSet[textureIdx].z = atlasRectMaxNormalised.z;
		}
		
		// Move the children inline with scrolled background
		Vector3 p = new Vector3 (v.x, v.y, 0);
		for (int n = 0; n < children.Count; n++)
		{
			children[n].quad.m_Position += p;
			children[n].UpdatedQuad ();
		}
			
		// TODO it going outside the window, i.e. clipping it.  However that only needed if is any visible space outside the window
		// as long as even the unsafe area used by windows this isn't a problem.  And would probably put a 'mask' over it anyway
	}

}

