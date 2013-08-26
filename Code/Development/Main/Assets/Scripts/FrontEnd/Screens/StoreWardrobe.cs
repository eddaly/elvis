using UnityEngine;
using System.Collections;

public class StoreWardrobe
{
	// Wardrobe tab and child elements
	public FastGUIElement Store_WardrobeScreen;	// Accessible by store to test if selected
    FastGUIElement Store_WardrobeScreen_InUse;
	FastGUIElement Store_Costume1Pane;
	FastGUIElement Store_Costume1DescPane;
	FastGUIElement Store_Costume2Pane;
	FastGUIElement Store_Costume2DescPane;
	FastGUIElement Store_Costume3Pane;
	FastGUIElement Store_Costume3DescPane;
	FastGUIElement Store_Costume4Pane;
	FastGUIElement Store_Costume4DescPane;
	FastGUIElement Store_CostumePrev;
	FastGUIElement Store_CostumeNext;	
	FastGUIElement Store_Upgrade1Tab;
	FastGUIElement Store_Upgrade2Tab;
	FastGUIElement Store_Upgrade3Tab;
	FastGUIElement Store_Upgrade1Desc;
	FastGUIElement Store_Upgrade2Desc;
	FastGUIElement Store_Upgrade3Desc;
	
	FastGUIButton Store_UpgradeBuyButton;
	FastGUIButton Store_UpgradeEquipButton;
	FastGUIElement Store_UpgradeInUse;
	
	private int costumeSelected = 1;	// 1, 2, 3 or 4
	private int upgradeSelected = 1;	// 1, 2 or 3
	
	private Store store;

	// Constructor, create elements
	public StoreWardrobe (Store aStore)
	{
		store = aStore;
		
		Store_WardrobeScreen = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen.png"));
		
		Store_WardrobeScreen_InUse = new FastGUIElement (
			new Vector2 (192, 0),
			FastGUIElement.UVsFrom (@"Store_WardrobeScreen_InUse.png"));
				
		Store_Costume1Pane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));
		Store_WardrobeScreen_InUse.Add (Store_Costume1Pane);
			
		Store_Costume1DescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png"));
		Store_WardrobeScreen_InUse.Add (Store_Costume1DescPane);
		
		Store_Costume2Pane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));	//*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume2Pane);
			
		Store_Costume2DescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png"));	//*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume2DescPane);
			
		Store_Costume3Pane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));	//*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume3Pane);
			
		Store_Costume3DescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png")); //*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume3DescPane);
		
		Store_Costume4Pane = new FastGUIElement (
			new Vector2 (64, 256),
			FastGUIElement.UVsFrom (@"Store_CostumePane.png"));	//*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume4Pane);
			
		Store_Costume4DescPane = new FastGUIElement (
			new Vector2 (832, 256),
			FastGUIElement.UVsFrom (@"Store_CostumeDescPane.png")); //*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_Costume4DescPane);
		
		Store_CostumePrev = new FastGUIElement (
			new Vector2 (1800, 256),	//*** Needs to be placed correctly
			new Rect (2048, 0, 100, 100)); //*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_CostumePrev);

		Store_CostumeNext = new FastGUIElement (
			new Vector2 (1900, 256),	//*** Needs to be placed correctly
			new Rect (2048, 0, 100, 100)); //*** Need new element and UVs
		Store_WardrobeScreen_InUse.Add (Store_CostumeNext);
		
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
		
		Store_Upgrade3Tab = new FastGUIElement (
			new Vector2 (64, 1280),
			FastGUIElement.UVsFrom (@"Store_Upgrade3Tab.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade3Tab);
		
		Store_Upgrade3Desc = new FastGUIElement (
			new Vector2 (832, 896), 
			FastGUIElement.UVsFrom (@"Store_Upgrade3Desc.png"));
		Store_WardrobeScreen_InUse.Add (Store_Upgrade3Desc);
		
		// The upgrade button/status indicators
		Store_UpgradeBuyButton = new FastGUIButton (
			new Vector2 (1600, 1175),
			FastGUIElement.UVsFrom (@"Store_UpgradeBuyButton.png"),
			FastGUIElement.UVsFrom (@"Store_UpgradeBuyButton.png"));	//*** Updated with pressed button texture);
		Store_WardrobeScreen_InUse.Add (Store_UpgradeBuyButton);

		Store_UpgradeEquipButton = new FastGUIButton (
			new Vector2 (1600, 1175),
			FastGUIElement.UVsFrom (@"Store_UpgradeEquipButton.png"),
			FastGUIElement.UVsFrom (@"Store_UpgradeEquipButton.png"));	//*** Updated with pressed button texture
		Store_WardrobeScreen_InUse.Add (Store_UpgradeEquipButton);
		
		Store_UpgradeInUse = new FastGUIElement (
			new Vector2 (1600, 1175),
			FastGUIElement.UVsFrom (@"Store_UpgradeInUseButton.png"));
		Store_WardrobeScreen_InUse.Add (Store_UpgradeInUse);
		
		// Hide the unselected and display the select costume and upgrade
		SetDisplayForCostumesAndUpgrade ();
		
		// Update the status in advance of the first UpdateTab() message
		UpdateWardrobeStatus ();
	}
	
	// Hide the unselected and display the select costume and upgrade
	private void SetDisplayForCostumesAndUpgrade ()
	{
		// First the costumes...
		Store_Costume1Pane.SetDisplayed (false);
		Store_Costume1DescPane.SetDisplayed (false);
		Store_Costume2Pane.SetDisplayed (false);
		Store_Costume2DescPane.SetDisplayed (false);
		Store_Costume3Pane.SetDisplayed (false);
		Store_Costume3DescPane.SetDisplayed (false);
		Store_Costume4Pane.SetDisplayed (false);
		Store_Costume4DescPane.SetDisplayed (false);
		if (costumeSelected == 1)
		{
			Store_Costume1Pane.SetDisplayed (true);
			Store_Costume1DescPane.SetDisplayed (true);
		}
		else if (costumeSelected == 2)
		{
			Store_Costume2Pane.SetDisplayed (true);
			Store_Costume2DescPane.SetDisplayed (true);
		}
		else if (costumeSelected == 3)
		{
			Store_Costume3Pane.SetDisplayed (true);
			Store_Costume3DescPane.SetDisplayed (true);
		}
		else
		{
			Store_Costume4Pane.SetDisplayed (true);
			Store_Costume4DescPane.SetDisplayed (true);
		}
		
		// Then the upgrades
		Store_Upgrade1Desc.SetDisplayed (false);
		Store_Upgrade2Desc.SetDisplayed (false);
		Store_Upgrade3Desc.SetDisplayed (false);
		if (upgradeSelected == 1)
			Store_Upgrade1Desc.SetDisplayed (true);
		else if (upgradeSelected == 2)
			Store_Upgrade2Desc.SetDisplayed (true);
		else if (upgradeSelected == 3)
			Store_Upgrade3Desc.SetDisplayed (true);		
	}
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_WardrobeScreen.SetDisplayed (!isSelected);
		Store_WardrobeScreen_InUse.SetDisplayed (isSelected);
	
		// Restore selections (as if just went to bank to top-up need to find it easily)
		if (isSelected)
		{
			// Hide the unselected and display the select costume and upgrade
			SetDisplayForCostumesAndUpgrade ();
			
			// Update the status in advance of the UpdateTab() message
			UpdateWardrobeStatus ();
		}
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{
		bool needToUpdateStatus = false;
				
		// Select costume
		int newCostumeSelected = costumeSelected;
		if (Store_CostumeNext.Tapped ())	{
			if (++newCostumeSelected == 5)
				newCostumeSelected = 1;
		}
		else if (Store_CostumePrev.Tapped ())	{
			if (--newCostumeSelected == 0)
				newCostumeSelected = 3;
		}
		if (newCostumeSelected != costumeSelected)
		{
			costumeSelected = newCostumeSelected;	

			// By default select Upgrade1
			upgradeSelected = 1;
						
			needToUpdateStatus = true;
		}
		
		// Select upgrade
		int newUpgradeSelected = upgradeSelected;		
		if (Store_Upgrade1Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_01");
			
			newUpgradeSelected = 1;
		}
		// Upgrade 2
		else if (Store_Upgrade2Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_02");
			
			newUpgradeSelected = 2;
		}
		// Upgrade 3	
		else if (Store_Upgrade3Tab.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_03");
			
			newUpgradeSelected = 3;
		}
		if (newUpgradeSelected != upgradeSelected)
		{			
			upgradeSelected = newUpgradeSelected;			
			needToUpdateStatus = true;
		}
		
		// Attempt to buy the upgrade
		if (Store_UpgradeBuyButton.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_01");

			// Get selected
			Metagame.UpgradeItems costumeUpgrade;
			Metagame.Item metagameUpgrade;
			GetSelectedMetagameCostume (out costumeUpgrade, out metagameUpgrade);

			// If can't afford it
			if (metagameUpgrade.priceCoins > PersistentData.coins || metagameUpgrade.priceGoldDiscs > PersistentData.goldDiscs)
			{
				// Display message and send to bank
				string message = "YOU CAN'T AFFORD IT!\nNEED ";
				if (metagameUpgrade.priceCoins > PersistentData.coins)
					message += (metagameUpgrade.priceCoins - PersistentData.coins) + " MORE COINS";
				if (metagameUpgrade.priceCoins > PersistentData.coins && metagameUpgrade.priceGoldDiscs > PersistentData.goldDiscs)
					message += "\nAND ";
				if (metagameUpgrade.priceGoldDiscs > PersistentData.goldDiscs)
					message += (metagameUpgrade.priceGoldDiscs - PersistentData.goldDiscs) + " MORE GOLD DISC";
				if ((metagameUpgrade.priceGoldDiscs - PersistentData.goldDiscs) > 1)
					message += "S";
				
				FrontEnd.DisplayMessage (message);
				
				store.SelectBank ();	
			}
			else
			{
				Debug.Log ("Bought and equipped the upgrade");
				
				PersistentData.upgradeItems |= costumeUpgrade;
				PersistentData.equippedCostume = costumeSelected;
				
				// Includes any unowned lower upgrades of same costume
				switch (costumeUpgrade)
				{
				case Metagame.UpgradeItems.COSTUME1_UPGRADE2:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME1_UPGRADE1;
					break;
				case Metagame.UpgradeItems.COSTUME2_UPGRADE1:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME2_UPGRADE0;
					break;
				case Metagame.UpgradeItems.COSTUME2_UPGRADE2:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME2_UPGRADE0 & Metagame.UpgradeItems.COSTUME2_UPGRADE1;
					break;
				case Metagame.UpgradeItems.COSTUME3_UPGRADE1:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME3_UPGRADE0;
					break;
				case Metagame.UpgradeItems.COSTUME3_UPGRADE2:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME3_UPGRADE0 & Metagame.UpgradeItems.COSTUME3_UPGRADE1;
					break;
				case Metagame.UpgradeItems.COSTUME4_UPGRADE1:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME4_UPGRADE0;
					break;
				case Metagame.UpgradeItems.COSTUME4_UPGRADE2:
					PersistentData.upgradeItems |= Metagame.UpgradeItems.COSTUME4_UPGRADE0 & Metagame.UpgradeItems.COSTUME4_UPGRADE1;
					break;
				}
				
				PersistentData.coins -= metagameUpgrade.priceCoins;
				PersistentData.goldDiscs -= metagameUpgrade.priceGoldDiscs;
				needToUpdateStatus = true;
			}
		}
		// Equip the costume
		else if (Store_UpgradeEquipButton.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_01");
			Debug.Log ("Equipped the upgrade");
			PersistentData.equippedCostume = costumeSelected;
			needToUpdateStatus = true;
		}
		
		// Update Wardrobe entries to show what is owned / available if neccessary
		if (needToUpdateStatus)
		{
			// Hide the unselected and display the select costume and upgrade
			SetDisplayForCostumesAndUpgrade ();

			UpdateWardrobeStatus ();
		}
	}	
	
	// Update Wardrobe entries to show what is owned / available
	//*** public for now to allow for testing
	public void UpdateWardrobeStatus ()
	{
		// First switch off the buttons etc. by default
		Store_UpgradeBuyButton.SetDisplayed (false);
		Store_UpgradeEquipButton.SetDisplayed (false);
		Store_UpgradeInUse.SetDisplayed (false);
		
		// Update selected costume upgrade status/buttons
		Metagame.UpgradeItems costumeUpgrade;
		Metagame.Item metagameUpgrade;
		GetSelectedMetagameCostume (out costumeUpgrade, out metagameUpgrade);
		
		// Do you own it?
		if (PersistentData.HasItem (costumeUpgrade))
		{
			// Is it equppied?
			if (PersistentData.equippedCostume == costumeSelected) {
				Debug.Log ("Costume " + metagameUpgrade.name + " owned and equipped");
				Store_UpgradeInUse.SetDisplayed (true);
			}
			else {
				Debug.Log ("Costume " + metagameUpgrade.name + " owned but not equipped");
				Store_UpgradeEquipButton.SetDisplayed (true);
			}
		}
		// Else is it unlocked? can you afford it?
		else
		{
			if (metagameUpgrade.unlockLevel > PersistentData.CurrentLevel()) {
				Debug.Log ("Costume " + metagameUpgrade.name + " not owned and locked till XP level: " + metagameUpgrade.unlockLevel + " currently: " + PersistentData.CurrentLevel());
			}
			else {
				if (metagameUpgrade.priceCoins > PersistentData.coins ||
					metagameUpgrade.priceGoldDiscs > PersistentData.goldDiscs) {
					Debug.Log ("Costume " + metagameUpgrade.name + " not owned, is unlocked but too expensive, coins: " + metagameUpgrade.priceCoins + " GDs: " + metagameUpgrade.priceGoldDiscs);
					Store_UpgradeBuyButton.SetDisplayed (true);// Make button available anyway and top-up if needed
				}
				else {
					Debug.Log ("Costume " + metagameUpgrade.name + " not owned, unlocked and affordable");
					Store_UpgradeBuyButton.SetDisplayed (true);
				}
			}
		}
	}
	
	// Get Metagame types from costumeSelected and upgradeSelected
	private void GetSelectedMetagameCostume (out Metagame.UpgradeItems costumeUpgrade, out Metagame.Item metagameUpgrade)
	{	
		if (costumeSelected == 1) {
			if (upgradeSelected == 1) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME1_UPGRADE0;
				metagameUpgrade = Metagame.costume1_upgrade0;
			}
			else if (upgradeSelected == 2) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME1_UPGRADE1;
				metagameUpgrade = Metagame.costume1_upgrade1;
			}
			else {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME1_UPGRADE2;
				metagameUpgrade = Metagame.costume1_upgrade2;
			}
		}
		else if (costumeSelected == 2) {
			if (upgradeSelected == 1) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME2_UPGRADE0;
				metagameUpgrade = Metagame.costume2_upgrade0;
			}
			else if (upgradeSelected == 2) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME2_UPGRADE1;
				metagameUpgrade = Metagame.costume2_upgrade1;
			}
			else {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME2_UPGRADE2;
				metagameUpgrade = Metagame.costume2_upgrade2;
			}
		}
		else if (costumeSelected == 3) {
			if (upgradeSelected == 1) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME3_UPGRADE0;
				metagameUpgrade = Metagame.costume3_upgrade0;
			}
			else if (upgradeSelected == 2) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME3_UPGRADE1;
				metagameUpgrade = Metagame.costume3_upgrade1;
			}
			else {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME3_UPGRADE2;
				metagameUpgrade = Metagame.costume3_upgrade2;
			}
		}
		else {
			if (upgradeSelected == 1) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME4_UPGRADE0;
				metagameUpgrade = Metagame.costume4_upgrade0;
			}
			else if (upgradeSelected == 2) {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME4_UPGRADE1;
				metagameUpgrade = Metagame.costume4_upgrade1;
			}
			else {
				costumeUpgrade = Metagame.UpgradeItems.COSTUME4_UPGRADE2;
				metagameUpgrade = Metagame.costume4_upgrade2;
			}
		}
	}

}

