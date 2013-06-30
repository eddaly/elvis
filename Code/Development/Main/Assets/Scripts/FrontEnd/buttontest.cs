using UnityEngine;
using System.Collections;

public class buttontest : MonoBehaviour
{
	FastGUIButton playFGE;
	
	// Use this for initialization
	void Start () {
		
		// Need to specify if want to set UVs with original pixels because Unity rescales texture on load
		FastGUIElement.SetOriginalAtlasPixels (2048, 1536);
	
		playFGE = new FastGUIButton (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768),		// Atlas position
			new Vector4 (0, 768, 2048, 768));	// Atlas position
	}
	
	// Update is called once per frame
	void Update () {
		FastGUIElement.DebugDrawSafeArea ();
	
		if (playFGE.UpdateTestPressed ())
		{
			Debug.Log ("PLAY");
		}		
	}
}

