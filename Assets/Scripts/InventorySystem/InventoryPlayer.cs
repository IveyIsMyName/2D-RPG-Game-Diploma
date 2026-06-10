using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryBase, ISaveable
{
	public event Action<int> OnQuickSlotUsed;
	[SerializeField] private ItemListDataSO itemDataBase;

	public List<InventoryEquipmentSlot> equipList;

	[Header("Quick Item SLots")]
	public InventoryItem[] quickItems = new InventoryItem[2];

	protected override void Awake()
	{
		base.Awake();
	}

	public void SetQuickItemInSlot(int slotNumber, InventoryItem itemToSet)
	{
		quickItems[slotNumber - 1] = itemToSet;
		TriggerUpdateUI();
	}

	public void TryUseQuickItemInSlot(int passedSlotNumber)
	{
		int slotNumber = passedSlotNumber - 1;
		var itemToUse = quickItems[slotNumber];

		if (itemToUse == null)
			return;

		TryUseItem(itemToUse);

		if (FindItem(itemToUse) == null)
		{
			quickItems[slotNumber] = FindSameItem(itemToUse);
		}
		TriggerUpdateUI();
		OnQuickSlotUsed?.Invoke(slotNumber);
	}

	public void TryEquipItem(InventoryItem item)
	{
		var inventoryItem = FindItem(item);
		var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

		//STEP 1: Try to find an empty slot and equip item
		foreach (var slot in matchingSlots)
		{
			if (slot.HasItem() == false)
			{
				EquipItem(inventoryItem, slot);
				return;
			}
		}

		//STEP2: No empty slots ? Replace first one

		var slotToReplace = matchingSlots[0];
		var itemToUnequip = slotToReplace.equipedItem;

		UnequipItem(itemToUnequip, slotToReplace != null);
		EquipItem(inventoryItem, slotToReplace);
	}

	private void EquipItem(InventoryItem itemToEquip, InventoryEquipmentSlot slot)
	{
		float savedHealthPercent = player.health.GetHealthPercent();

		slot.equipedItem = itemToEquip;
		slot.equipedItem.AddModifiers(player.stats);
		slot.equipedItem.AddItemEffect(player);

		player.health.SetHealthToPercent(savedHealthPercent);
		RemoveItem(itemToEquip);
	}

	public void UnequipItem(InventoryItem itemToUnequip, bool replacingItem = false)
	{
		if (CanAddItem() == false && replacingItem == false)
		{
			Debug.Log("No space in the inventory!");
			return;
		}

		float savedHealthPercent = player.health.GetHealthPercent();
		var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

		if (slotToUnequip != null)
			slotToUnequip.equipedItem = null;

		itemToUnequip.RemoveModifiers(player.stats);
		itemToUnequip.RemoveItemEffect();

		player.health.SetHealthToPercent(savedHealthPercent);
		AddItem(itemToUnequip);
	}

	public void LoadData(GameData data)
	{
		foreach (var item in data.inventory)
		{
			string saveID = item.Key;
			int stackSize = item.Value;

			ItemDataSO itemData = itemDataBase.GetItemData(saveID);

			if (itemData == null)
			{
				Debug.LogWarning("Item not found: " + saveID);
				continue;
			}


			for (int i = 0; i < stackSize; i++)
			{
				InventoryItem itemToLoad = new InventoryItem(itemData);
				AddItem(itemToLoad);
			}
		}

		foreach(var entry in data.equipedItems)
		{
			string saveID = entry.Key;
			ItemType loadedSlotType = entry.Value;

			ItemDataSO itemData = itemDataBase.GetItemData(saveID);
			InventoryItem itemToLoad = new InventoryItem(itemData);

			var slot = equipList.Find(slot => slot.slotType == loadedSlotType && slot.HasItem() == false); 

			slot.equipedItem = itemToLoad;
			slot.equipedItem.AddModifiers(player.stats);
			slot.equipedItem.AddItemEffect(player);
		}

		TriggerUpdateUI();
	}

	public void SaveData(ref GameData data)
	{
		data.inventory.Clear();
		data.equipedItems.Clear();

		foreach (var item in itemList)
		{
			if (item != null && item.itemData != null)
			{
				string saveID = item.itemData.saveID;

				if (data.inventory.ContainsKey(saveID) == false)
					data.inventory[saveID] = 0;

				data.inventory[saveID] += item.stackSize;

			}
		}

		foreach (var slot in equipList)
		{
			if (slot.HasItem())
				data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
		}
	}
}
