using UnityEngine;
using System.Collections;

public class scrolltest : MonoBehaviour {
	
	FastGUIElement play,box;
	FastGUIScrollWindow scrollWindow;
	FastGUIElement levelIcon;

	FastGUIElement popup;
	FastGUIButton popupButton;

	// Use this for initialization
	void Start ()
	{
		box = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 1536));		// Atlas position
		/*play = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768));		// Atlas position
		
		scrollWindow = new FastGUIScrollWindow (
			new Vector2 (0, play.height),		// Screen position
			new Vector4 (0, 768, 1024, 768),	// Atlas position, window
			new Vector4 (0, 768, 2048, 768));	// Atlas position, total area
		
		levelIcon = new FastGUIElement (
			scrollWindow,
			new Vector2 (0, 100),				// Position within ScrollWindow
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
	*/}
	
	// Update is called once per frame
	void Update ()
	{	
		// Needed to handle inputs
	/*	scrollWindow.Update ();
		if (popupButton.UpdateTestPressed ())
		{
			Debug.Log ("Tapped RED Button");
			popup.SetDisplayed (false);
		}
		
		if (play.Tapped ())
		{
			Debug.Log ("Tapped PLAY");
		}
		if (scrollWindow.Tapped ())
		{
			Debug.Log ("Tapped ScrollWindow");
		}
		if (levelIcon.Tapped ())
		{
			Debug.Log ("Tapped Icon");
			popup.SetDisplayed (true);
		}*/
	}
}
