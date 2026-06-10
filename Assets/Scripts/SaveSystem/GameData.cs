using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
	public int skillPoints;
	public List<InventoryItem> itemList;
	public SerializableDictionary<string, int> inventory;
	public SerializableDictionary<string, ItemType> equipedItems;
	public SerializableDictionary<string, bool> skillTreeUI;
	public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades;
	public SerializableDictionary<string, bool> unlockedCheckpoints;

	public string lastScenePlayed;
	public Vector3 lastPlayerPosition;
	

	public GameData()
	{
		inventory = new SerializableDictionary<string, int>();
		equipedItems = new SerializableDictionary<string, ItemType>();
		skillTreeUI = new SerializableDictionary<string, bool>();
		skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();
		unlockedCheckpoints = new SerializableDictionary<string, bool>();
	}


}
