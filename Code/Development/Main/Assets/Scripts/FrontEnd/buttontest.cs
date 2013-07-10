using UnityEngine;
using System.Collections;

public class buttontest : MonoBehaviour
{
	FastGUIButton playFGE;
	
	// Use this for initialization
	void Start () {
		
		playFGE = new FastGUIButton (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (0, 0, 2048, 768),		// Atlas position
			new Vector4 (0, 768, 2048, 768));	// Atlas position when pressed
	}
	
	// Update is called once per frame
	void Update () {
	
		if (playFGE.UpdateTestPressed ())
		{
			Debug.Log ("PLAY");
		}		
	}
}

