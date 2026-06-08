using System.Text;
using TMPro;
using UnityEngine;

public class UIItemToolTip : UIToolTip
{
	[SerializeField] private TextMeshProUGUI itemName;
	[SerializeField] private TextMeshProUGUI itemType;
	[SerializeField] private TextMeshProUGUI itemInfo;
	[SerializeField] private Transform inventoryInfo;

	public void ShowToolTip(bool show, RectTransform targetRect, InventoryItem itemToShow)
	{
		base.ShowToolTip(show, targetRect);

		itemName.text = itemToShow.itemData.itemName;
		itemType.text = itemToShow.itemData.itemType.ToString();
		itemInfo.text = GetItemInfo(itemToShow);
	}

	public string GetItemInfo(InventoryItem item)
	{
		if (item.itemData.itemType == ItemType.Material)
			return "Used for crafting.";

		if (item.itemData.itemType == ItemType.Consumable)
			return item.itemData.itemEffect.effectDescription;

		StringBuilder sb = new StringBuilder();

		sb.Append("");

		foreach (var mod in item.modifiers)
		{
			string modType = GetStatNamyByType(mod.statType);
			string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
			sb.AppendLine("+ " + modValue + " " + modType);
		}

		if(item.itemEffect != null)
		{
			sb.AppendLine("");
			sb.AppendLine("Unique effect:");
			sb.AppendLine(item.itemEffect.effectDescription);
		}

		return sb.ToString();
	}

	private string GetStatNamyByType(StatType type)
	{
		switch (type)
		{
			case StatType.MaxHealth: return "Max Health";
			case StatType.HealthRegen: return "Health Regeneration";
			case StatType.Strength: return "Strength";
			case StatType.Agility: return "Agility";
			case StatType.Intelligence: return "Intelligence";
			case StatType.Vitality: return "Vitality";
			case StatType.AttackSpeed: return "Attack Speed";
			case StatType.Damage: return "Damage";
			case StatType.CritChance: return "Critical Chance";
			case StatType.CritPower: return "Critical Power";
			case StatType.ArmorReduction: return "Armor Reduction";
			case StatType.FireDamage: return "Fire Damage";
			case StatType.IceDamage: return "Ice Damage";
			case StatType.LightningDamage: return "Lightning Damage";
			case StatType.Armor: return "Armor";
			case StatType.Evasion: return "Evasion";
			case StatType.IceResistance: return "Ice Resistance";
			case StatType.FireResistance: return "Fire Resistance";
			case StatType.LightningResistance: return "Lightning Resistance";
			default: return "Uknown Stat";
		}
	}

	private bool IsPercentageStat(StatType type)
	{
		switch (type)
		{
			case StatType.CritChance:
			case StatType.CritPower:
			case StatType.ArmorReduction:
			case StatType.IceResistance:
			case StatType.FireResistance:
			case StatType.LightningResistance:
			case StatType.AttackSpeed:
			case StatType.Evasion:
				return true;
			default:
				return false;
		}
	}
}
