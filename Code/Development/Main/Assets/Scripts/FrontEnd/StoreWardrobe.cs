using UnityEngine;
using System.Collections;

public class StoreWardrobe
{
	// Wardrobe tab and child elements
	public FastGUIElement Store_WardrobeScreen;	// Accessible by store to test if selected
    FastGUIElement Store_WardrobeScreen_InUse;
	FastGUIElement Store_CostumePane;
	FastGUIElement Store_CostumeDescPane;
	FastGUIElement Store_Upgrade1Tab;
	FastGUIElement Store_Upgrade2Tab;
	FastGUIElement Store_Upgrade3Tab;
	FastGUIElement Store_Upgrade1Desc;
	FastGUIElement Store_Upgrade2Desc;
	FastGUIElement Store_Upgrade3Desc;

	// Constructor, create elements
	public StoreWardrobe ()
	{
		Store_WardrobeScreen = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen.png"));
		Store_WardrobeScreen.SetDisplayed (false);
		
		Store_WardrobeScreen_InUse = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen_InUse.png"));
				
		Store_CostumePane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));
		Store_WardrobeScreen_InUse.Add (Store_CostumePane);
			
		Store_CostumeDescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png"));
		Store_WardrobeScreen_InUse.Add (Store_CostumeDescPane);
		
		Store_Upgrade1Tab = new FastGUIElement (
			new Vector2 (64, 896),
			FastGUIElement.UVsFrom (@"Store_Upgrade1Tab.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade1Tab);
		
		Store_Upgrade1Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade1Desc.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade1Desc);
		
		Store_Upgrade2Tab = new FastGUIElement (
			new Vector2 (64, 1088),
			FastGUIElement.UVsFrom (@"Store_Upgrade2Tab.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade2Tab);
		
		Store_Upgrade2Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade2Desc.png"));	
		Store_WardrobeScreen_InUse.Add (Store_Upgrade2Desc);
		Store_Upgrade2Desc.SetDisplayed (false);
		
		Store_Upgrade3Tab = new FastGUIElement (
			new Vector2 (64, 1280),
			FastGUIElement.UVsFrom (@"Store_Upgrade3Tab.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade3Tab);
		
		Store_Upgrade3Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade3Desc.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade3Desc);
		Store_Upgrade3Desc.SetDisplayed (false);
	}
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_WardrobeScreen.SetDisplayed (!isSelected);
		Store_WardrobeScreen_InUse.SetDisplayed (isSelected);
	
		// By default select Upgrade1
		if (isSelected)	{
			Store_Upgrade1Desc.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Desc.SetDisplayed (false);
		}
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{
		// Update Wardrobe entries to show what is owned / available
		UpdateWardrobeDescs ();			

		// Upgrade 1
		if (Store_Upgrade1Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_01");
			
			Debug.Log ("Upgrade1 Tapped");
			Store_Upgrade1Desc.SetDisplayed (true);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Desc.SetDisplayed (false);
		}
		// Upgrade 2
		else if (Store_Upgrade2Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_02");
			
			Debug.Log ("Upgrade2 Tapped");
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Desc.SetDisplayed (true);
			Store_Upgrade3Desc.SetDisplayed (false);
		}
		// Upgrade 3	
		else if (Store_Upgrade3Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_03");
			
			Debug.Log ("Upgrade3 Tapped");
			Store_Upgrade1Desc.SetDisplayed (false);
			Store_Upgrade2Desc.SetDisplayed (false);
			Store_Upgrade3Desc.SetDisplayed (true);
		}
	
	}
	
	// Update Wardrobe entries to show what is owned / available
	private void UpdateWardrobeDescs ()
	{
		// Do you own it?
		if (PersistentData.HasItem (Metagame.UpgradeItems.COSTUME1_UPGRADE0)) {
			//Store_Upgrade1Desc = owned
		}
		// Else is it unlocked, can you afford it?
		else {
			if (Metagame.costume1_upgrade0.unlockLevel > PersistentData.CurrentLevel()) {
				// Store_Upgrade1Desc = locked, XP level too low
			}
			else {
				if (Metagame.costume1_upgrade0.priceCoins > PersistentData.coins ||
					Metagame.costume1_upgrade0.priceGoldDiscs > PersistentData.goldDiscs) {
					// Store_Upgrade1Desc = not enough coins and/or GDs
				}
				else {
					// Store_Upgrade1Desc = unlocked and affordable, buy it?
				}
			}
				
		}
		if (PersistentData.HasItem (Metagame.UpgradeItems.COSTUME1_UPGRADE1)) {
			//Store_Upgrade2Desc = owned
		}
		// Else is it unlocked, can you afford it?
		else {
			if (Metagame.costume1_upgrade1.unlockLevel > PersistentData.CurrentLevel()) {
				// Store_Upgrade2Desc = locked, XP level too low
			}
			else {
				if (Metagame.costume1_upgrade1.priceCoins > PersistentData.coins ||
					Metagame.costume1_upgrade1.priceGoldDiscs > PersistentData.goldDiscs) {
					// Store_Upgrade2Desc = not enough coins and/or GDs
				}
				else {
					// Store_Upgrade2Desc = unlocked and affordable, buy it?
				}
			}
				
		}
		if (PersistentData.HasItem (Metagame.UpgradeItems.COSTUME1_UPGRADE2)) {
			//Store_Upgrade3Desc = owned
		}
		// Else is it unlocked, can you afford it?
		else {
			if (Metagame.costume1_upgrade2.unlockLevel > PersistentData.CurrentLevel()) {
				// Store_Upgrade3Desc = locked, XP level too low
			}
			else {
				if (Metagame.costume1_upgrade2.priceCoins > PersistentData.coins ||
					Metagame.costume1_upgrade2.priceGoldDiscs > PersistentData.goldDiscs) {
					// Store_Upgrade3Desc = not enough coins and/or GDs
				}
				else {
					// Store_Upgrade3Desc = unlocked and affordable, buy it?
				}
			}		
		}
	}

}

