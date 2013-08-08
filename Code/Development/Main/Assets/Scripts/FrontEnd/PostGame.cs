using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	
#pragma warning disable 414
	
	//persistent
	FastGUIElement General_Background;
	FastGUIElement General_BackButton;
	FastGUIElement General_CoinDisplay;
	
	//stuff
	FastGUIElement PostGame_CoinsCollected;
	FastGUIElement PostGame_Enemies;
	FastGUIElement PostGame_Continue;
	FastGUIElement PostGame_XPBar;
	FastGUIElement PostGame_XPFill;
	FastGUIElement PostGame_Twitter;
	FastGUIElement PostGame_FB;
	FastGUIElement PostGame_Distance;
	FastGUIElement PostGame_Level;
	
	// Use this for initialization
	void Start () {
	
		FastGUIElement.uvxmlFile = @"assets//resources//PostGame_Atlas.xml";
		
		//persistent
		General_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"General_Background.png"));
		
		General_BackButton = new FastGUIElement (
			new Vector2 (0,0),
			FastGUIElement.UVsFrom (@"General_BackButton.png"));
		
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536,0),
			FastGUIElement.UVsFrom (@"General_CoinDisplay.png"));
		
		//stuff
		PostGame_Distance = new FastGUIElement (
			new Vector2 (448, 256),
			FastGUIElement.UVsFrom (@"PostGame_Distance.png"));
		
		PostGame_Enemies = new FastGUIElement (
			new Vector2 (192, 896),
			FastGUIElement.UVsFrom (@"PostGame_Enemies.png"));	

		PostGame_CoinsCollected = new FastGUIElement (
			new Vector2 (192, 1024),
			FastGUIElement.UVsFrom (@"PostGame_CoinsCollected.png"));
		
		PostGame_Level = new FastGUIElement (
			new Vector2 (704, 896),
			FastGUIElement.UVsFrom (@"PostGame_Level.png"));
		
		PostGame_XPBar = new FastGUIElement (
			new Vector2 (960, 896),
			FastGUIElement.UVsFrom (@"PostGame_XPBar.png"));	
		
		PostGame_FB = new FastGUIElement (
			new Vector2 (192, 1216),
			FastGUIElement.UVsFrom (@"PostGame_FB.png"));
		
		PostGame_Twitter = new FastGUIElement (
			new Vector2 (448, 1216),
			FastGUIElement.UVsFrom (@"PostGame_Twitter.png"));
		
		PostGame_Continue = new FastGUIElement (
			new Vector2 (768, 1216),
			FastGUIElement.UVsFrom (@"PostGame_Continue.png"));
		
		//
		
		PostGame_XPFill = new FastGUIElement (
			new Vector2 (970, 906),
			FastGUIElement.UVsFrom (@"PostGame_XPFill.png"));		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
