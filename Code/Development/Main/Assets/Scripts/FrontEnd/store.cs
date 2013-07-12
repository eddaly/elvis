using UnityEngine;
using System.Collections;

public class store : MonoBehaviour {
#pragma warning disable 414
	FastGUIElement backGround;
	FastGUIButton backButton;
	FastGUIElement upgradesTab;
	FastGUIElement descriptionTab;
	FastGUIElement upgradesButton;
	FastGUIElement descriptionButton;
#pragma warning restore 414

	// Use this for initialization
	void Start () {

		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//test_atlus.xml";
			
		backGround = new FastGUIElement (
			new Vector2 (0, 0),					// Screen position
			FastGUIElement.UVsFrom (@"Elvis_FE_Test.psd"));
		
		upgradesTab = new FastGUIElement (
			new Vector2 (1024, 382),			// Screen position
			FastGUIElement.UVsFrom (@"Elvis_FE_Store_UpgradesTab.png"));

		descriptionTab = new FastGUIElement (
			new Vector2 (1024, 382),			// Screen position
			FastGUIElement.UVsFrom (@"Elvis_FE_Store_DescriptionTab.png"));
		descriptionTab.SetDisplayed (false);
		
		upgradesButton = new FastGUIElement (
			new Vector2 (1472, 382),			// Screen position
			new Vector4 (450, 2, 448, 96));		// Atlas position
		
		descriptionButton = new FastGUIElement (
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
		else if (descriptionButton.Tapped ())
		{
			descriptionTab.SetDisplayed (true);
			upgradesTab.SetDisplayed (false);			
		}
		else if (upgradesButton.Tapped ())
		{
			descriptionTab.SetDisplayed (false);
			upgradesTab.SetDisplayed (true);			
		}
	}
}
