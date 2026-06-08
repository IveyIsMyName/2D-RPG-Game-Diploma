using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryBase : MonoBehaviour
{
    public event Action onInventoryChange;

    protected Player player;
    public int maxInventorySize = 10;
    public List<InventoryItem> itemList = new List<InventoryItem>();

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TryUseItem(InventoryItem itemToUse)
    {
        InventoryItem consumable = itemList.Find(item => item == itemToUse);

        if (consumable == null)
            return;

		if (consumable.itemEffect.CanBeUsed(player) == false)
			return;

		consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveItem(consumable);

        onInventoryChange?.Invoke();
    }

    public bool CanAddItem() => itemList.Count < maxInventorySize;
    public InventoryItem FindStackable(InventoryItem itemToAdd)
    {
        List<InventoryItem> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach(var  stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }

    public void AddItem(InventoryItem itemToAdd)
    {
        InventoryItem itemInInventory = FindStackable(itemToAdd);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        onInventoryChange?.Invoke();
    }

    public void RemoveItem(InventoryItem itemToRemove)
    {
        itemList.Remove(itemToRemove);
        onInventoryChange?.Invoke();
    }

    public InventoryItem FindItem(InventoryItem itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    public InventoryItem FindSameItem(InventoryItem itemToFind)
    {
        return itemList.Find(item => item.itemData == itemToFind.itemData);
    }

    public void TriggerUpdateUI() => onInventoryChange?.Invoke();
}
