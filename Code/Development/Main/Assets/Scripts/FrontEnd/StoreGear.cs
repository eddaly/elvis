using UnityEngine;
using System.Collections;

public class StoreGear
{
	// Gear tab and child elements
	public FastGUIElement Store_GearScreen;	// Accessible by store to test if selected
    FastGUIElement Store_GearScreen_InUse;
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
	
	// Constructor, create elements
	public StoreGear ()
	{
		Store_GearScreen = new FastGUIElement (
			new Vector2 (640, 0),
			FastGUIElement.UVsFrom (@"Store_GearScreen.png"));
		
		Store_GearScreen_InUse = new FastGUIElement (
			new Vector2 (640, 0),
			FastGUIElement.UVsFrom (@"Store_GearScreen_InUse.png"));

		Store_EPIcon = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_EPIcon.png"));
		Store_GearScreen_InUse.Add (Store_EPIcon);
		
		Store_BuckleIcon = new FastGUIElement (
			new Vector2 (448, 256),
			FastGUIElement.UVsFrom (@"Store_BuckleIcon.png"));
		Store_GearScreen_InUse.Add (Store_BuckleIcon);
		
		Store_RPMIcon = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_RPMIcon.png"));
		Store_GearScreen_InUse.Add (Store_RPMIcon);
		
		Store_ShoesIcon = new FastGUIElement (
			new Vector2 (1216, 256),
			FastGUIElement.UVsFrom (@"Store_ShoesIcon.png"));
		Store_GearScreen_InUse.Add (Store_ShoesIcon);
		
		Store_ShadesIcon = new FastGUIElement (
			new Vector2 (1600, 256),
			FastGUIElement.UVsFrom (@"Store_ShadesIcon.png"));
		Store_GearScreen_InUse.Add (Store_ShadesIcon);
		
		Store_GearIcon1 = new FastGUIElement (
			new Vector2 (64, 704),
			FastGUIElement.UVsFrom (@"Store_GearIcon1.png"));
		Store_GearScreen_InUse.Add (Store_GearIcon1);
		
		Store_GearDesc1 = new FastGUIElement (
			new Vector2 (320, 704),
			FastGUIElement.UVsFrom (@"Store_GearDesc1.png"));
		Store_GearScreen_InUse.Add (Store_GearDesc1);
		
		Store_GearBuy1 = new FastGUIElement (
			new Vector2 (1600, 704),
			FastGUIElement.UVsFrom (@"Store_GearBuy1.png"));
		Store_GearScreen_InUse.Add (Store_GearBuy1);
		
		Store_GearIcon2 = new FastGUIElement (
			new Vector2 (64, 1024),
			FastGUIElement.UVsFrom (@"Store_GearIcon2.png"));
		Store_GearScreen_InUse.Add (Store_GearIcon2);
		
		Store_GearDesc2 = new FastGUIElement (
			new Vector2 (320, 1024),
			FastGUIElement.UVsFrom (@"Store_GearDesc2.png"));
		Store_GearScreen_InUse.Add (Store_GearDesc2);
		
		Store_GearBuy2 = new FastGUIElement (
			new Vector2 (1600, 1024),
			FastGUIElement.UVsFrom (@"Store_GearBuy2.png"));
		Store_GearScreen_InUse.Add (Store_GearBuy2);
		
		Store_GearIcon3 = new FastGUIElement (
			new Vector2 (64, 1344),
			FastGUIElement.UVsFrom (@"Store_GearIcon3.png"));
		Store_GearScreen_InUse.Add (Store_GearIcon3);
		
		Store_GearDesc3 = new FastGUIElement (
			new Vector2 (320, 1344),
			FastGUIElement.UVsFrom (@"Store_GearDesc3.png"));
		Store_GearScreen_InUse.Add (Store_GearDesc3);
		
		Store_GearBuy3 = new FastGUIElement (
			new Vector2 (1600, 1344),
			FastGUIElement.UVsFrom (@"Store_GearBuy3.png"));
		Store_GearScreen_InUse.Add (Store_GearBuy3);
		
		// Don't display Gear screen by default, note this calls SetDisplay() on child elements
		Store_GearScreen_InUse.SetDisplayed (false);	
	}
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_GearScreen.SetDisplayed (!isSelected);
		Store_GearScreen_InUse.SetDisplayed (isSelected);
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{
		// Update Gear entries to show what is owned / available
		UpdateGearDescs ();

		// Buy 1
		if (Store_GearBuy1.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_01");
			Debug.Log ("GEAR TAPPED!");
		}
		// Buy 2
		else if (Store_GearBuy2.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_02");
			
			Debug.Log ("GEAR TAPPED!");
		}
		//Buy 3
		else if (Store_GearBuy3.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_03");
			Debug.Log ("GEAR TAPPED!");
		}
	
	}
	
	// Update Gear entries to show what is owned / available
	private void UpdateGearDescs ()
	{
		//*** Ignored bought unlocked until clarified whether feature exists
		
		// How many of each do you own?

    	//*** Store_EPIcon count = PersistentData.consumableItems [(int) Metagame.ConsumableItems.EP];
		//*** Store_BuckleIcon count = PersistentData.consumableItems [(int) Metagame.ConsumableItems.BELTBUCKLE];
		//*** Store_RPMIcon count = PersistentData.consumableItems [(int) Metagame.ConsumableItems.RPM];
		//*** Store_ShoesIcon count = PersistentData.consumableItems [(int) Metagame.ConsumableItems.BLUESUEDESHOES];
		//*** Store_ShadesIcon count = PersistentData.consumableItems [(int) Metagame.ConsumableItems.SHADES];

		// Is it unlocked, can you afford it?
		if (Metagame.shades.unlockLevel > PersistentData.CurrentLevel()) {
				// Store_GearDesc1 = locked, XP level too low
				// Store_GearBuy1 = no can do
			}
		else {
			if (Metagame.shades.priceCoins > PersistentData.coins ||
				Metagame.shades.priceGoldDiscs > PersistentData.goldDiscs) {
				// Store_GearDesc1 = not enough coins and/or GDs
				// Store_GearBuy1 = no can do
			}
			else {
				// Store_GearDesc1 = unlocked and affordable
				// Store_GearBuy1 = buy it?
			}
		}
		//*** etc for the others but why only 3 choices and 5 types of items?		
	}
	
}

