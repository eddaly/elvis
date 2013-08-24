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

	/*//*************************************************************** Items
	static public Metagame.PermanentItems permanentItems
	{
		get {return (Metagame.PermanentItems)SecuredPlayerPrefs.GetInt ("permanentItems", 0);}
		set {
			SecuredPlayerPrefs.SetInt ("permanentItems", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public bool HasItem (Metagame.PermanentItems item) {
		return ((int)permanentItems & (int)item) != 0;
	}*/
	
	//*************************************************************** Upgrades
	static public Metagame.UpgradeItems upgradeItems
	{
		get {return (Metagame.UpgradeItems)SecuredPlayerPrefs.GetInt ("upgradeItems", (int)Metagame.UpgradeItems.COSTUME1_UPGRADE0);}
		set {
			SecuredPlayerPrefs.SetInt ("upgradeItems", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public bool HasItem (Metagame.UpgradeItems item) {
		return ((int)upgradeItems & (int)item) != 0;
	}
	static public bool IsUpgradeUnlocked (Metagame.Item item) {
		return item.unlockLevel <= CurrentLevel();
	}

	// Currently equippedCostume, 1, 2 or 3
	static public int equippedCostume
	{
		get {return SecuredPlayerPrefs.GetInt ("equippedCostume", 1);}
		set {SecuredPlayerPrefs.SetInt ("equippedCostume", value);}
	}

	//*************************************************************** Consumable Items
	static int [] zeroArray;
	static public int GetConsumableItem (Metagame.ConsumableItems index)
	{
		return SecuredPlayerPrefs.GetIntArray ("consumableItems", zeroArray) [(int)index];
	}
	static public void SetConsumableItem (Metagame.ConsumableItems index, int value)
	{
		int []tempCopy = SecuredPlayerPrefs.GetIntArray ("consumableItems", zeroArray);
		if (tempCopy[(int)index] != value)
		{
			tempCopy[(int)index] = value;
			SecuredPlayerPrefs.SetIntArray ("consumableItems", tempCopy);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	

	//*************************************************************** Missions
	static public bool IsMissionsComplete (int mission)
	{
		return SecuredPlayerPrefs.GetIntArray ("missionsComplete", zeroArray)[mission] == 1;
	}
	static public void SetMissionComplete (int mission, bool isComplete)
	{
		int []tempCopy = SecuredPlayerPrefs.GetIntArray ("missionsComplete", zeroArray);
		tempCopy[mission] = isComplete ? 1 : 0;
		SecuredPlayerPrefs.SetIntArray ("missionsComplete", tempCopy);
		Save ();	// Save here to ensure not lost if crash
	}
	static public bool IsMissionsActive (int mission)
	{
		return SecuredPlayerPrefs.GetIntArray ("missionsActive", zeroArray)[mission] == 1;
	}
	static public void SetMissionActive (int mission, bool isActive)
	{
		int []tempCopy = SecuredPlayerPrefs.GetIntArray ("missionsActive", zeroArray);
		tempCopy[mission] = isActive ? 1 : 0;
		SecuredPlayerPrefs.SetIntArray ("missionsActive", tempCopy);
		Save ();	// Save here to ensure not lost if crash
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
		
		// Instatiate default array
		int maxArraySize = Mathf.Max (Metagame.missions.Length, (int)Metagame.ConsumableItems.COUNT); 
		zeroArray = new int [maxArraySize];
		for (int n = 0; n < zeroArray.Length; n++)
			zeroArray[n] = 0;
		
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
