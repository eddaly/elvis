using UnityEngine;
using System.Collections;

public class Metagame
{
	// Hard currency price (either Coins or cash)
	public int m_GoldDiscsPriceCoins = 10000;
	public float m_GoldDiscsPriceCashGBP = .99f;
	
	// Upgrades (need both Coins and GDs)
	public struct Upgrade
	{
		public string name;
		public int unlockLevel;
		public int priceCoins;
		public int priceGoldDiscs;
	};
	static public Upgrade [] upgrades = {
		new Upgrade {name = "JumpsuitRed",		unlockLevel = 1,	priceCoins = 5000,		priceGoldDiscs = 1},
		new Upgrade {name = "JumpsuitSilver",	unlockLevel = 10,	priceCoins = 25000,		priceGoldDiscs = 5},
		new Upgrade {name = "JumpsuitGold", 	unlockLevel = 20,	priceCoins = 150000,	priceGoldDiscs = 20}
	};
	public enum UpgradesSave
	{
		JUMPSUIT_NONE = 0,
		JUMPSUIT_RED,
		JUMPSUIT_SILVER,
		JUMPSUIT_GOLD
	};	
	
	// Items (need both Coins and GDs)
	public struct Item
	{
		public string name;
		public int unlockLevel;
		public int priceCoins;
		public int priceGoldDiscs;
	};
	static public Item [] items = {
		new Item {name = "BlueSuedeShoes",	unlockLevel = 5,	priceCoins = 10000,		priceGoldDiscs = 0},
		new Item {name = "SpeedwayHelmet",	unlockLevel = 12,	priceCoins = 40000,		priceGoldDiscs = 1},
		new Item {name = "ArmyGun", 		unlockLevel = 16,	priceCoins = 100000,	priceGoldDiscs = 5}
	};
	// Bitfield
	public enum ItemsSave
	{
		BLUESUEDESHOES	= 1 << 0,
		SPEEDWAYHELMET	= 1 << 1,
		ARMYGUN			= 1 << 2
	};

	// Level-up cost (20pc increase rounded), note for convienience level[0] is meaningless and level[1] is free
	static public int [] levelUpXP = {-1, 0, 
		500, 600, 700, 900, 1000, 1200, 1500, 1800, 2100, 2600, 3100, 3700, 4500, 5300, 6400, 7700, 9200, 11100, 13300};
	static public int maxLevels = levelUpXP.Length;
	
	// Missions
	public struct Mission
	{
		public string name;
		public int coinsPrize;
		public int coinsPrice;
		public int goldDiscsPrice;
	};
	static public Mission [] missions = {
		new Mission {name = "Mission1", coinsPrize = 50,	coinsPrice = 500,	goldDiscsPrice = 1},
		new Mission {name = "Mission2", coinsPrize = 100,	coinsPrice = 1000,	goldDiscsPrice = 2}
	};
}

