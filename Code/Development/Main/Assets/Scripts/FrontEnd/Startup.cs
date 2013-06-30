using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	
	FastGUIElement playFGE, persistentDataFGE;
	
	// NOTE: iPad Retina is 2048 x 768
	
	// Use this for initialization
	void Start () {
		
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SetOriginalAtlasPixels (2048, 1536);
	
		playFGE = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768));		// Atlas position
		
		persistentDataFGE = new FastGUIElement (
			new Vector2 (0, playFGE.height),	// Screen position
			new Vector4 (0, 768, 2048, 768));	// Atlas position
	}
	
	// Update is called once per frame
	void Update () {
	
		if (playFGE.Tapped ())
		{
			Debug.Log ("PLAY");
		}
		else if (persistentDataFGE.Tapped ())
		{
			Debug.Log ("PERSISTENT DATA");
		}
		
		FastGUIElement.DebugDrawSafeArea ();
	}
}
