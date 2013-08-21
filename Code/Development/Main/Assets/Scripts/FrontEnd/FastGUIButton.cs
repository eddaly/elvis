using UnityEngine;
using System.Collections;

public class FastGUIButton : FastGUIElement
{
	private FastGUIElement pressedButton;
	
	// Make a Button and add to the ScrollWindow, will immediately render
	public FastGUIButton (
		FastGUIScrollWindow scrollWindow,	// Add this element to ScrollWindow
		Vector2 scrollWindowPos,			// Position in window (origin is top-left) in pixels
		Rect	atlasRect, 					// Atlas rectangle in pixels
		Rect	atlasRectPressedButton) 	// Atlas rectangle for the pressed Button
		: base (scrollWindow, scrollWindowPos, atlasRect)
	{
		pressedButton = new FastGUIElement (scrollWindow, scrollWindowPos, atlasRectPressedButton);
		pressedButton.SetDisplayed (false);
	}

	// Make a Button, will immediately render
	public FastGUIButton (
		Vector2 screenPos,					// Position on screen (relative to Position)
		Rect	atlasRect, 					// Atlas rectangle in pixels
		Rect	atlasRectPressedButton, 	// Atlas rectangle for the pressed Button
		Position pos = Position.TOPLEFT)
		: base (screenPos, atlasRect, pos)
	{
		pressedButton = new FastGUIElement (screenPos, atlasRectPressedButton, pos);
		pressedButton.SetDisplayed (false);
	}
	
	// Added by this parent Element at the provided position
	protected internal override void AddedBy (FastGUIElement parent,
		Vector2 pos)		// x,y is top-left in pixels
	{
		base.AddedBy (parent, pos);
		pressedButton.SetDisplayed (true);	// Need to display before adding as had moved the quad off screen! (at least because otherwise the displayed flag is confused)
		pressedButton.AddedBy (parent, pos);
		pressedButton.SetDisplayed (false);	// And hide again
	}
	
	// Update to see if pressed, display the pressed texture, return true when press complete
	public bool UpdateTestPressed ()
	{
		if (!displayed)
			return false;
		
		// Get position
		Vector3 pos;		
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount == 0)
			return false;
		pos = Input.GetTouch(0).position;
#else
		pos = Input.mousePosition;
#endif			
		
#if UNITY_IPHONE || UNITY_ANDROID
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#else
		if (Input.GetMouseButton(0))
#endif
		{
			// If touched the button, display it, else remove it
			pressedButton.SetDisplayed (screenRect.Contains (pos));
		}
#if UNITY_IPHONE || UNITY_ANDROID			
		else if (Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			// If moved away from button, don't display pressed button texture
			if (!screenRect.Contains (pos))
				pressedButton.SetDisplayed (false);
		}			
#endif							

#if UNITY_IPHONE || UNITY_ANDROID			
		else if (Input.GetTouch(0).phase == TouchPhase.Ended)
#else
		else if (Input.GetMouseButtonUp(0))
#endif
		{
			// Hide the pressed button texture
			pressedButton.SetDisplayed (false);

			// If touch ended in the button, press is complete, else cancel
			return screenRect.Contains (pos);
		}
		return false;
	}
	
}