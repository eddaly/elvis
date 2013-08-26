using UnityEngine;
using System.Collections;

public class Record: MonoBehaviour {
	
#pragma warning disable 414


    //<Store_UpgradeEquipButton
    //<sprite n="Store_UpgradeInUseButton */
	
	//persistent
	FastGUIElement General_Background; //- should this be on this screen?
	FastGUIElement General_BackButton;
	FastGUIElement General_CoinDisplay;
			
	//body
	FastGUIElement Record_Base;
	FastGUIElement Record_Record;
	FastGUIElement Record_RightPane;
	FastGUIElement Record_Stop;
	FastGUIElement Record_Tokens;
	FastGUIElement Record_Sell;
	FastGUIElement Record_Counts;
	FastGUIElement Record_Stop_Tap;
	FastGUIElement Record_Sell_Tap;
	
#pragma warning restore 414

	// Use this for initialization
	void Start () 
	{
			
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = "Record_AtlasUV";

		/*		
		FAST GUI BUTTON example
		Store_WardrobeButton = new FastGUIButton (
 			new Vector2 (192, 0),
 			FastGUIElement.UVsFrom ("Store_WardrobeButtonUp.png"),
 			FastGUIElement.UVsFrom ("Store_WardrobeButtonDown.png"));
 
  		Store_WardrobeButton.SetDisplayed (false);
  		*/
		
		//persistent
		General_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom ("General_Background.png"));
		
		General_BackButton = new FastGUIElement (
			new Vector2 (0,0),
			FastGUIElement.UVsFrom ("General_BackButton.png"));
		
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536,0),
			FastGUIElement.UVsFrom ("General_CoinDisplay.png"));
		
		//body
		
		Record_Base = new FastGUIElement (
			new Vector2 (64, 320),
			FastGUIElement.UVsFrom ("Record_Base.png"));
		Record_Base.SetDisplayed (true);
		
		Record_Record = new FastGUIElement (
			new Vector2 (128, 384),
			FastGUIElement.UVsFrom ("Record_Record.png"));
		Record_Record.SetDisplayed (true);
		
		Record_Stop = new FastGUIElement (
			new Vector2 (1024, 1152),
			FastGUIElement.UVsFrom ("Record_Stop.png"));
		Record_Stop.SetDisplayed (true);
		
		Record_RightPane = new FastGUIElement (
			new Vector2 (1472, 320),
			FastGUIElement.UVsFrom ("Record_RightPane.png"));
		Record_RightPane.SetDisplayed (true);
		
		Record_Tokens = new FastGUIElement (
			new Vector2 (1536, 384),
			FastGUIElement.UVsFrom ("Record_Tokens.png"));
		Record_Tokens.SetDisplayed (true);
		
		Record_Sell = new FastGUIElement (
			new Vector2 (1536, 768),
			FastGUIElement.UVsFrom ("Record_Sell.png"));
		Record_Sell.SetDisplayed (true);
		
		Record_Counts = new FastGUIElement (
			new Vector2 (1536, 1024),
			FastGUIElement.UVsFrom ("Record_Counts.png"));
		Record_Counts.SetDisplayed (true);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		/*if (backButton.UpdateTestPressed ())
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
		}*/
		
		if (General_BackButton.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Back");
			
			Debug.Log ("EXIT");
		}	

	}
}
