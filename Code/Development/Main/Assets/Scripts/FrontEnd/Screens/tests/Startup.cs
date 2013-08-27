using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	
	FastGUIElement playFGE, persistentDataFGE;
	
	// NOTE: iPad Retina is 2048 x 768
	
	// Use this for initialization
	void Start () {
	
		FrontEnd.instance.SelectAtlas ("EdTestAtlas");
			
		playFGE = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Rect (0, 0, 2048, 768));		// Atlas position
		
		persistentDataFGE = new FastGUIElement (
			new Vector2 (0, playFGE.height),	// Screen position
			new Rect (0, 768, 2048, 768));	// Atlas position
	}
		
	// Update is called once per frame
	void Update () {
			
		if (playFGE.Tapped ())
		{
			Debug.Log ("PLAY");
			FrontEnd.UnloadAtlases ();
			Application.LoadLevel ("RunningProto3");
		}
		else if (persistentDataFGE.Tapped ())
		{
			Debug.Log ("PERSISTENT DATA");
			Application.LoadLevel ("Store");
		}
	}
}
