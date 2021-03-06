using UnityEngine;
using System.Collections;

public class store : MonoBehaviour {
	
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
	
	// The Coins/GD/Level display
	private GUIText aGUIText;
	
	// Use this for initialization
	void Start ()
	{	
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//Frontend_AtlasUV.xml";
		
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
		
		// Create the GUITexts
		//aGUIText = (GUIText)gameObject.AddComponent (typeof (GUIText));
		GameObject go = Instantiate (Resources.Load ("Frontend_GUIText")) as GameObject;  // Note need to clone prefab as can't access pixel correct property from script
		aGUIText = go.guiText;
		aGUIText.transform.position = new Vector3 (.775f, .995f, 0);
		aGUIText.transform.localScale = new Vector3 (.5f, .5f, 1);
		aGUIText.text = "Coins\nGDs\nXP Level";
		aGUIText.color = Color.red;
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
		
		// Update text
		aGUIText.text = "Coins: " + PersistentData.coins + 
			"\nGDs: " + PersistentData.goldDiscs + 
			"\nXP Level: " + PersistentData.CurrentLevel ();
	}
	
#if !UNITY_IPHONE && !UNITY_ANDROID
	void OnGUI ()
	{
		if (Application.isEditor)
		{
			GUIStyle guistyle = new GUIStyle (GUI.skin.GetStyle("button"));
			guistyle.alignment = TextAnchor.MiddleLeft;
			if (GUI.Button (new Rect (0, 50, 150, 20), "XP = " + PersistentData.xP + " .MORE?", guistyle))
				PersistentData.xP += 100;
			if (GUI.Button (new Rect (0, 70, 150, 20), "COINS = " + PersistentData.coins + " .MORE?", guistyle))
				PersistentData.coins += 1000;
			if (GUI.Button (new Rect (0, 90, 150, 20), "GDs = " + PersistentData.goldDiscs + " .MORE?", guistyle))
				PersistentData.goldDiscs += 1;
			if (GUI.Button (new Rect (0, 110, 150, 20), "RESET SAVE DATA?", guistyle))
				PersistentData.ResetAll();
			wardrobe.UpdateWardrobeStatus ();
		}
	}
#endif

}
