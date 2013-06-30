using UnityEngine;
using System.Collections;

/*
 * INSTRUCTIONS
 * 
 * Adding an upgrade or an item:
 * 
 * 1. Add an entry to Upgrades or Items enum
 * 2. Add UnlockLevel, PriceCoins and PriceGoldDiscs properties
 * 3. Update the UI
 * 
*/

public class PersistentData : MonoBehaviour {
	
	//*************************************************************** Editor parameters and defaults
	
	// Hard currency price (either Coins or cash)
	public int m_GoldDiscsPriceCoins = 10000;
	public float m_GoldDiscsPriceCashGBP = .99f;
	
	// Upgrades price (need both Coins and GDs)
	public int m_JumpsuitRedUnlockLevel = 1;
	public int m_JumpsuitRedPriceCoins = 5000;
	public int m_JumpsuitRedPriceGoldDiscs = 1;
	public int m_JumpsuitSilverUnlockLevel = 10;
	public int m_JumpsuitSilverPriceCoins = 25000;
	public int m_JumpsuitSilverPriceGoldDiscs = 5;
	public int m_JumpsuitGoldUnlockLevel = 20;
	public int m_JumpsuitGoldPriceCoins = 150000;
	public int m_JumpsuitGoldPriceGoldDiscs = 20;

	// Items price (need both XP and GDs)
	public int m_BlueSuedeShoesUnlockLevel = 5;
	public int m_BlueSuedeShoesPriceCoins = 10000;
	public int m_BlueSuedeShoesPriceGoldDiscs = 0;
	public int m_SpeedwayHelmetUnlockLevel = 12;
	public int m_SpeedwayHelmetPriceCoins = 40000;
	public int m_SpeedwayHelmetPriceGoldDiscs = 1;
	public int m_ArmyGunUnlockLevel = 16;
	public int m_ArmyGunPriceCoins = 100000;
	public int m_ArmyGunPriceGoldDiscs = 5;
	
	// Level-up cost (20pc increase rounded)
	public int m_LevelUpXP2 = 500;
	public int m_LevelUpXP3 = 600;
	public int m_LevelUpXP4 = 700;
	public int m_LevelUpXP5 = 900;
	public int m_LevelUpXP6 = 1000;
	public int m_LevelUpXP7 = 1200;
	public int m_LevelUpXP8 = 1500;
	public int m_LevelUpXP9 = 1800;
	public int m_LevelUpXP10 = 2100;
	public int m_LevelUpXP11 = 2600;
	public int m_LevelUpXP12 = 3100;
	public int m_LevelUpXP13 = 3700;
	public int m_LevelUpXP14 = 4500;
	public int m_LevelUpXP15 = 5300;
	public int m_LevelUpXP16 = 6400;
	public int m_LevelUpXP17 = 7700;
	public int m_LevelUpXP18 = 9200;
	public int m_LevelUpXP19 = 11100;
	public int m_LevelUpXP20 = 13300;
	static int maxLevels = 20;
	static private int [] levelUpXP;
	
	// This is a singleton
	public static PersistentData instance {get; private set;}
		
	//*************************************************************** The persisent data
	static public int hiScore
	{
		get {return SecuredPlayerPrefs.GetInt ("hiScore", 0);}
		set {SecuredPlayerPrefs.SetInt ("hiScore", value);}
	}
	
	//*************************************************************** Currency
	static public int coins	// Soft currency
	{
		get {return SecuredPlayerPrefs.GetInt ("coins", 0);}
		set {SecuredPlayerPrefs.SetInt ("coins", value);}
	}
	
	static public int goldDiscs	// Hard currency
	{
		get {return SecuredPlayerPrefs.GetInt ("goldDiscs", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("goldDiscs", value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	static public float cashSpentGBP
	{
		get {return SecuredPlayerPrefs.GetFloat ("cashSpentGBP", 0);}
		set {
			SecuredPlayerPrefs.SetFloat ("cashSpentGBP", value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	//*************************************************************** XP + Level
	static public int xP
	{
		get {return SecuredPlayerPrefs.GetInt ("xp", 0);}
		set {SecuredPlayerPrefs.SetInt ("xp", value);}
	}
	static public int CurrentLevel ()
	{
		int n;
		for (n = 1; n < maxLevels; n++)
		{
			if (xP < levelUpXP[n + 1])
				break;		
		}
		return n;
	}

	//*************************************************************** Items
	public enum Items
	{
		BLUESUEDESHOES	= 1 << 0,
		SPEEDWAYHELMET	= 1 << 1,
		ARMYGUN			= 1 << 2
	};
	static public Items items
	{
		get {return (Items)SecuredPlayerPrefs.GetInt ("items", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("items", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public bool HasItem (Items item) {
		return ((int)items & (int)item) != 0;
	}
	
	//*************************************************************** Upgrades
	public enum Upgrades
	{
		JUMPSUIT_NONE = 0,
		JUMPSUIT_RED,
		JUMPSUIT_SILVER,
		JUMPSUIT_GOLD
	};	
	static public Upgrades upgrades
	{
		get {return (Upgrades)SecuredPlayerPrefs.GetInt ("upgrades", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("upgrades", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	//*************************************************************** Public API
	
	
	//*************************************************************** Internal methods
		
	// Load the SecuredPlayerPrefs
	void Awake ()
	{
		// Singleton
		if (instance == null) {
			instance = FindObjectOfType (typeof(PersistentData)) as PersistentData;
			if (instance == null)
				instance = this;
			DontDestroyOnLoad (gameObject);	// Maintain the non-statics too
		}
		
		// Initialise the encryption (don't change the key!)
		SecuredPlayerPrefs.SetSecretKey("kqcgLEJFUxUSFkWhUrjC6");
		//Debug.Log (Application. persistentDataPath); Note the savefile stored here
			
		// Set up the array equivalent
		levelUpXP = new int [maxLevels+2];
		levelUpXP[0] = levelUpXP[1] = 0;
		levelUpXP[2] = m_LevelUpXP2;
		levelUpXP[3] = m_LevelUpXP3;
		levelUpXP[4] = m_LevelUpXP4;
		levelUpXP[5] = m_LevelUpXP5;
		levelUpXP[6] = m_LevelUpXP6;
		levelUpXP[7] = m_LevelUpXP7;
		levelUpXP[8] = m_LevelUpXP8;
		levelUpXP[9] = m_LevelUpXP9;
		levelUpXP[10] = m_LevelUpXP10;
		levelUpXP[11] = m_LevelUpXP11;
		levelUpXP[12] = m_LevelUpXP12;
		levelUpXP[13] = m_LevelUpXP13;
		levelUpXP[14] = m_LevelUpXP14;
		levelUpXP[15] = m_LevelUpXP15;
		levelUpXP[16] = m_LevelUpXP16;
		levelUpXP[17] = m_LevelUpXP17;
		levelUpXP[18] = m_LevelUpXP18;
		levelUpXP[19] = m_LevelUpXP19;
		levelUpXP[20] = m_LevelUpXP20;
		
		// Test
		Debug.Log (SecuredPlayerPrefs.ToPrettyString());
	}
	
	void Update ()
	{
	}
	
	static public void ResetAll ()
	{
		SecuredPlayerPrefs.DeleteAll ();
	}
	
	// A Save wrapper to handle exceptions
	static void Save ()
	{
		try {
			SecuredPlayerPrefs.Save ();	// Save here to ensure not lost if crash
		}
		catch (PlayerPrefsException e) {
			Debug.LogException (e);
		}
	}
	
	// Needed to save when Application.LoadLevel called	
	public void OnDestroy ()
	{
		Save ();
	}
}
