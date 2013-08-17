using UnityEngine;
using System.Collections;

public class Challenges : MonoBehaviour {
	
#pragma warning disable 414


    //<Store_UpgradeEquipButton
    //<sprite n="Store_UpgradeInUseButton */
	
	//persistent
	FastGUIElement General_Background;
	FastGUIElement General_BackButton;
	FastGUIElement General_CoinDisplay;
			
	//body
	FastGUIElement Challenges_Avatar1;
	FastGUIElement Challenges_Avatar2;
	FastGUIElement Challenges_Avatar3;
	FastGUIElement Challenges_Avatar4;
	FastGUIElement Challenges_CreateGame;
	FastGUIElement Challenges_Name1;
	FastGUIElement Challenges_Name2;
	FastGUIElement Challenges_Name3;
	FastGUIElement Challenges_Name4;
	FastGUIElement Challenges_Nudge1;
	FastGUIElement Challenges_Nudge2;
	FastGUIElement Challenges_Nudge3;
	FastGUIElement Challenges_Nudge4;
	FastGUIElement Challenges_Play1;
	FastGUIElement Challenges_Play2;
	FastGUIElement Challenges_Play3;
	FastGUIElement Challenges_Play4;
	FastGUIElement Challenges_Rematch1;
	FastGUIElement Challenges_Rematch2;
	FastGUIElement Challenges_Rematch3;
	FastGUIElement Challenges_Rematch4;
	FastGUIElement Challenges_Stats1;
	FastGUIElement Challenges_Stats2;
	FastGUIElement Challenges_Stats3;
	FastGUIElement Challenges_Stats4;
	FastGUIElement Challenges_Waiting1;
	FastGUIElement Challenges_Waiting2;
	FastGUIElement Challenges_Waiting3;
	FastGUIElement Challenges_Waiting4;
	
#pragma warning restore 414

	// Use this for initialization
	void Start () 
	{
			
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//Challenges_Atlas.xml";

		/*		
		FAST GUI BUTTON example
		Store_WardrobeButton = new FastGUIButton (
 			new Vector2 (192, 0),
 			FastGUIElement.UVsFrom (@"Store_WardrobeButtonUp.png"),
 			FastGUIElement.UVsFrom (@"Store_WardrobeButtonDown.png"));
 
  		Store_WardrobeButton.SetDisplayed (false);
  		*/
		
		//persistent
		General_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"Store_Background.png"));
		
		General_BackButton = new FastGUIElement (
			new Vector2 (0,0),
			FastGUIElement.UVsFrom (@"General_BackButton.png"));
		
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536,0),
			FastGUIElement.UVsFrom (@"General_CoinDisplay.png"));
		
		//body
		
		Challenges_CreateGame = new FastGUIElement (
			new Vector2 (320, 0),
			FastGUIElement.UVsFrom (@"Challenges_CreateGame.png"));
		Challenges_CreateGame.SetDisplayed (true);
		
		Challenges_Avatar1 = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Challenges_Avatar1.png"));
		Challenges_Avatar1.SetDisplayed (true);
		
		Challenges_Avatar2 = new FastGUIElement (
			new Vector2 (64, 576),
			FastGUIElement.UVsFrom (@"Challenges_Avatar2.png"));
		Challenges_Avatar2.SetDisplayed (true);
		
		Challenges_Avatar3 = new FastGUIElement (
			new Vector2 (64, 896),
			FastGUIElement.UVsFrom (@"Challenges_Avatar3.png"));
		Challenges_Avatar3.SetDisplayed (true);
		
		Challenges_Avatar4 = new FastGUIElement (
			new Vector2 (64, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Avatar4.png"));
		Challenges_Avatar4.SetDisplayed (true);
		
		Challenges_Name1 = new FastGUIElement (
			new Vector2 (320, 256),
			FastGUIElement.UVsFrom (@"Challenges_Name1.png"));
		Challenges_Name1.SetDisplayed (true);
		
		Challenges_Name2 = new FastGUIElement (
			new Vector2 (320, 576),
			FastGUIElement.UVsFrom (@"Challenges_Name2.png"));
		Challenges_Name2.SetDisplayed (true);
		
		Challenges_Name3 = new FastGUIElement (
			new Vector2 (320, 896),
			FastGUIElement.UVsFrom (@"Challenges_Name3.png"));
		Challenges_Name3.SetDisplayed (true);
		
		Challenges_Name4 = new FastGUIElement (
			new Vector2 (320, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Name4.png"));
		Challenges_Name4.SetDisplayed (true);
		
		Challenges_Stats1 = new FastGUIElement (
			new Vector2 (320, 384),
			FastGUIElement.UVsFrom (@"Challenges_Stats1.png"));
		Challenges_Stats1.SetDisplayed (true);
		
		Challenges_Stats2 = new FastGUIElement (
			new Vector2 (320, 704),
			FastGUIElement.UVsFrom (@"Challenges_Stats2.png"));
		Challenges_Stats2.SetDisplayed (true);
		
		Challenges_Stats3 = new FastGUIElement (
			new Vector2 (320, 1024),
			FastGUIElement.UVsFrom (@"Challenges_Stats3.png"));
		Challenges_Stats3.SetDisplayed (true);
		
		Challenges_Stats4 = new FastGUIElement (
			new Vector2 (320, 1344),
			FastGUIElement.UVsFrom (@"Challenges_Stats4.png"));
		Challenges_Stats4.SetDisplayed (true);
		
		//buttons/nudge  (nudge always shows just now. no reason other than waiting for the metagame to plug in before this can get conditional)
		Challenges_Nudge1 = new FastGUIElement (
			new Vector2 (1664, 256),
			FastGUIElement.UVsFrom (@"Challenges_Nudge1.png"));
		Challenges_Nudge1.SetDisplayed (true);
		
		Challenges_Nudge2 = new FastGUIElement (
			new Vector2 (1664, 576),
			FastGUIElement.UVsFrom (@"Challenges_Nudge2.png"));
		Challenges_Nudge2.SetDisplayed (true);
		
		Challenges_Nudge3 = new FastGUIElement (
			new Vector2 (1664, 896),
			FastGUIElement.UVsFrom (@"Challenges_Nudge3.png"));
		Challenges_Nudge3.SetDisplayed (true);
		
		Challenges_Nudge4 = new FastGUIElement (
			new Vector2 (1664, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Nudge4.png"));
		Challenges_Nudge4.SetDisplayed (true);
		
		//buttons/play
		Challenges_Play1 = new FastGUIElement (
			new Vector2 (1664, 256),
			FastGUIElement.UVsFrom (@"Challenges_Play1.png"));
		Challenges_Play1.SetDisplayed (false);
		
		Challenges_Play2 = new FastGUIElement (
			new Vector2 (1664, 576),
			FastGUIElement.UVsFrom (@"Challenges_Play2.png"));
		Challenges_Play2.SetDisplayed (false);
		
		Challenges_Play3 = new FastGUIElement (
			new Vector2 (1664, 896),
			FastGUIElement.UVsFrom (@"Challenges_Play3.png"));
		Challenges_Play3.SetDisplayed (false);
		
		Challenges_Play4 = new FastGUIElement (
			new Vector2 (1664, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Play4.png"));
		Challenges_Play4.SetDisplayed (false);
		
		//buttons/waiting
		Challenges_Waiting1 = new FastGUIElement (
			new Vector2 (1664, 256),
			FastGUIElement.UVsFrom (@"Challenges_Waiting1.png"));
		Challenges_Waiting1.SetDisplayed (false);
		
		Challenges_Waiting2 = new FastGUIElement (
			new Vector2 (1664, 576),
			FastGUIElement.UVsFrom (@"Challenges_Waiting2.png"));
		Challenges_Waiting2.SetDisplayed (false);
		
		Challenges_Waiting3 = new FastGUIElement (
			new Vector2 (1664, 896),
			FastGUIElement.UVsFrom (@"Challenges_Waiting3.png"));
		Challenges_Waiting3.SetDisplayed (false);
		
		Challenges_Waiting4 = new FastGUIElement (
			new Vector2 (1664, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Waiting4.png"));
		Challenges_Waiting4.SetDisplayed (false);
		
		//buttons/rematch
		Challenges_Rematch1 = new FastGUIElement (
			new Vector2 (1664, 256),
			FastGUIElement.UVsFrom (@"Challenges_Rematch1.png"));
		Challenges_Rematch1.SetDisplayed (false);
		
		Challenges_Rematch2 = new FastGUIElement (
			new Vector2 (1664, 576),
			FastGUIElement.UVsFrom (@"Challenges_Rematch2.png"));
		Challenges_Rematch2.SetDisplayed (false);
		
		Challenges_Rematch3 = new FastGUIElement (
			new Vector2 (1664, 896),
			FastGUIElement.UVsFrom (@"Challenges_Rematch3.png"));
		Challenges_Rematch3.SetDisplayed (false);
		
		Challenges_Rematch4 = new FastGUIElement (
			new Vector2 (1664, 1216),
			FastGUIElement.UVsFrom (@"Challenges_Rematch4.png"));
		Challenges_Rematch4.SetDisplayed (false);
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
