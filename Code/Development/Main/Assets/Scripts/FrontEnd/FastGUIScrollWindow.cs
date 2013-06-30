using UnityEngine;
using System.Collections;

public class FastGUIScrollWindow : FastGUIElement
{
	Vector2 firstPos, lastPos, velocity, movement;
	bool decelerate = false;
	
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
		
		TextureAtlas frontendAtlas = PrimitiveLibrary.Get.m_Atlases[(int)TextureAtlas.AtlasID.FRONTEND];		
			
		// Get movement since last Update
		Vector2 touchPos;
#if UNITY_IPHONE
		if (Input.GetTouch(0).phase == TouchPhase.Began)
#else
		if (Input.GetMouseButtonDown (0))
#endif
		{
			getTouchPos (out firstPos);
			lastPos = firstPos;
		}
#if UNITY_IPHONE
		if (Input.GetTouch(0).phase == TouchPhase.Moved)
#else
		if (Input.GetMouseButton (0))
#endif
		{
			getTouchPos (out touchPos);
			velocity = touchPos - firstPos;
			movement = touchPos - lastPos;
			lastPos = touchPos;
		}
#if UNITY_IPHONE
		if (Input.GetTouch(0).phase == TouchPhase.Stationary)
#else
		if (movement.magnitude < Mathf.Epsilon)
#endif
		{
			// Stop still
			velocity = Vector2.zero;
		}
		
#if UNITY_IPHONE
		if (Input.GetTouch(0).phase == TouchPhase.Ended)
#else
		if (Input.GetMouseButtonUp (0))
#endif
		{
			// start decelerating
			decelerate = true;
		}
		if (decelerate)
		{
			Vector3 v3 = new Vector3 (velocity.x, 0, 0);
			v3 *= .1f;
			quad.m_Position += v3;
			// Scroll the UVs
			
			//frontendAtlas.m_UVSet[textureIdx].x += v3.x / originalAtlasPixelsWidth;
			//if (frontendAtlas.m_UVSet[textureIdx].x > frontendAtlas.m_UVSet[textureIdx].x //***clamp both sides
																							//***set it up to be initial window
																							//*** and will need max window to clamp to
			velocity = velocity * .99f;
			if (velocity.magnitude  < Mathf.Epsilon)
			{
				decelerate = false;
			}
		}
		else
		{
			Vector3 m3 = new Vector3 (movement.x, 0, 0);
			m3 *= 3f;
			quad.m_Position += m3;
		}
	}
}

