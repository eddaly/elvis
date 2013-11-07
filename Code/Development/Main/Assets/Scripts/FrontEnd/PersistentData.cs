using UnityEngine;
using System.Collections;

public class PersistentData : MonoBehaviour {
	
	// This is a singleton
	public static PersistentData instance {get; private set;}
		
	//*************************************************************** The persisent data
	static public int hiScore
	{
		get {return /*Secured*/PlayerPrefs.GetInt ("hiScore", 0);}
		set {/*Secured*/PlayerPrefs.SetInt ("hiScore", value);}
	}
	
	//*************************************************************** Currency
	static public int coins	// Soft currency
	{
		get {return /*Secured*/PlayerPrefs.GetInt ("coins", 0);}
		set {/*Secured*/PlayerPrefs.SetInt ("coins", value);}
	}
	
	static public int goldDiscs	// Hard currency
	{
		get {return /*Secured*/PlayerPrefs.GetInt ("goldDiscs", 0);}
		set {
			/*Secured*/PlayerPrefs.SetInt ("goldDiscs", value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	static public float cashSpentGBP
	{
		get {return /*Secured*/PlayerPrefs.GetFloat ("cashSpentGBP", 0);}
		set {
			/*Secured*/PlayerPrefs.SetFloat ("cashSpentGBP", value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	
	//*************************************************************** XP + Level
	static public int xP
	{
		get {return /*Secured*/PlayerPrefs.GetInt ("xp", 0);}
		set {/*Secured*/PlayerPrefs.SetInt ("xp", value);}
	}
	static public int CurrentLevel ()
	{
		int n;
		for (n = 1; n < Metagame.maxLevels - 1; n++)
		{
			if (xP < Metagame.levelUpXP[n + 1])
				break;		
		}
		return n;
	}

	/*//*************************************************************** Items
	static public Metagame.PermanentItems permanentItems
	{
		get {return (Metagame.PermanentItems)SecuredPlayerPrefsX.GetInt ("permanentItems", 0);}
		set {
			SecuredPlayerPrefsX.SetInt ("permanentItems", (int)value);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	static public bool HasItem (Metagame.PermanentItems item) {
		return ((int)permanentItems & (int)item) != 0;
	}*/
	
	//*************************************************************** Upgrades
	static public Metagame.UpgradeItems upgradeItems
	{
		get {return (Metagame.UpgradeItems)/*Secured*/PlayerPrefs.GetInt ("upgradeItems", (int)Metagame.UpgradeItems.COSTUME1_UPGRADE0);}
		set {
			/*Secured*/PlayerPrefs.SetInt ("upgradeItems", (int)value);
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
		get {return /*Secured*/PlayerPrefs.GetInt ("equippedCostume", 1);}
		set {/*Secured*/PlayerPrefs.SetInt ("equippedCostume", value);}
	}

	//*************************************************************** Consumable Items
	static int [] zeroArray;
	static public int GetConsumableItem (Metagame.ConsumableItems index)
	{
		return /*Secured*/PlayerPrefsX.GetIntArray ("consumableItems", /*zeroArray*/0, (int)Metagame.ConsumableItems.COUNT) [(int)index];
	}
	static public void SetConsumableItem (Metagame.ConsumableItems index, int value)
	{
		int []tempCopy = /*Secured*/PlayerPrefsX.GetIntArray ("consumableItems", /*zeroArray*/0, (int)Metagame.ConsumableItems.COUNT);
		if (tempCopy[(int)index] != value)
		{
			tempCopy[(int)index] = value;
			/*Secured*/PlayerPrefsX.SetIntArray ("consumableItems", tempCopy);
			Save ();	// Save here to ensure not lost if crash
		}
	}
	

	//*************************************************************** Missions
	static public bool IsMissionsComplete (int mission)
	{
		return /*Secured*/PlayerPrefsX.GetIntArray ("missionsComplete", /*zeroArray*/0, Metagame.missions.Length)[mission] == 1;
	}
	static public void SetMissionComplete (int mission, bool isComplete)
	{
		int []tempCopy = /*Secured*/PlayerPrefsX.GetIntArray ("missionsComplete", /*zeroArray*/0, Metagame.missions.Length);
		tempCopy[mission] = isComplete ? 1 : 0;
		/*Secured*/PlayerPrefsX.SetIntArray ("missionsComplete", tempCopy);
		Save ();	// Save here to ensure not lost if crash
	}
	static public bool IsMissionsActive (int mission)
	{
		return /*Secured*/PlayerPrefsX.GetIntArray ("missionsActive", /*zeroArray*/0, Metagame.missions.Length)[mission] == 1;
	}
	static public void SetMissionActive (int mission, bool isActive)
	{
		int []tempCopy = /*Secured*/PlayerPrefsX.GetIntArray ("missionsActive", /*zeroArray*/0, Metagame.missions.Length);
		tempCopy[mission] = isActive ? 1 : 0;
		/*Secured*/PlayerPrefsX.SetIntArray ("missionsActive", tempCopy);
		Save ();	// Save here to ensure not lost if crash
	}
	
	//*************************************************************** Public API
	
	
	//*************************************************************** Internal methods
		
	// Load the /*Secured*/PlayerPrefsX
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
		//SecuredPlayerPrefs.SetSecretKey("kqcgLEJFUxUSFkWhUrjC6");
		//SecuredPlayerPrefs.DeleteAll ();
		//Debug.Log (Application. persistentDataPath); Note the savefile stored here
		
		// Instatiate default array
		int maxArraySize = Mathf.Max (Metagame.missions.Length, (int)Metagame.ConsumableItems.COUNT); 
		zeroArray = new int [maxArraySize];
		for (int n = 0; n < zeroArray.Length; n++)
			zeroArray[n] = 0;
		
		// Test
		//Debug.Log (/*Secured*/PlayerPrefsX.ToPrettyString());
	}
	
	void Update ()
	{
	}
	
	static public void ResetAll ()
	{
		/*Secured*/PlayerPrefs.DeleteAll ();
	}
	
	// A Save wrapper to handle exceptions
	static void Save ()
	{
		try {
			/*Secured*/PlayerPrefs.Save ();	// Save here to ensure not lost if crash
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
