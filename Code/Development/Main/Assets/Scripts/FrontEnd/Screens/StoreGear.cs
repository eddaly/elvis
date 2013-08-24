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
	
	// The counters
#pragma warning disable 414	
	GameObject shadesCounter_go, beltBuckleCounter_go, shoesCounter_go, epCounter_go, rpmCounter_go;
	FontNumber shadesCounter, beltBuckleCounter, shoesCounter, epCounter, rpmCounter;
#pragma warning restore 414

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
		
		// Set up the counters (trial and errored rather than offset from element for now, at least)
		int x = -600, xStep = 384, y = 175, z = 0;
		SetupFontNumber (out epCounter, out epCounter_go, new Vector3 (x + xStep * 0, y, z));
		SetupFontNumber (out beltBuckleCounter, out beltBuckleCounter_go, new Vector3 (x + xStep * 1, y, z));
		SetupFontNumber (out rpmCounter, out rpmCounter_go, new Vector3 (x + xStep * 2, y, z));
		SetupFontNumber (out shoesCounter, out shoesCounter_go, new Vector3 (x + xStep * 3, y, z));
		SetupFontNumber (out shadesCounter, out shadesCounter_go, new Vector3 (x + xStep * 4, y, z));
	}
	
	//***check when called
	~StoreGear ()
	{
		epCounter.Release ();
		beltBuckleCounter.Release ();
		rpmCounter.Release ();
		shoesCounter.Release ();
		shadesCounter.Release();
	}
	
	private void SetupFontNumber (out FontNumber fontNumber, out GameObject go, Vector3 position)
	{
		go = new GameObject ();
		fontNumber = go.AddComponent<FontNumber>();		
		go.transform.position = position; 
		fontNumber.m_Number = 0;
		fontNumber.m_Multiplier = true;
		fontNumber.m_DisplayLeadingDigits = false;
		fontNumber.m_NumDigits = 5;
		fontNumber.m_Anchor = 2;
		fontNumber.m_ScaleX = 100.0f;
		fontNumber.m_ScaleY = 100.0f;
		fontNumber.m_Spacing = 64.0f;
		fontNumber.m_Colour = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		fontNumber.InitialiseDigits();
		fontNumber.Hide ();
		go.SetActive (false);	// Need to disable object or Hide() doesn't work correctly
	}
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_GearScreen.SetDisplayed (!isSelected);
		Store_GearScreen_InUse.SetDisplayed (isSelected);
		
		epCounter_go.SetActive (isSelected);
		beltBuckleCounter_go.SetActive (isSelected);
		rpmCounter_go.SetActive (isSelected);
		shoesCounter_go.SetActive (isSelected);
		shadesCounter_go.SetActive (isSelected);
		
		if (isSelected) {
			UpdateGearStatus ();
			epCounter.Show ();
			beltBuckleCounter.Show ();
			rpmCounter.Show ();
			shoesCounter.Show ();
			shadesCounter.Show ();
		}
		else {
			epCounter.Hide ();
			beltBuckleCounter.Hide ();
			rpmCounter.Hide ();
			shoesCounter.Hide ();
			shadesCounter.Hide ();
		}
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{
		// Buy 1
		if (Store_GearBuy1.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_01");
			
			// Buy it
			PersistentData.SetConsumableItem (Metagame.ConsumableItems.EP, 
				PersistentData.GetConsumableItem(Metagame.ConsumableItems.EP) + 1);		
			PersistentData.coins -= Metagame.ep.priceCoins;
			PersistentData.goldDiscs -= Metagame.ep.priceGoldDiscs;

			UpdateGearStatus ();
		}
		// Buy 2
		else if (Store_GearBuy2.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_02");
			
			// Buy it
			PersistentData.SetConsumableItem (Metagame.ConsumableItems.BELTBUCKLE, 
				PersistentData.GetConsumableItem(Metagame.ConsumableItems.BELTBUCKLE) + 1);		
			PersistentData.coins -= Metagame.beltBuckle.priceCoins;
			PersistentData.goldDiscs -= Metagame.beltBuckle.priceGoldDiscs;

			UpdateGearStatus ();
		}
		//Buy 3
		else if (Store_GearBuy3.Tapped ())
		{
			// Play sound
			RL.m_SoundController.Play("FE_Confirm_03");
			
			// Buy it
			PersistentData.SetConsumableItem (Metagame.ConsumableItems.RPM, 
				PersistentData.GetConsumableItem(Metagame.ConsumableItems.RPM) + 1);		
			PersistentData.coins -= Metagame.rpm.priceCoins;
			PersistentData.goldDiscs -= Metagame.rpm.priceGoldDiscs;

			UpdateGearStatus ();
		}
	
	}

	// Update Gear entries to show what is owned / available
	//*** public for now to allow for testing
	public void UpdateGearStatus ()
	{
		// How many of each do you own?
		epCounter.m_Number = PersistentData.GetConsumableItem (Metagame.ConsumableItems.EP);
		beltBuckleCounter.m_Number = PersistentData.GetConsumableItem (Metagame.ConsumableItems.BELTBUCKLE);
		rpmCounter.m_Number = PersistentData.GetConsumableItem (Metagame.ConsumableItems.RPM);
		shoesCounter.m_Number = PersistentData.GetConsumableItem (Metagame.ConsumableItems.BLUESUEDESHOES);
		shadesCounter.m_Number = PersistentData.GetConsumableItem (Metagame.ConsumableItems.SHADES);
		 
		// Display or hide the buy buttons for gear based on XP lock and affordable status
		Store_GearBuy1.SetDisplayed (IsGearItemAvailable (Metagame.ep));
		Store_GearBuy2.SetDisplayed (IsGearItemAvailable (Metagame.beltBuckle));
		Store_GearBuy3.SetDisplayed (IsGearItemAvailable (Metagame.rpm));
		//Store_GearBuy4.SetDisplayed (IsGearItemAvailable (Metagame.shoes));
		//Store_GearBuy5.SetDisplayed (IsGearItemAvailable (Metagame.shades));		
	}

	// Is gear available to buy based on XP lock and affordable status
	private bool IsGearItemAvailable (Metagame.Item item)
	{	
		// Is it unlocked? can you afford it?
		if (item.unlockLevel > PersistentData.CurrentLevel()) {
			Debug.Log (item.name + " locked till XP level: " + item.unlockLevel + " currently: " + PersistentData.CurrentLevel());
		}
		else {
			if (item.priceCoins > PersistentData.coins ||
				item.priceGoldDiscs > PersistentData.goldDiscs) {
				Debug.Log (item.name + " is unlocked but too expensive, coins: " + item.priceCoins + " GDs: " + item.priceGoldDiscs);
			}
			else {
				Debug.Log (item.name + " unlocked and affordable");
				return true;
			}
		}
		return false;
	}
	
}
