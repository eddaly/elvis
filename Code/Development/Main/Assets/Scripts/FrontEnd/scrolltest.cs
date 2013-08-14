using UnityEngine;
using System.Collections;

public class scrolltest : MonoBehaviour {
#pragma warning disable 414
	FastGUIElement play;
	FastGUIScrollWindow hScrollWindow, vScrollWindow;
	FastGUIElement levelIcon, levelIcon2;
	FastGUIElement popup;
	FastGUIButton popupButton;
#pragma warning restore 414
	
	bool testAutoScroll = false;
	
	// Use this for initialization
	void Start ()
	{
		play = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768));		// Atlas position
		
		hScrollWindow = new FastGUIScrollWindow (
			new Vector2 (0, play.height + 10),		// Screen position
			new Vector4 (0, 768, 1024, 768),	// Atlas position, window
			new Vector4 (0, 768, 2048, 768));	// Atlas position, total area		
		levelIcon = new FastGUIElement (
			hScrollWindow,
			new Vector2 (0, 0),				// Position within ScrollWindow
			new Vector4 (0, 0, 128, 128));		// Atlas position

		vScrollWindow = new FastGUIScrollWindow (
			new Vector2 (hScrollWindow.width, play.height + 10),		// Screen position
			new Vector4 (0, 0, 1024, 768),			// Atlas position, window
			new Vector4 (0, 0, 1024, 1536));		// Atlas position, total area		
		levelIcon2 = new FastGUIElement (
			vScrollWindow,
			new Vector2 (vScrollWindow.width - 128, 0),					// Position within ScrollWindow
			new Vector4 (0, 0, 128, 128));		// Atlas position
			
		// Make the popup window
		popup = new FastGUIElement (
			new Vector2 (2048*.75f, 1536*.75f),	// Screen position
			new Vector4 (200, 0, 500, 400),		// Atlas position
			FastGUIButton.Position.XYCENTRED);	
		
		// Make and Add the button to go on it
		popupButton = new FastGUIButton (
			new Vector2 (0, 0),					// Screen position (will be ignored)
			new Vector4 (0, 0, 128, 128),		// Atlas position
			new Vector4 (128, 0, 128, 128));	// Atlas position when pressed
		popup.Add (popupButton, new Vector2 (popup.width/2 - popupButton.width/2, popup.height/2 - popupButton.height/2));
		
		// Hide it until ready
		popup.SetDisplayed (false);
	}
	
	// Update is called once per frame
	void Update ()
	{			
		if (testAutoScroll)	{
			if (!vScrollWindow.ScrollUVs (new Vector2 (0, GlobalData.Get.m_GlobalDTime * 100)))
				testAutoScroll = false;
		}

		// Needed to handle inputs
		hScrollWindow.Update ();
		vScrollWindow.Update ();
		if (popupButton.UpdateTestPressed ())
		{
			Debug.Log ("Tapped RED Button");
			popup.SetDisplayed (false);
			testAutoScroll = true;
		}
		
		if (play.Tapped ())
		{
			Debug.Log ("Tapped PLAY");
		}
		if (hScrollWindow.Tapped ())
		{
			Debug.Log ("Tapped HScrollWindow");
		}
		if (levelIcon.Tapped ())
		{
			Debug.Log ("Tapped Icon");
			popup.SetDisplayed (true);
		}
		if (vScrollWindow.Tapped ())
		{
			Debug.Log ("Tapped VScrollWindow");
		}
		if (levelIcon2.Tapped ())
		{
			Debug.Log ("Tapped Icon2");
			popup.SetDisplayed (true);
		}
	}
}
