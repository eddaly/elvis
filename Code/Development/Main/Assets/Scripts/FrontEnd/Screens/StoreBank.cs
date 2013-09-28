using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

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
	FastGUIButton Store_BuyItem2;
	FastGUIButton Store_BuyItem3;
	FastGUIButton Store_BuyItem4;
	FastGUIElement Store_CoinsDesc1;
	FastGUIElement Store_CoinsDesc2;
	FastGUIElement Store_CoinsDesc3;
	FastGUIElement Store_CoinsDesc4;
	FastGUIElement Store_CoinsPrice1;
	FastGUIElement Store_CoinsPrice2;
	FastGUIElement Store_CoinsPrice3;
	FastGUIElement Store_CoinsPrice4;

	private static GUISkin fontGUISkin;
	
	private const string SKPID_1GD = "com.echopeak.elvis.1gd";
	private const string SKPID_5GD = "com.echopeak.elvis.5gd";
	private const string SKPID_1kC = "com.echopeak.elvis.1kc";
	
#pragma warning disable 414
	MyStoreKitEventListener storeKitEventListener;	// This listens for StoreKit events
#pragma warning restore 414

#if UNITY_IPHONE
	private List<StoreKitProduct> _products;		// A list of StoreKitProducts available to buy
#endif
	
	// Constructor, create elements
	public StoreBank ()
	{
		Store_BankScreen = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom ("Store_BankScreen.png"));
		
		Store_BankScreen_InUse = new FastGUIElement (
			new Vector2 (1088, 0),
			FastGUIElement.UVsFrom ("Store_BankScreen_InUse.png"));
		
		Store_BankIcon1 = new FastGUIElement (
			new Vector2 (64, 256), //update
			FastGUIElement.UVsFrom ("Store_Bank_Icon1.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon1);
		
		Store_CoinsDesc1 = new FastGUIElement (
			new Vector2 (320, 256), //update
			FastGUIElement.UVsFrom ("Store_CoinsDesc1.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc1);
		
		Store_CoinsPrice1 = new FastGUIElement (
			new Vector2 (1152, 256),
			FastGUIElement.UVsFrom ("Store_CoinsPrice1.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice1);
		
		Store_BuyItem1 = new FastGUIButton (
			new Vector2 (1600, 256),
			FastGUIElement.UVsFrom ("Store_BuyItem1.png"),
			FastGUIElement.UVsFrom ("Store_Bank_Icon2.png")); //***REPLACE THIS WITH THE PRESSED BUTTON IMAGE UVs
		Store_BankScreen_InUse.Add (Store_BuyItem1);
	
		Store_BankIcon2 = new FastGUIElement (
			new Vector2 (64, 576), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_Bank_Icon2.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon2);
		
		Store_CoinsDesc2 = new FastGUIElement (
			new Vector2 (320, 576), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_CoinsDesc2.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc2);
		
		Store_CoinsPrice2 = new FastGUIElement (
			new Vector2 (1152, 576),
			FastGUIElement.UVsFrom ("Store_CoinsPrice2.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice2);
		
		Store_BuyItem2 = new FastGUIButton (
			new Vector2 (1600, 576),
			FastGUIElement.UVsFrom ("Store_BuyItem2.png"),
			FastGUIElement.UVsFrom ("Store_Bank_Icon2.png")); //***REPLACE THIS WITH THE PRESSED BUTTON IMAGE UVs
		Store_BankScreen_InUse.Add (Store_BuyItem2);
		
		Store_BankIcon3 = new FastGUIElement (
			new Vector2 (64, 896), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_Bank_Icon3.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon3);
		
		Store_CoinsDesc3 = new FastGUIElement (
			new Vector2 (320, 896), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_CoinsDesc3.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc3);
		
		Store_CoinsPrice3 = new FastGUIElement (
			new Vector2 (1152,896),
			FastGUIElement.UVsFrom ("Store_CoinsPrice3.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice3);
		
		Store_BuyItem3 = new FastGUIButton (
			new Vector2 (1600, 896),
			FastGUIElement.UVsFrom ("Store_BuyItem3.png"),
			FastGUIElement.UVsFrom ("Store_Bank_Icon2.png")); //***REPLACE THIS WITH THE PRESSED BUTTON IMAGE UVs
		Store_BankScreen_InUse.Add (Store_BuyItem3);

		Store_BankIcon4 = new FastGUIElement (
			new Vector2 (64, 1216), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_Bank_Icon4.png"));
		Store_BankScreen_InUse.Add (Store_BankIcon4);
		
		Store_CoinsDesc4 = new FastGUIElement (
			new Vector2 (320, 1216), //update (ED - Fraser's comment left in case relevant)
			FastGUIElement.UVsFrom ("Store_CoinsDesc4.png"));
		Store_BankScreen_InUse.Add (Store_CoinsDesc4);
		
		Store_CoinsPrice4 = new FastGUIElement (
			new Vector2 (1152, 1216),
			FastGUIElement.UVsFrom ("Store_CoinsPrice4.png"));
		Store_BankScreen_InUse.Add (Store_CoinsPrice4);
		
		Store_BuyItem4 = new FastGUIButton (
			new Vector2 (1600, 1216),
			FastGUIElement.UVsFrom ("Store_BuyItem4.png"),
			FastGUIElement.UVsFrom ("Store_Bank_Icon2.png")); //***REPLACE THIS WITH THE PRESSED BUTTON IMAGE UVs
		Store_BankScreen_InUse.Add (Store_BuyItem4);
		
		// Load the GUISkin used for fonts
		fontGUISkin = Resources.Load ("FrontEnd_GUISkin") as GUISkin;
		
		// Don't display Gear screen by default, note this calls SetDisplay() on child elements
		Store_BankScreen_InUse.SetDisplayed (false);
		
		// This listens for events from iOS StoreKit
		storeKitEventListener = new MyStoreKitEventListener ();
		
#if UNITY_IPHONE
		
		// prime31 comment "you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products"
		// ED: so this seems to add another event handler called allProducts which is automatically serialised as a <List> of StoreKitProducts
		// which then evaluates via the lambada operator to this code which logs the message and takes a local copy (i.e. it intercepts the list and takes a copy)
		StoreKitManager.productListReceivedEvent += allProducts => //***what about removing it?
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
		
		// Set up callbacks for purchases
		StoreKitManager.purchaseSuccessfulEvent += PurchaseSuccessful;
		StoreKitManager.purchaseFailedEvent += PurchaseFailed;
		
		// Request the product data
		string[] productIdentifiers = new string[] {SKPID_1GD, SKPID_5GD, SKPID_1kC};
		StoreKitBinding.requestProductData (productIdentifiers);
#endif		
	}
	
/*	~StoreBank()
	{
		//StoreKitManager.productListReceivedEvent -= allProducts;	//***seemed missing from demo?
	}*/
	
	// Select the tab view
	public void SetSelect (bool isSelected)
	{
		Store_BankScreen.SetDisplayed (!isSelected);
		Store_BankScreen_InUse.SetDisplayed (isSelected);
	}
	
	// Update is called once per frame
	public void UpdateTab ()
	{		
		// Update Bank entries to show what is available?
		UpdateBankStatus ();
	
		// Buy something?
		if (Store_BuyItem1.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_01");
			
			// Buy 1 GD for cash
			AttemptBuyProduct (SKPID_1GD);			
		}
		else if (Store_BuyItem2.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_02");
			
			// Buy 5 GDs for cash
			AttemptBuyProduct (SKPID_5GD);			
		}
		else if (Store_BuyItem3.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_03");

			// Buy 1000 Coins for cash
			AttemptBuyProduct (SKPID_1kC);			
		}
		else if (Store_BuyItem4.UpdateTestPressed ())
		{
			RL.m_SoundController.Play("FE_Confirm_01");
			
			// Earn 1 GD for watching an Ad
			//***TBC watch the ad
			PersistentData.goldDiscs++;
			Debug.Log ("Earn 1 GD for watching Ad");			
		}
	}
	
	// Update Bank entries to show what is available?
	private void UpdateBankStatus ()
	{
		// No such designed behaviour currently
	}

#if UNITY_IPHONE

	// Attempt to buy the product, if available
	private void AttemptBuyProduct (string productIdentifier)
	{
		// Check the product is available
		bool foundProduct = false;	
		if (_products != null) {
			foreach (StoreKitProduct product in _products)
			{
				if (product.productIdentifier == productIdentifier)
				{
					Debug.Log ("preparing to purchase product: " + product.productIdentifier);
					
					// Attempt to buy it
					StoreKitBinding.purchaseProduct (product.productIdentifier, 1);
					foundProduct = true;
					break;
				}
			}
		}
		if (!foundProduct)
		{
			Debug.Log ("Couldn't find IAP product " + productIdentifier);
			FrontEnd.DisplayMessage ("SORRY, SOMETHING WENT WRONG WITH YOUR PURCHASE.\nMAY BE A PROBLEM CONTACTING APPLE.\nPLEASE TRY AGAIN LATER.");
		}
	}
	
	// Callback when made a purchase
	private void PurchaseSuccessful (StoreKitTransaction transaction)
	{
		if (transaction.productIdentifier == SKPID_1GD)
		{
			PersistentData.goldDiscs++;
			Debug.Log ("Bought 1 GD for cash");
		}
		else if (transaction.productIdentifier == SKPID_5GD)
		{
			PersistentData.goldDiscs += 5;
			Debug.Log ("Bought 5 GDs for cash");			
		}
		else if (transaction.productIdentifier == SKPID_1kC)
		{
			PersistentData.coins += 1000;
			Debug.Log ("Bought 1000 Coins for cash");			
		}
		FrontEnd.DisplayMessage ("PURCHASE SUCCESSFUL!");
	}
	
	// Callback when made a purchase
	private void PurchaseFailed (string error)	
	{
		FrontEnd.DisplayMessage ("SOMETHING WENT WRONG:\n" + error);
	}
	
#else
	
	// When running in webplayer just buy it immediately
	private void AttemptBuyProduct (string productIdentifier)
	{
		if (productIdentifier == SKPID_1GD)
		{
			PersistentData.goldDiscs++;
			Debug.Log ("Bought 1 GD for cash");
		}
		else if (productIdentifier == SKPID_5GD)
		{
			PersistentData.goldDiscs += 5;
			Debug.Log ("Bought 5 GDs for cash");			
		}
		else if (productIdentifier == SKPID_1kC)
		{
			PersistentData.coins += 1000;
			Debug.Log ("Bought 1000 Coins for cash");			
		}
	}

#endif	
	
	private Rect ScreenToGUI (Rect rect)
	{
		return new Rect (rect.x, Screen.height - rect.y - rect.height, rect.width, rect.height);
	}
		
	// Unity GUI stuff (need fonts)
	public void OnGUI()
	{
		string []price = {"Unknown", "Unknown", "Unknown"};
		string []desc = {"Unknown", "Unknown", "Unknown"};
		
#if UNITY_IPHONE
		if (_products != null) {
			foreach (StoreKitProduct product in _products)
			{
				if (product.productIdentifier == SKPID_1GD)
				{
					price[0] = product.formattedPrice;
					desc[0] = product.title;
				}
				else if (product.productIdentifier == SKPID_5GD)
				{
					price[1] = product.formattedPrice;
					desc[1] = product.title;
				}
				else if (product.productIdentifier == SKPID_1kC)
				{
					price[2] = product.formattedPrice;
					desc[2] = product.title;
				}
			}
		}
#endif
		
		GUI.Label (ScreenToGUI (Store_CoinsDesc1.screenRect), desc[0], fontGUISkin.GetStyle ("Label"));
		GUI.Label (ScreenToGUI (Store_CoinsPrice1.screenRect), price[0], fontGUISkin.GetStyle ("Label")); 
		GUI.Label (ScreenToGUI (Store_CoinsDesc2.screenRect), desc[1], fontGUISkin.GetStyle ("Label"));
		GUI.Label (ScreenToGUI (Store_CoinsPrice2.screenRect), price[1], fontGUISkin.GetStyle ("Label")); 
		GUI.Label (ScreenToGUI (Store_CoinsDesc3.screenRect), desc[2], fontGUISkin.GetStyle ("Label"));
		GUI.Label (ScreenToGUI (Store_CoinsPrice3.screenRect), price[2], fontGUISkin.GetStyle ("Label")); 
		GUI.Label (ScreenToGUI (Store_CoinsPrice4.screenRect), "WATCH AD", fontGUISkin.GetStyle ("Label")); 
	
#if UNITY_IPHONE

		// TEST CODE TO REMOVE
		/*if (Debug.isDebugBuild && !Application.isEditor)
		{ 
			GUILayout.MinHeight (350);
			
			if( GUILayout.Button("Get Can Make Payments" ) )
			{
				bool canMakePayments = StoreKitBinding.canMakePayments();
				Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
			}
			
			
			if( GUILayout.Button( "Get Product Data" ) )
			{
				// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
				string[] productIdentifiers = new string[] {SKPID_1GD, SKPID_5GD, SKPID_1kC};
				StoreKitBinding.requestProductData (productIdentifiers);
			}
			
			
			if( GUILayout.Button( "Restore Completed Transactions" ) )
			{
				StoreKitBinding.restoreCompletedTransactions();
			}
	
		
			//endColumn( true );
			
			
			// enforce the fact that we can't purchase products until we retrieve the product data
			if( _products != null && _products.Count > 0 )
			{
				if( GUILayout.Button( "Purchase Random Product" ) )
				{
					var productIndex = Random.Range( 0, _products.Count );
					var product = _products[productIndex];
					
					Debug.Log( "preparing to purchase product: " + product.productIdentifier );
					StoreKitBinding.purchaseProduct( product.productIdentifier, 1 );
				}
			}
		
			if( GUILayout.Button( "Get Saved Transactions" ) )
			{
				List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
				
				// Print all the transactions to the console
				Debug.Log( "\ntotal transaction received: " + transactionList.Count );
				
				foreach( StoreKitTransaction transaction in transactionList )
					Debug.Log( transaction.ToString() + "\n" );
			}
	
			
			if( GUILayout.Button( "Turn Off Auto Confirmation of Transactions" ) )
			{
				// this is used when you want to validate receipts on your own server or when dealing with iTunes hosted content
				// you must manually call StoreKitBinding.finishPendingTransactions when the download/validation is complete
				StoreKitManager.autoConfirmTransactions = false;
			}
	
	
			if( GUILayout.Button( "Display App Store" ) )
			{
				StoreKitBinding.displayStoreWithProductId( "693265148" );//From iTunes
			}
		}*/
#endif
	}
	
}

/*
 * TODO
 * 	StoreKitBinding.restoreCompletedTransactions();
 * */