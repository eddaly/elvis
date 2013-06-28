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
			new Vector2 (0, 0),				// Screen position ***Fraser wanted these in pixels (i.e. WCs) as he'll pull them from Photoshop
			new Vector4 (0, 0, 2048, 768));		
		
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
		
		// Safe area guides
		Debug.DrawLine (new Vector3 (-FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (-FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
		Debug.DrawLine (new Vector3 (+FastGUIElement.safeScreenWidth/2f, -FastGUIElement.safeScreenHeight/2f, 0), 
			new Vector3 (+FastGUIElement.safeScreenWidth/2f, +FastGUIElement.safeScreenHeight/2f, 0), Color.red);
	}
}
