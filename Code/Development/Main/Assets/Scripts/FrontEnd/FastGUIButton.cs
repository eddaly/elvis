using UnityEngine;
using System.Collections;

public class FastGUIButton : FastGUIElement
{
	private FastGUIElement pressedButton;
	
	// Make a Button and add to the ScrollWindow, will immediately render
	public FastGUIButton (
		FastGUIScrollWindow scrollWindow,	// Add this element to ScrollWindow
		Vector2 scrollWindowPos,			// Position in window (origin is top-left) in pixels
		Vector4 atlasRect, 					// Atlas rectangle (x,y is top-leftx, z,w is width and height) in pixels
		Vector4 atlasRectPressedButton) 	// Atlas rectangle for the pressed Button
		: base (scrollWindow, scrollWindowPos, atlasRect)
	{
		pressedButton = new FastGUIElement (scrollWindow, scrollWindowPos, atlasRectPressedButton);
		pressedButton.quad.m_Position += new Vector3 (10000, 0, 0);
	}

	// Make a Button, will immediately render
	public FastGUIButton (
		Vector2 screenPos,					// Position on screen (relative to Position)
		Vector4 atlasRect, 					// Atlas rectangle (x,y is top-leftx, z,w is width and height) in pixels
		Vector4 atlasRectPressedButton, 	// Atlas rectangle for the pressed Button
		Position pos = Position.TOPLEFT)
		: base (screenPos, atlasRect, pos)
	{
		pressedButton = new FastGUIElement (screenPos, atlasRectPressedButton, pos);
		pressedButton.quad.m_Position += new Vector3 (10000, 0, 0);
	}
	
	// Update to see if pressed, display the pressed texture, return true when press complete
	public bool UpdateTestPressed ()
	{
		Vector3 pos;
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			pos = Input.GetTouch(0).position;
#else
		if (Input.GetMouseButtonDown(0))
		{
			pos = Input.mousePosition;
#endif			
			// If touched the button
			if (screenRect.Contains (pos))
			{
				// Display pressed Button quad (bring back in view and in front of unpressed button)
				pressedButton.quad.m_Position -= new Vector3 (10000, 0, 0);
			}
		}
#if UNITY_IPHONE || UNITY_ANDROID			
		else if (Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			pos = Input.GetTouch(0).position;
#else
		else if (Input.GetMouseButtonUp(0))
		{
			pos = Input.mousePosition;
#endif							
			// Pull away the display pressed button texture
			pressedButton.quad.m_Position += new Vector3 (10000, 0, 0);

			// If touch ended in the button, press complete, else cancel
			return screenRect.Contains (pos);
		}
		return false;
	}
	
}