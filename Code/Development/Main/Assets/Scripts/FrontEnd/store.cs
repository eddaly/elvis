using UnityEngine;
using System.Collections;

public class store : MonoBehaviour {
	
#pragma warning disable 414
	//FastGUIElement Store_Background;
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
	
	//wardrobe
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
	FastGUIElement Store_Upgrade1Desc;
	FastGUIElement Store_Upgrade2Desc;
	FastGUIElement Store_Upgrade3Desc;
		
	//gear
	FastGUIElement Store_EPIcon;
	FastGUIElement Store_BuckleIcon;
	FastGUIElement Store_RPMIcon;
	FastGUIElement Store_ShoesIcon;
	FastGUIElement Store_ShadesIcon;
	FastGUIElement Store_GearIcon1;
	FastGUIElement Store_GearIcon2;
	FastGUIElement Store_GearIcon3;
	FastGUIElement Store_GearDesc1;
	FastGUIElement Store_GearDesc2;
	FastGUIElement Store_GearDesc3;
	FastGUIElement Store_GearBuy1;
	FastGUIElement Store_GearBuy2;
	FastGUIElement Store_GearBuy3;	

	//bank
	FastGUIElement Store_BankIcon1;
	FastGUIElement Store_BankIcon2;
	FastGUIElement Store_BankIcon3;
	FastGUIElement Store_BankIcon4;
	FastGUIElement Store_BuyItem1;
	FastGUIElement Store_BuyItem2;
	FastGUIElement Store_BuyItem3;
	FastGUIElement Store_BuyItem4;
	FastGUIElement Store_CoinsDesc1;
	FastGUIElement Store_CoinsDesc2;
	FastGUIElement Store_CoinsDesc3;
	FastGUIElement Store_CoinsDesc4;
	FastGUIElement Store_CoinsPrice1;
	FastGUIElement Store_CoinsPrice2;
	FastGUIElement Store_CoinsPrice3;
	FastGUIElement Store_CoinsPrice4;
	
#pragma warning restore 414

	// Use this for initialization
	void Start () {
		
		
		// The XML file containing the atlas UVs
		FastGUIElement.uvxmlFile = @"assets//resources//Frontend_Atlas.xml";

		
		//		backGround = new FastGUIElement (
		//	new Vector2 (0, 0),					// Screen position
		//	FastGUIElement.UVsFrom (@"Elvis_FE_Test.psd"));
		
		/*
		 	Store_Background = new FastGUIElement (
			new Vector2 (0, 0),
			FastGUIElement.UVsFrom (@"Store_Background.png"));
		*/	
		
		//wardrobe...
		
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
		
		Store_Upgrade1Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade1Desc.png"));
		
		Store_Upgrade2Tab = new FastGUIElement (
			new Vector2 (64, 1088),
			FastGUIElement.UVsFrom (@"Store_Upgrade2Tab.png"));
		
		Store_Upgrade2Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade2Desc.png"));	
		Store_Upgrade2Desc.SetDisplayed (false);
		
		Store_Upgrade3Tab = new FastGUIElement (
			new Vector2 (64, 1280),
			FastGUIElement.UVsFrom (@"Store_Upgrade3Tab.png"));
		
		Store_Upgrade3Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade3Desc.png"));
		Store_Upgrade3Desc.SetDisplayed (false);
		
		//Gear Screen...
		
		Store_EPIcon = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_EPIcon.png"));
		Store_EPIcon.SetDisplayed (false);
		
		Store_BuckleIcon = new FastGUIElement (
			new Vector2 (448, 256),
			FastGUIElement.UVsFrom (@"Store_BuckleIcon.png"));
		Store_BuckleIcon.SetDisplayed (false);
		
		Store_RPMIcon = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_RPMIcon.png"));
		Store_RPMIcon.SetDisplayed (false);
		
		Store_ShoesIcon = new FastGUIElement (
			new Vector2 (1216, 256),
			FastGUIElement.UVsFrom (@"Store_ShoesIcon.png"));
		Store_ShoesIcon.SetDisplayed (false);
		
		Store_ShadesIcon = new FastGUIElement (
			new Vector2 (1600, 256),
			FastGUIElement.UVsFrom (@"Store_ShadesIcon.png"));
		Store_ShadesIcon.SetDisplayed (false);
		
		Store_GearIcon1 = new FastGUIElement (
			new Vector2 (64,704),
			FastGUIElement.UVsFrom (@"Store_GearIcon1.png"));
		Store_GearIcon1.SetDisplayed (false);
		
		Store_GearDesc1 = new FastGUIElement (
			new Vector2 (320,704),
			FastGUIElement.UVsFrom (@"Store_GearDesc1.png"));
		Store_GearDesc1.SetDisplayed (false);
		
		Store_GearBuy1 = new FastGUIElement (
			new Vector2 (1600, 704),
			FastGUIElement.UVsFrom (@"Store_GearBuy1.png"));
		Store_GearBuy1.SetDisplayed (false);
		
		Store_GearIcon2 = new FastGUIElement (
			new Vector2 (64,1024),
			FastGUIElement.UVsFrom (@"Store_GearIcon2.png"));
		Store_GearIcon2.SetDisplayed (false); //roll back
		
		Store_GearDesc2 = new FastGUIElement (
			new Vector2 (320,1024),
			FastGUIElement.UVsFrom (@"Store_GearDesc2.png"));
		Store_GearDesc2.SetDisplayed (false);
		
		Store_GearBuy2 = new FastGUIElement (
			new Vector2 (1600, 1024),
			FastGUIElement.UVsFrom (@"Store_GearBuy2.png"));
		Store_GearBuy2.SetDisplayed (false);
		
		Store_GearIcon3 = new FastGUIElement (
			new Vector2 (64,1344),
			FastGUIElement.UVsFrom (@"Store_GearIcon3.png"));
		Store_GearIcon3.SetDisplayed (false);
		
		Store_GearDesc3 = new FastGUIElement (
			new Vector2 (320,1344),
			FastGUIElement.UVsFrom (@"Store_GearDesc3.png"));
		Store_GearDesc3.SetDisplayed (false);
		
		Store_GearBuy3 = new FastGUIElement (
			new Vector2 (1600, 1344),
			FastGUIElement.UVsFrom (@"Store_GearBuy3.png"));
		Store_GearBuy3.SetDisplayed (false);
		
		//Bank Screen...
		
		Store_BankIcon1 = new FastGUIElement (
			new Vector2 (64, 256), //update
			FastGUIElement.UVsFrom (@"Store_Bank_Icon1.png"));
		Store_BankIcon1.SetDisplayed (false);
		
		Store_CoinsDesc1 = new FastGUIElement (
			new Vector2 (320, 256), //update
			FastGUIElement.UVsFrom (@"Store_CoinsDesc1.png"));
		Store_CoinsDesc1.SetDisplayed (false);
		
		Store_CoinsPrice1 = new FastGUIElement (
			new Vector2 (1152,256),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice1.png"));
		Store_CoinsPrice1.SetDisplayed (false);
		
		Store_BuyItem1 = new FastGUIElement (
			new Vector2 (1600,256),
			FastGUIElement.UVsFrom (@"Store_BuyItem1.png"));
		Store_BuyItem1.SetDisplayed (false);

		Store_BankIcon2 = new FastGUIElement (
			new Vector2 (64, 576), //update
			FastGUIElement.UVsFrom (@"Store_Bank_Icon2.png"));
		Store_BankIcon2.SetDisplayed (false);
		
		Store_CoinsDesc2 = new FastGUIElement (
			new Vector2 (320, 576), //update
			FastGUIElement.UVsFrom (@"Store_CoinsDesc2.png"));
		Store_CoinsDesc2.SetDisplayed (false);
		
		Store_CoinsPrice2 = new FastGUIElement (
			new Vector2 (1152,576),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice2.png"));
		Store_CoinsPrice2.SetDisplayed (false);
		
		Store_BuyItem2 = new FastGUIElement (
			new Vector2 (1600,576),
			FastGUIElement.UVsFrom (@"Store_BuyItem2.png"));
		Store_BuyItem2.SetDisplayed (false);
		
		Store_BankIcon3 = new FastGUIElement (
			new Vector2 (64, 896), //update
			FastGUIElement.UVsFrom (@"Store_Bank_Icon3.png"));
		Store_BankIcon3.SetDisplayed (false);
		
		Store_CoinsDesc3 = new FastGUIElement (
			new Vector2 (320, 896), //update
			FastGUIElement.UVsFrom (@"Store_CoinsDesc3.png"));
		Store_CoinsDesc3.SetDisplayed (false);
		
		Store_CoinsPrice3 = new FastGUIElement (
			new Vector2 (1152,896),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice3.png"));
		Store_CoinsPrice3.SetDisplayed (false);
		
		Store_BuyItem3 = new FastGUIElement (
			new Vector2 (1600,896),
			FastGUIElement.UVsFrom (@"Store_BuyItem3.png"));
		Store_BuyItem3.SetDisplayed (false);

		Store_BankIcon4 = new FastGUIElement (
			new Vector2 (64, 1216), //update
			FastGUIElement.UVsFrom (@"Store_Bank_Icon4.png"));
		Store_BankIcon4.SetDisplayed (false);
		
		Store_CoinsDesc4 = new FastGUIElement (
			new Vector2 (320, 1216), //update
			FastGUIElement.UVsFrom (@"Store_CoinsDesc4.png"));
		Store_CoinsDesc4.SetDisplayed (false);
		
		Store_CoinsPrice4 = new FastGUIElement (
			new Vector2 (1152,1216),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice4.png"));
		Store_CoinsPrice4.SetDisplayed (false);
		
		Store_BuyItem4 = new FastGUIElement (
			new Vector2 (1600,1216),
			FastGUIElement.UVsFrom (@"Store_BuyItem4.png"));
		Store_BuyItem4.SetDisplayed (false);
		
		/* FAST GUI BUTTON
		Store_WardrobeButton = new FastGUIButton (
 			new Vector2 (192, 0),
 			FastGUIElement.UVsFrom (@"Store_WardrobeButtonUp.png"),
 			FastGUIElement.UVsFrom (@"Store_WardrobeButtonDown.png"));
 
  		Store_WardrobeButton.SetDisplayed (false);
  		*/

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
			//wardrobe
			Store_WardrobeScreen.SetDisplayed (false);
			Store_WardrobeScreen_InUse.SetDisplayed (true);
			Store_BankScreen.SetDisplayed (true);
			Store_BankScreen_InUse.SetDisplayed (false);
			Store_GearScreen.SetDisplayed (true);
			Store_GearScreen_InUse.SetDisplayed (false);
			Store_CostumePane.SetDisplayed (true);
			Store_CostumeDescPane.SetDisplayed (true);
			Store_Upgrade1Tab.SetDisplayed (true);
			Store_Upgrade1Desc.SetDisplayed (true);
			Store_Upgrade2Tab.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (false);
			
			/*Store_Upgrade1Tab.SetDisplayed (true);
			Store_Upgrade1Desc.SetDisplayed (true);
			Store_Upgrade2Tab.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (true);
			Store_Upgrade3Tab.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (true);
			*/
			
			//gear
			Store_EPIcon.SetDisplayed (false);
			Store_BuckleIcon.SetDisplayed (false);
			Store_RPMIcon.SetDisplayed (false);
			Store_ShoesIcon.SetDisplayed (false);
			Store_ShadesIcon.SetDisplayed (false);
			
			Store_GearIcon1.SetDisplayed (false);
			Store_GearIcon2.SetDisplayed (false);
			Store_GearIcon3.SetDisplayed (false);
			Store_GearDesc1.SetDisplayed (false);
			Store_GearDesc2.SetDisplayed (false);
			Store_GearDesc3.SetDisplayed (false);
			Store_GearBuy1.SetDisplayed (false);
			Store_GearBuy2.SetDisplayed (false);
			Store_GearBuy3.SetDisplayed (false);
			
			//bank
			Store_BankIcon1.SetDisplayed (false);
			Store_CoinsDesc1.SetDisplayed (false);
			Store_CoinsPrice1.SetDisplayed (false);
			Store_BuyItem1.SetDisplayed (false);
			Store_BankIcon2.SetDisplayed (false);
			Store_CoinsDesc2.SetDisplayed (false);
			Store_CoinsPrice2.SetDisplayed (false);
			Store_BuyItem2.SetDisplayed (false);
			Store_BankIcon3.SetDisplayed (false);
			Store_CoinsDesc3.SetDisplayed (false);
			Store_CoinsPrice3.SetDisplayed (false);
			Store_BuyItem3.SetDisplayed (false);
			Store_BankIcon4.SetDisplayed (false);
			Store_CoinsDesc4.SetDisplayed (false);
			Store_CoinsPrice4.SetDisplayed (false);
			Store_BuyItem4.SetDisplayed (false);
		}
		
		if (Store_Upgrade1Tab.Tapped ())
		{
			Debug.Log ("Upgrade1 Tapped");
			Store_Upgrade1Tab.SetDisplayed (true);
			Store_Upgrade1Desc.SetDisplayed (true);
			Store_Upgrade2Tab.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (false);
		}
			
		if (Store_Upgrade2Tab.Tapped ())
		{
			Debug.Log ("Upgrade2 Tapped");
			Store_Upgrade1Tab.SetDisplayed (true);
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (true);
			Store_Upgrade3Tab.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (false);
		}
			
		if (Store_Upgrade3Tab.Tapped ())
		{
			Debug.Log ("Upgrade3 Tapped");
			Store_Upgrade1Tab.SetDisplayed (true);
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (true);
		}
		
		if (Store_BankScreen.Tapped ())
		{
			//wardrobe
			Store_WardrobeScreen.SetDisplayed (true);
			Store_WardrobeScreen_InUse.SetDisplayed (false);
			Store_BankScreen.SetDisplayed (false);
			Store_BankScreen_InUse.SetDisplayed (true);
			Store_GearScreen.SetDisplayed (true);
			Store_GearScreen_InUse.SetDisplayed (false);
			Store_CostumePane.SetDisplayed (false);
			Store_CostumeDescPane.SetDisplayed (false);
			Store_Upgrade1Tab.SetDisplayed (false);
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (false);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (false);
			Store_Upgrade3Desc.SetDisplayed (false);
			
			//gear
			Store_EPIcon.SetDisplayed (false);
			Store_BuckleIcon.SetDisplayed (false);
			Store_RPMIcon.SetDisplayed (false);
			Store_ShoesIcon.SetDisplayed (false);
			Store_ShadesIcon.SetDisplayed (false);
			Store_GearIcon1.SetDisplayed (false);
			Store_GearIcon2.SetDisplayed (false);
			Store_GearIcon3.SetDisplayed (false);
			Store_GearDesc1.SetDisplayed (false);
			Store_GearDesc2.SetDisplayed (false);
			Store_GearDesc3.SetDisplayed (false);
			Store_GearBuy1.SetDisplayed (false);
			Store_GearBuy2.SetDisplayed (false);
			Store_GearBuy3.SetDisplayed (false);
			
			//bank
			Store_BankIcon1.SetDisplayed (true);
			Store_CoinsDesc1.SetDisplayed (true);
			Store_CoinsPrice1.SetDisplayed (true);
			Store_BuyItem1.SetDisplayed (true);
			Store_BankIcon2.SetDisplayed (true);
			Store_CoinsDesc2.SetDisplayed (true);
			Store_CoinsPrice2.SetDisplayed (true);
			Store_BuyItem2.SetDisplayed (true);
			Store_BankIcon3.SetDisplayed (true);
			Store_CoinsDesc3.SetDisplayed (true);
			Store_CoinsPrice3.SetDisplayed (true);
			Store_BuyItem3.SetDisplayed (true);
			Store_BankIcon4.SetDisplayed (true);
			Store_CoinsDesc4.SetDisplayed (true);
			Store_CoinsPrice4.SetDisplayed (true);
			Store_BuyItem4.SetDisplayed (true);
		}
		
		if (Store_GearScreen.Tapped ())
		{
			//wardrobe
			Store_WardrobeScreen.SetDisplayed (true);
			Store_WardrobeScreen_InUse.SetDisplayed (false);
			Store_BankScreen.SetDisplayed (true);
			Store_BankScreen_InUse.SetDisplayed (false);
			Store_GearScreen.SetDisplayed (false);
			Store_GearScreen_InUse.SetDisplayed (true);
			Store_CostumePane.SetDisplayed (false);
			Store_CostumeDescPane.SetDisplayed (false);
			Store_Upgrade1Tab.SetDisplayed (false);
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Tab.SetDisplayed (false);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Tab.SetDisplayed (false);
			Store_Upgrade3Desc.SetDisplayed (false);
			
			//gear
			Store_EPIcon.SetDisplayed (true);
			Store_BuckleIcon.SetDisplayed (true);
			Store_RPMIcon.SetDisplayed (true);
			Store_ShoesIcon.SetDisplayed (true);
			Store_ShadesIcon.SetDisplayed (true);
			Store_GearIcon1.SetDisplayed (true);
			Store_GearIcon2.SetDisplayed (true);
			Store_GearIcon3.SetDisplayed (true);
			Store_GearDesc1.SetDisplayed (true);
			Store_GearDesc2.SetDisplayed (true);
			Store_GearDesc3.SetDisplayed (true);
			Store_GearBuy1.SetDisplayed (true);
			Store_GearBuy2.SetDisplayed (true);
			Store_GearBuy3.SetDisplayed (true);
			
			//bank
			Store_BankIcon1.SetDisplayed (false);
			Store_CoinsDesc1.SetDisplayed (false);
			Store_CoinsPrice1.SetDisplayed (false);
			Store_BuyItem1.SetDisplayed (false);
			Store_BankIcon2.SetDisplayed (false);
			Store_CoinsDesc2.SetDisplayed (false);
			Store_CoinsPrice2.SetDisplayed (false);
			Store_BuyItem2.SetDisplayed (false);
			Store_BankIcon3.SetDisplayed (false);
			Store_CoinsDesc3.SetDisplayed (false);
			Store_CoinsPrice3.SetDisplayed (false);
			Store_BuyItem3.SetDisplayed (false);
			Store_BankIcon4.SetDisplayed (false);
			Store_CoinsDesc4.SetDisplayed (false);
			Store_CoinsPrice4.SetDisplayed (false);
			Store_BuyItem4.SetDisplayed (false);
		}
		
		if (Store_GearBuy1.Tapped ())
		{
			Debug.Log ("GEAR TAPPED!");
		}
	}
}
