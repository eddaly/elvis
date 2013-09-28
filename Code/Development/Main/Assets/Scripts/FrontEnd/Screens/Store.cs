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
	
	// The Coins/GD/Level display
	private GameObject myGUITextObject;
	
	// Use this for initialization
	void Start ()
	{	
		FrontEnd.instance.SelectAtlas ("Frontend_Atlas");

		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = "Frontend_AtlasUV";
		
		// Persistent across tabs
		Store_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom ("Store_Background.png"));		
		General_BackButton = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom ("General_BackButton.png"));
		General_CoinDisplay = new FastGUIElement (
			new Vector2 (1536, 0),
			FastGUIElement.UVsFrom ("General_CoinDisplay.png"));
		
		// Create tabs
		wardrobe = new StoreWardrobe (this);
		gear = new StoreGear (this);
		bank = new StoreBank ();
		
		// Create the GUIText
		myGUITextObject = Instantiate (Resources.Load ("Frontend_GUIText")) as GameObject;
		myGUITextObject.guiText.anchor = TextAnchor.MiddleCenter;
		myGUITextObject.guiText.pixelOffset = new Vector2 (General_CoinDisplay.screenRect.x + General_CoinDisplay.screenRect.width/2, 
			General_CoinDisplay.screenRect.y + General_CoinDisplay.screenRect.height/2);
		myGUITextObject.guiText.transform.position = Vector3.zero;
		myGUITextObject.guiText.text = "Coins\nGDs\nXP Level";
		myGUITextObject.guiText.color = Color.red;
	}

	// Update is called once per frame
	void Update ()
	{			
		// Back
		if (General_BackButton.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Back");
			Application.LoadLevel ("Startup");	//Test
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
			SelectBank ();
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
		
		// Update text (preventing freak scores exceed alloted space)
		if (PersistentData.coins <= 999999)
			myGUITextObject.guiText.text =	"Coins: " + PersistentData.coins;
		else
			myGUITextObject.guiText.text = "Coins: 999999+";
		if (PersistentData.goldDiscs <= 999999)
			myGUITextObject.guiText.text +=	"\nGDs: " + PersistentData.goldDiscs;
		else
			myGUITextObject.guiText.text += "\nGDs: 999999+";
		myGUITextObject.guiText.text += "\nXP Level: " + PersistentData.CurrentLevel ();
	}
	
	// Select the Bank screen (can be called by other tabs if need more currency)
	public void SelectBank ()
	{
		// Select Bank tab
		bank.SetSelect (true);
		wardrobe.SetSelect (false);
		gear.SetSelect (false);
		bankSelected = true;
		gearSelected = wardrobeSelected = false;		
	}
		
	// Debug GUI stuff
	void OnGUI ()
	{
		if (Debug.isDebugBuild && Application.isEditor)
		{
			GUIStyle guistyle = new GUIStyle (GUI.skin.GetStyle("button"));
			guistyle.alignment = TextAnchor.MiddleLeft;
			bool update = false;
			if (GUI.Button (new Rect (0, 50, 150, 20), "XP = " + PersistentData.xP + " .MORE?", guistyle)) {
				PersistentData.xP += 100;
				update = true;
			}
			if (GUI.Button (new Rect (0, 70, 150, 20), "COINS = " + PersistentData.coins + " .MORE?", guistyle)) {
				PersistentData.coins += 1000;
				update = true;
			}
			if (GUI.Button (new Rect (0, 90, 150, 20), "GDs = " + PersistentData.goldDiscs + " .MORE?", guistyle)) {
				PersistentData.goldDiscs += 1;
				update = true;
			}
			if (GUI.Button (new Rect (0, 110, 150, 20), "RESET SAVE DATA?", guistyle)) {
				PersistentData.ResetAll();
				update = true;
			}
			if (update) {
				if (wardrobeSelected)
					wardrobe.UpdateWardrobeStatus ();
				if (gearSelected)
					gear.UpdateGearStatus ();
			}		
		}
		if (bankSelected)
			bank.OnGUI();
	}

}
