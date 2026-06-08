using UnityEngine;
using UnityEngine.EventSystems;

public class UIQuickItemSlotOption : UIItemSlot
{
	private UIQuickItemSlot currentQuickItemSlot;
	
	public void SetupOption(UIQuickItemSlot currentQuickItemSlot, InventoryItem itemToSet)
	{
		this.currentQuickItemSlot = currentQuickItemSlot;
		UpdateSlot(itemToSet);
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
		ui.inGameUI.HideQuickItemOptions();
	}
	
}
