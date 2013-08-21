using UnityEngine;
using System.Collections;

public class StoreBank
{
	// Bank tab and child elements
	public FastGUIElement Store_BankScreen;	// Accessible by store to test if selected
    FastGUIElement Store_BankScreen_InUse;
	FastGUIElement Store_BankIcon1;
	FastGUIElement Store_BankIcon2;
	FastGUIElement Store_BankIcon3;
	FastGUIElement Store_BankIcon4;
	FastGUIButton Store_BuyItem1;
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
	
	// Constructor, create elements
	public StoreBank ()
	{
		Store_BankScreen = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom (@"Store_BankScreen.png"));
		
		Store_BankScreen_InUse = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom (@"Store_BankScreen_InUse.png"));
		
		Store_BankIcon1 = new FastGUIElement (
			new Vector2 (64, 256), //update
			FastGUIElement.UVsFrom (@"Store_Bank_Icon1.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon1);
		
		Store_CoinsDesc1 = new FastGUIElement (
			new Vector2 (320, 256), //update
			FastGUIElement.UVsFrom (@"Store_CoinsDesc1.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc1);
		
		Store_CoinsPrice1 = new FastGUIElement (
			new Vector2 (1152, 256),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice1.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice1);
		
		Store_BuyItem1 = new FastGUIButton (
			new Vector2 (1600, 256),
			FastGUIElement.UVsFrom (@"Store_BuyItem1.png"),
			FastGUIElement.UVsFrom (@"Store_Bank_Icon2.png")); //***REPLACE THIS WITH THE PRESSED BUTTON IMAGE UVs
		Store_BankScreen_InUse.Add (Store_BuyItem1);
	
		Store_BankIcon2 = new FastGUIElement (
			new Vector2 (64, 576), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_Bank_Icon2.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon2);
		
		Store_CoinsDesc2 = new FastGUIElement (
			new Vector2 (320, 576), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_CoinsDesc2.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc2);
		
		Store_CoinsPrice2 = new FastGUIElement (
			new Vector2 (1152, 576),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice2.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice2);
		
		Store_BuyItem2 = new FastGUIElement (
			new Vector2 (1600, 576),
			FastGUIElement.UVsFrom (@"Store_BuyItem2.png"));
		Store_BankScreen_InUse.Add (Store_BuyItem2);
		
		Store_BankIcon3 = new FastGUIElement (
			new Vector2 (64, 896), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_Bank_Icon3.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon3);
		
		Store_CoinsDesc3 = new FastGUIElement (
			new Vector2 (320, 896), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_CoinsDesc3.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc3);
		
		Store_CoinsPrice3 = new FastGUIElement (
			new Vector2 (1152,896),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice3.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice3);
		
		Store_BuyItem3 = new FastGUIElement (
			new Vector2 (1600, 896),
			FastGUIElement.UVsFrom (@"Store_BuyItem3.png"));
		Store_BankScreen_InUse.Add (Store_BuyItem3);

		Store_BankIcon4 = new FastGUIElement (
			new Vector2 (64, 1216), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_Bank_Icon4.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon4);
		
		Store_CoinsDesc4 = new FastGUIElement (
			new Vector2 (320, 1216), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom (@"Store_CoinsDesc4.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc4);
		
		Store_CoinsPrice4 = new FastGUIElement (
			new Vector2 (1152, 1216),
			FastGUIElement.UVsFrom (@"Store_CoinsPrice4.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice4);
		
		Store_BuyItem4 = new FastGUIElement (
			new Vector2 (1600, 1216),
			FastGUIElement.UVsFrom (@"Store_BuyItem4.png"));
		Store_BankScreen_InUse.Add (Store_BuyItem4);
		
		// Don't display Gear screen by default, note this calls SetDisplay() on child elements
		Store_BankScreen_InUse.SetDisplayed (false);	
	}
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_BankScreen.SetDisplayed (!isSelected);
		Store_BankScreen_InUse.SetDisplayed (isSelected);
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{
		// Update Bank entries to show what is owned / available?
		UpdateBankDescs ();
	
		// Buy item 1
		if (Store_BuyItem1.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_01");
			Debug.Log ("BUYITEM1 BUTTON PRESSED!");			
		}
	}
	
	// Update Bank entries to show what is owned / available?
	private void UpdateBankDescs ()
	{
		//*** (not started as unclear on connection with Metagame)
	}

}

