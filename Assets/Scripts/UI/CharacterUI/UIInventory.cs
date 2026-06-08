using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    private InventoryPlayer inventory;
    private UIItemSlot[] uiItemSlots;
    private UIEquipSlot[] uiEquipSlots;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UIItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UIEquipSlot>();

        inventory = FindAnyObjectByType<InventoryPlayer>();
        inventory.onInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdadeInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<InventoryEquipmentSlot> playerEquipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (playerEquipSlot.HasItem() == false)
                uiEquipSlots[i].UpdateSlot(null);
            else
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }

    private void UpdadeInventorySlots()
    {
        List<InventoryItem> itemList = inventory.itemList;

        for (int i = 0; i < uiItemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        }
    }
}
