using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryBase
{
	public event Action<int> OnQuickSlotUsed;

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

}
