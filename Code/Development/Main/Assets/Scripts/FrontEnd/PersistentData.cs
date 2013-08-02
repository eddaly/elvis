using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {
	
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
		for (n = 1; n < Metagame.maxLevels; n++)
		{
			if (xP < Metagame.levelUpXP[n + 1])
				break;		
		}
		return n;
	}

	//*************************************************************** Items
	static public Metagame.ItemsSave itemsSave
	{
		get {return (Metagame.ItemsSave)SecuredPlayerPrefs.GetInt ("items", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("items", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public bool HasItem (Metagame.ItemsSave item) {
		return ((int)itemsSave & (int)item) != 0;
	}
	
	//*************************************************************** Upgrades
	static public Metagame.UpgradesSave upgradesSave
	{
		get {return (Metagame.UpgradesSave)SecuredPlayerPrefs.GetInt ("upgrades", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("upgrades", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	//*************************************************************** Missions
	static int [] zeroArray = {};
	static public int [] missionsComplete // 0 or 1, done or not done
	{
		get {return SecuredPlayerPrefs.GetIntArray ("missionsComplete", zeroArray);}
		set {
			SecuredPlayerPrefs.SetIntArray ("missionsComplete", value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public int [] missionsActive // index into Metagame.missions
	{
		get {return SecuredPlayerPrefs.GetIntArray ("missionsActive", zeroArray);}
		set {
			SecuredPlayerPrefs.SetIntArray ("missionsActive", value);
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
