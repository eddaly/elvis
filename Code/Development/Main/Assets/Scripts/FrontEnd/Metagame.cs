using UnityEngine;
using System.Collections;

public class Metagame
{
	// Used by upgrades, permanent and consumable items, note need BOTH coins and GDs to buy
	public struct Item
	{
		public string name;
		public int unlockLevel;
		public int priceCoins;
		public int priceGoldDiscs;
	};

	// Hard currency price (either Coins or cash)
	public int m_GoldDiscsPriceCoins = 10000;
	public float m_GoldDiscsPriceCashGBP = .99f;
	
	// Level-up cost (20pc increase rounded), note for convienience level[0] is meaningless and level[1] is free
	static public int [] levelUpXP = {-1, 0, 
		500, 600, 700, 900, 1000, 1200, 1500, 1800, 2100, 2600, 3100, 3700, 4500, 5300, 6400, 7700, 9200, 11100, 13300};
	static public int maxLevels = levelUpXP.Length;

	// Upgrades (need both Coins and GDs)b
	static public Item costume1_upgrade0 = 
		new Item {name = "costume1_upgrade0",		unlockLevel = 1,	priceCoins = 5000,		priceGoldDiscs = 1};
	static public Item costume1_upgrade1 = 
		new Item {name = "costume1_upgrade1",		unlockLevel = 10,	priceCoins = 25000,		priceGoldDiscs = 5};
	static public Item costume1_upgrade2 = 
		new Item {name = "costume1_upgrade2", 		unlockLevel = 20,	priceCoins = 150000,	priceGoldDiscs = 20};

	static public Item costume2_upgrade0 = 
		new Item {name = "costume2_upgrade0",		unlockLevel = 1,	priceCoins = 5000,		priceGoldDiscs = 1};
	static public Item costume2_upgrade1 = 
		new Item {name = "costume2_upgrade1",		unlockLevel = 10,	priceCoins = 25000,		priceGoldDiscs = 5};
	static public Item costume2_upgrade2 = 
		new Item {name = "costume2_upgrade2", 		unlockLevel = 20,	priceCoins = 150000,	priceGoldDiscs = 20};
	
	static public Item costume3_upgrade0 = 
		new Item {name = "costume3_upgrade0",		unlockLevel = 1,	priceCoins = 5000,		priceGoldDiscs = 1};
	static public Item costume3_upgrade1 = 
		new Item {name = "costume3_upgrade1",		unlockLevel = 10,	priceCoins = 25000,		priceGoldDiscs = 5};
	static public Item costume3_upgrade2 = 
		new Item {name = "costume3_upgrade2", 		unlockLevel = 20,	priceCoins = 150000,	priceGoldDiscs = 20};

	static public Item costume4_upgrade0 = 
		new Item {name = "costume4_upgrade0",		unlockLevel = 1,	priceCoins = 5000,		priceGoldDiscs = 1};
	static public Item costume4_upgrade1 = 
		new Item {name = "costume4_upgrade1",		unlockLevel = 10,	priceCoins = 25000,		priceGoldDiscs = 5};
	static public Item costume4_upgrade2 = 
		new Item {name = "costume4_upgrade2", 		unlockLevel = 20,	priceCoins = 150000,	priceGoldDiscs = 20};

	// Note, bitfield to test for PersistentData.upgradeItems
	public enum UpgradeItems
	{
		COSTUME1_UPGRADE0 = 1 << 0,
		COSTUME1_UPGRADE1 = 1 << 1,
		COSTUME1_UPGRADE2 = 1 << 2,
		
		COSTUME2_UPGRADE0 = 1 << 3,
		COSTUME2_UPGRADE1 = 1 << 4,
		COSTUME2_UPGRADE2 = 1 << 5,

		COSTUME3_UPGRADE0 = 1 << 6,
		COSTUME3_UPGRADE1 = 1 << 7,
		COSTUME3_UPGRADE2 = 1 << 8,
		
		COSTUME4_UPGRADE0 = 1 << 9,
		COSTUME4_UPGRADE1 = 1 << 10,
		COSTUME4_UPGRADE2 = 1 << 11
	};	
	
	/*// Items (need both Coins and GDs)
	static public Item [] permanentItems = {
		new Item {name = "BlueSuedeShoes",	unlockLevel = 5,	priceCoins = 10000,		priceGoldDiscs = 0},
		new Item {name = "SpeedwayHelmet",	unlockLevel = 12,	priceCoins = 40000,		priceGoldDiscs = 1},
		new Item {name = "ArmyGun", 		unlockLevel = 16,	priceCoins = 100000,	priceGoldDiscs = 5}
	};
	// Note, bitfield to test for PersistentData.permanentItems
	public enum PermanentItems
	{
		BLUESUEDESHOES	= 1 << 0,
		SPEEDWAYHELMET	= 1 << 1,
		ARMYGUN			= 1 << 2
	};*/

	// Consumables Unlocked (need both Coins and GDs)
	static public Item [] consumableUnlocked = {
		new Item {name = "Sunglasses",		unlockLevel = 1,	priceCoins = 0,			priceGoldDiscs = 0},
		new Item {name = "BeltBuckle",		unlockLevel = 1,	priceCoins = 0,			priceGoldDiscs = 0},
		new Item {name = "BlueSuedeShoes", 	unlockLevel = 4,	priceCoins = 10000,		priceGoldDiscs = 3},
		new Item {name = "EP",				unlockLevel = 8,	priceCoins = 20000,		priceGoldDiscs = 5},
		new Item {name = "Slowmo",			unlockLevel = 12,	priceCoins = 40000,		priceGoldDiscs = 10}
	};
	// Note, bitfield to test for PersistentData.consumableUnlocked
	public enum ConsumableUnlocked
	{
		SUNGLASSES		= 1 << 0,
		BELTBUCKLE		= 1 << 1,
		BLUESUEDESHOES	= 1 << 2,
		EP				= 1 << 3,
		SLOWMO			= 1 << 4
	};
	// Consumables Items (need both Coins and GDs), note unlock NA
	static public Item shades = 
		new Item {name = "Shades",			unlockLevel = 1,	priceCoins = 500,		priceGoldDiscs = 0};
	static public Item beltBuckle = 
		new Item {name = "BeltBuckle",		unlockLevel = 1,	priceCoins = 500,		priceGoldDiscs = 0};
	static public Item shoes = 
		new Item {name = "BlueSuedeShoes", 	unlockLevel = 1,	priceCoins = 1000,		priceGoldDiscs = 1};
	static public Item ep = 
		new Item {name = "EP",				unlockLevel = 1,	priceCoins = 2000,		priceGoldDiscs = 2};
	static public Item rpm = 
		new Item {name = "RPM",				unlockLevel = 1,	priceCoins = 4000,		priceGoldDiscs = 3};
	
	// Index into PersistentData.consumableItems
	public enum ConsumableItems
	{
		SHADES = 0,
		BELTBUCKLE,
		BLUESUEDESHOES,
		EP,
		RPM
	};
	
	// Missions
	public struct Mission
	{
		public string name;
		public int coinsPrize;
		public int coinsPrice;
		public int goldDiscsPrice;
	};
	static public Mission [] missions = {
		new Mission {name = "Mission1", 	coinsPrize = 50,	coinsPrice = 500,		goldDiscsPrice = 1},
		new Mission {name = "Mission2", 	coinsPrize = 100,	coinsPrice = 1000,		goldDiscsPrice = 2}
	};
}

