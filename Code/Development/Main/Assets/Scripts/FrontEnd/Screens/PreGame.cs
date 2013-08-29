using UnityEngine;
using System.Collections;

public class PreGame : MonoBehaviour {
	
#pragma warning disable 414
	
	//persistent
	//FastGUIElement General_Background;
	FastGUIElement General_BackButton;
	FastGUIElement General_CoinDisplay;
	
	//stuff
	FastGUIElement PreGame_Play;
	
	FastGUIElement PreGame_Shades_No;
	FastGUIElement PreGame_Shades_Upgrade;
	FastGUIElement PreGame_Shades_Use;
	FastGUIElement PreGame_Shades_Desc;
	FastGUIElement PreGame_Shades_Icon;
	
	FastGUIElement PreGame_Buckle_No;
	FastGUIElement PreGame_Buckle_Upgrade;
	FastGUIElement PreGame_Buckle_Use;
	FastGUIElement PreGame_Buckle_Desc;
	FastGUIElement PreGame_Buckle_Icon;
	
	FastGUIElement PreGame_Shoes_No;
	FastGUIElement PreGame_Shoes_Upgrade;
	FastGUIElement PreGame_Shoes_Use;
	FastGUIElement PreGame_Shoes_Desc;
	FastGUIElement PreGame_Shoes_Icon;
	
	
	// Use this for initialization
	void Start () {
		
		FrontEnd.instance.SelectAtlas ("PreGame_Atlas");
	
		FastGUIElement.uvxmlFile = "PreGame_AtlasUV";
		
		//persistent
		/*General_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom ("General_Background.png"));
		*/
		
		General_BackButton = new FastGUIElement (
			new Vector2 (0,0),
			FastGUIElement.UVsFrom ("General_BackButton.png"));
		
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536,0),
			FastGUIElement.UVsFrom ("General_CoinDisplay.png"));
		
		/* Duplicate of the one above
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536,0),
			FastGUIElement.UVsFrom ("General_CoinDisplay.png"));
		*/
		
		//stuff
		PreGame_Play = new FastGUIElement (
			new Vector2 (448, 256),
			FastGUIElement.UVsFrom ("PreGame_Play.png"));
		
		// This one doesn't work
		PreGame_Shades_No = new FastGUIElement (
			new Vector2 (448, 256),
			FastGUIElement.UVsFrom ("PreGame_Shades_No.png"));
		//to add.
		
		//
		/*
		PostGame_XPFill = new FastGUIElement (
			new Vector2 (970, 906),
			FastGUIElement.UVsFrom ("PostGame_XPFill.png"));		
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
