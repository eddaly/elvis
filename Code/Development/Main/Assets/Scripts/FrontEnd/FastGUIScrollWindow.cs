using UnityEngine;
using System.Collections;

public class FastGUIScrollWindow : FastGUIElement
{
	Vector2 lastPos, velocity, movement;
	bool autoscroll = false;
	
	public FastGUIScrollWindow (
		Vector2 topLeft,	// Co-ords origin is top-left in pixels
		Vector4 uvs, 		// x,y is top-left, z,w is width and height in pixels
		Position pos = Position.TOPLEFT) : base (topLeft, uvs, pos)
	{
		
	}
	
	// Update should be called once per frame to update
	public void Update ()
	{
		//***if in area
		
		//TextureAtlas frontendAtlas = PrimitiveLibrary.Get.m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
			
		Vector2 touchPos;
		
		// If just touched the screen
#if UNITY_IPHONE
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#else
		if (Input.GetMouseButtonDown (0))
#endif
		{
			autoscroll = false;			// Stop previous autoscroll
			getTouchPos (out lastPos);	// Record initial position, used to set initial velocity of autoscroll
		}
		
		// If moved the touch
#if UNITY_IPHONE
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
#else
		if (Input.GetMouseButton (0))
#endif
		{
			getTouchPos (out touchPos);
#if UNITY_IPHONE			
			velocity = Input.GetTouch(0).deltaPosition / Input.GetTouch(0).deltaTime; // Velocity for autoscroll (touch only)
#endif
			movement = touchPos - lastPos;	// Movement in frame
			lastPos = touchPos;

			// Move the quad position
			Vector3 m3 = new Vector3 (movement.x, 0, 0); // X-axis only
			m3 *= 3f;	// Scale to scroll with finger position (set with trial and error on iPhone5!)
			if (m3.magnitude > 10) // Unless barely moving (at least with touch)
				quad.m_Position += m3;
		}
		
		// If touch ended start autoscroll
#if UNITY_IPHONE
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			autoscroll = true;
			movement = Vector2.zero;
		}
		
		// Autoscroll
		if (autoscroll)
		{
			Vector3 v3 = new Vector3 (velocity.x, 0, 0);
			v3 *= Time.deltaTime;	// Scale scroll speed to framerate
			quad.m_Position += v3;
			//frontendAtlas.m_UVSet[textureIdx].x += v3.x / originalAtlasPixelsWidth;
			//if (frontendAtlas.m_UVSet[textureIdx].x > frontendAtlas.m_UVSet[textureIdx].x //***clamp both sides
																							//***set it up to be initial window
																							//*** and will need max window to clamp to
			// Decelerate until stop
			velocity = velocity * .9f;
			if (velocity.magnitude  < Mathf.Epsilon)
			{
				autoscroll = false;
			}
		}
#endif
	}
}

