using UnityEngine;
using System.Collections;

public class store : MonoBehaviour {
	
#pragma warning disable 414
	FastGUIElement Store_Background;
	/* FastGUIElement General_BackButton;
	FastGUIButton BackButton;
	FastGUIElement Store_Upgrade1Tab;
	FastGUIElement Store_Upgrade2Tab;
	FastGUIElement Store_Upgrade3Tab;
	
	FastGUIElement descriptionTab;
	FastGUIElement upgradesButton;
	FastGUIElement descriptionButton;
	
	General_BackButton
    General_CoinDisplay
    Store_Background
    Store_BankScreen
    Store_BankScreen_InUse
    Store_CostumeDescPane
    Store_CostumePane
    Store_GearScreen
    Store_GearScreen_InUse
    Store_Upgrade1Tab
    Store_Upgrade2Tab
    Store_Upgrade3Tab
   Store_UpgradeBuyButton
    <Store_UpgradeEquipButton
    <sprite n="Store_UpgradeInUseButton */
    FastGUIElement Store_WardrobeScreen;
    FastGUIElement Store_WardrobeScreen_InUse;
	FastGUIElement Store_BankScreen;
    FastGUIElement Store_BankScreen_InUse;
	FastGUIElement Store_GearScreen;
    FastGUIElement Store_GearScreen_InUse;
	FastGUIElement Store_CostumePane;
	FastGUIElement Store_CostumeDescPane;
	FastGUIElement Store_Upgrade1Tab;
	FastGUIElement Store_Upgrade2Tab;
	FastGUIElement Store_Upgrade3Tab;
	FastGUIElement Store_GearEPIcon;
#pragma warning restore 414

	// Use this for initialization
	void Start () {
		
		
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//Elvis_StoreAtlas.xml";
		
		Store_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"Store_Background.png"));
		
		Store_WardrobeScreen = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen.png"));
		Store_WardrobeScreen.SetDisplayed (false);
		
		Store_WardrobeScreen_InUse = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen_InUse.png"));
		
		Store_GearScreen = new FastGUIElement (
			new Vector2 (640, 0),
			FastGUIElement.UVsFrom (@"Store_GearScreen.png"));
		
		Store_GearScreen_InUse = new FastGUIElement (
			new Vector2 (640, 0),
			FastGUIElement.UVsFrom (@"Store_GearScreen_InUse.png"));
		Store_GearScreen_InUse.SetDisplayed (false);
		
		Store_BankScreen = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom (@"Store_BankScreen.png"));
		
		Store_BankScreen_InUse = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom (@"Store_BankScreen_InUse.png"));
		Store_BankScreen_InUse.SetDisplayed (false);
		
		Store_CostumePane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));
		
		Store_CostumeDescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png"));
		
		Store_Upgrade1Tab = new FastGUIElement (
			new Vector2 (64, 896),
			FastGUIElement.UVsFrom (@"Store_Upgrade1Tab.png"));
		
		Store_Upgrade2Tab = new FastGUIElement (
			new Vector2 (64, 896),
			FastGUIElement.UVsFrom (@"Store_Upgrade2Tab.png"));
		Store_Upgrade2Tab.SetDisplayed (false);
		
		Store_Upgrade3Tab = new FastGUIElement (
			new Vector2 (64, 896),
			FastGUIElement.UVsFrom (@"Store_Upgrade3Tab.png"));
		Store_Upgrade3Tab.SetDisplayed (false);
		
		//Gear Screen...
		
		Store_GearEPIcon = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_Upgrade3Tab.png"));
		Store_Upgrade3Tab.SetDisplayed (false);

		//		backGround = new FastGUIElement (
		//	new Vector2 (0, 0),					// Screen position
		//	FastGUIElement.UVsFrom (@"Elvis_FE_Test.psd"));
		

	}
	
	// Update is called once per frame
	void Update () {
		
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
		if (Store_WardrobeScreen.Tapped ())
		{
			Store_WardrobeScreen.SetDisplayed (false);
			Store_WardrobeScreen_InUse.SetDisplayed (true);
			Store_BankScreen.SetDisplayed (true);
			Store_BankScreen_InUse.SetDisplayed (false);
			Store_GearScreen.SetDisplayed (true);
			Store_GearScreen_InUse.SetDisplayed (false);
			Store_CostumePane.SetDisplayed (true);
			Store_CostumeDescPane.SetDisplayed (true);
			Store_Upgrade1Tab.SetDisplayed (true);
		}
		
		if (Store_BankScreen.Tapped ())
		{
			Store_WardrobeScreen.SetDisplayed (true);
			Store_WardrobeScreen_InUse.SetDisplayed (false);
			Store_BankScreen.SetDisplayed (false);
			Store_BankScreen_InUse.SetDisplayed (true);
			Store_GearScreen.SetDisplayed (true);
			Store_GearScreen_InUse.SetDisplayed (false);
			Store_CostumePane.SetDisplayed (false);
			Store_CostumeDescPane.SetDisplayed (false);
			Store_Upgrade1Tab.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (false);
		}
		
		if (Store_GearScreen.Tapped ())
		{
			Store_WardrobeScreen.SetDisplayed (true);
			Store_WardrobeScreen_InUse.SetDisplayed (false);
			Store_BankScreen.SetDisplayed (true);
			Store_BankScreen_InUse.SetDisplayed (false);
			Store_GearScreen.SetDisplayed (false);
			Store_GearScreen_InUse.SetDisplayed (true);
			Store_CostumePane.SetDisplayed (false);
			Store_CostumeDescPane.SetDisplayed (false);
			Store_Upgrade1Tab.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (false);
		}
		
		if (Store_Upgrade1Tab.Tapped ())
		{
			Debug.Log ("Upgrade1 Tapped");
		}
	}
}
