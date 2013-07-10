using UnityEngine;
using System.Collections;

public class store : MonoBehaviour {
	
	FastGUIElement backGround;
	FastGUIButton backButton;
	FastGUIElement UpgradesTab;
	FastGUIElement DescriptionTab;
	FastGUIElement UpgradesButton;
	FastGUIElement DescriptionButton;


	// Use this for initialization
	void Start () {
		
		backGround = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			new Vector4 (1798, 2, 2048, 1536));	// Atlas position
		
		UpgradesTab = new FastGUIElement (
			new Vector2 (1024, 382),			// Screen position
			new Vector4 (900, 2, 896, 768));	// Atlas position

		DescriptionTab = new FastGUIElement (
			new Vector2 (1024, 382),			// Screen position
			new Vector4 (2, 2, 896, 768));		// Atlas position
		DescriptionTab.SetDisplayed (false);
		
		UpgradesButton = new FastGUIElement (
			new Vector2 (1472, 382),			// Screen position
			new Vector4 (450, 2, 448, 96));		// Atlas position
		
		DescriptionButton = new FastGUIElement (
			new Vector2 (1024, 382),			// Screen position
			new Vector4 (2, 2, 448, 96));		// Atlas position
		
		backButton = new FastGUIButton (
			new Vector2 (0, 0),
			new Vector4 (1798, 2, 3*64, 3*64),
			new Vector4 (1000, 0, 3*64, 3*64));
	}
	
	// Update is called once per frame
	void Update () {
		
		if (backButton.UpdateTestPressed ())
		{
			Debug.Log ("BACK");
		}
		else if (DescriptionButton.Tapped ())
		{
			DescriptionTab.SetDisplayed (true);
			UpgradesTab.SetDisplayed (false);			
		}
		else if (UpgradesButton.Tapped ())
		{
			DescriptionTab.SetDisplayed (false);
			UpgradesTab.SetDisplayed (true);			
		}
	}
}
