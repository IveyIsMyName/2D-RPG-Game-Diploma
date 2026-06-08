using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipSlot : UIItemSlot
{
    public ItemType slotType;

    private void OnValidate()
    {
        gameObject.name = "UI_EquipemntSlot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        inventory.UnequipItem(itemInSlot);
    }
}
