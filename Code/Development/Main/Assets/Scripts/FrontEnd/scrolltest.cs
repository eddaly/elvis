using UnityEngine;
using System.Collections;

public class scrolltest : MonoBehaviour {
	
	FastGUIElement play;
	FastGUIScrollWindow scrollWindow;
	FastGUIElement levelIcon;

	// Use this for initialization
	void Start () {
	
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SetOriginalAtlasPixels (2048, 1536);
	
		play = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768));		// Atlas position
		
		scrollWindow = new FastGUIScrollWindow (
			new Vector2 (0, play.height),	// Screen position
			new Vector4 (0, 768, 1024, 768),	// Atlas position, window
			new Vector4 (0, 768, 2048, 768));	// Atlas position, total area
		
		levelIcon = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position (will be overridden when added to ScrollWindow)
			new Vector4 (0, 0, 128, 128));		// Atlas position
		
		scrollWindow.Add (levelIcon,
			new Vector2 (0, 100));				// Position within ScrollWindow
	}
	
	// Update is called once per frame
	void Update () {
		FastGUIElement.DebugDrawSafeArea ();
		
		scrollWindow.Update ();
		
		if (levelIcon.Tapped ())
		{
			Debug.Log ("Tapped Icon");
		}
		if (scrollWindow.Tapped ())
		{
			Debug.Log ("Tapped ScrollWindow");
		}
		if (play.Tapped ())
		{
			Debug.Log ("Tapped PLAY");
		}
	}
}
