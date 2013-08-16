using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
#pragma warning disable 414
	
	// Persistent elements always on screen
	private FastGUIElement Store_Background;
	private FastGUIElement General_BackButton;
	private FastGUIElement General_CoinDisplay;
	
#pragma warning restore 414

	// The tab panels
	private StoreWardrobe wardrobe;
	private StoreGear gear;
	private StoreBank bank;
	
	// Tab selected flags
	private bool wardrobeSelected = true, gearSelected = false, bankSelected = false;
	
	// Use this for initialization
	void Start ()
	{	
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//Frontend_Atlas.xml";
		
		// Persistent across tabs
		Store_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"Store_Background.png"));		
		General_BackButton = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"General_BackButton.png"));
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536, 0),
			FastGUIElement.UVsFrom (@"General_CoinDisplay.png"));
		
		// Create tabs
		wardrobe = new StoreWardrobe ();
		gear = new StoreGear ();
		bank = new StoreBank ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		// Back
		if (General_BackButton.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Back");			
			Debug.Log ("EXIT");
		}
		
		// Test for change of tab
		if (wardrobe.Store_WardrobeScreen.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Navigation_01");
			
			// Select Wardrobe tab
			wardrobe.SetSelect (true);
			gear.SetSelect (false);
			bank.SetSelect (false);
			wardrobeSelected = true;
			gearSelected = bankSelected = false;
		}
		else if (gear.Store_GearScreen.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Navigation_01");
			
			// Select Gear tab
			gear.SetSelect (true);
			wardrobe.SetSelect (false);
			bank.SetSelect (false);
			gearSelected = true;
			wardrobeSelected = bankSelected = false;
		}
		else if (bank.Store_BankScreen.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Navigation_01");
			
			// Select Bank tab
			bank.SetSelect (true);
			wardrobe.SetSelect (false);
			gear.SetSelect (false);
			bankSelected = true;
			gearSelected = wardrobeSelected = false;
		}

		// Update selected tab
		if (wardrobeSelected)
		{
			wardrobe.UpdateTab ();
		}
		else if (gearSelected)
		{
			gear.UpdateTab ();
		}
		else if (bankSelected)
		{
			bank.UpdateTab ();
		}
	}
}
