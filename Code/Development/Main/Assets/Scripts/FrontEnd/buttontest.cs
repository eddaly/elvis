using UnityEngine;
using System.Collections;

public class buttontest : MonoBehaviour
{
	FastGUIButton playFGE;
	FastGUIProgressBar progressBar;
	float progressComplete = 0;
	
	// Use this for initialization
	void Start () {
		
		/*playFGE = new FastGUIButton (
			new Vector2 (0, 0),				// Screen position
			new Rect (2048, 0, 4096, 768),	// Atlas position
			new Rect (0, 768, 2048, 768));	// Atlas position when pressed
	*/
		progressBar = new FastGUIProgressBar (
			new Vector2 (0, 768),
			new Rect (2048, 0, 1, 1),//4096, 768),
			new Rect (2048 + 4096/4, 0 + 768/4, 4096/2, 768/2),
			Color.red);
	}
	
	// Update is called once per frame
	void Update () {
	
		//if (playFGE.UpdateTestPressed ())
		{
			Debug.Log ("PLAY");
		}
		
		if (Input.GetKeyUp(KeyCode.Space))
		{
			progressComplete += .1f;
		}
		progressBar.Update (progressComplete);
	}
}

