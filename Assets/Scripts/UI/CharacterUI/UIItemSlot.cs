using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public InventoryItem itemInSlot { get; private set; }
	protected InventoryPlayer inventory;
	protected UI ui;
	protected RectTransform rect;

	[Header("UI Slot Setup")]
	[SerializeField] protected GameObject defaultIcon;
	[SerializeField] protected Image itemIcon;
	[SerializeField] protected TextMeshProUGUI itemStackSize;

	protected virtual void Awake()
	{
		ui = GetComponentInParent<UI>();
		rect = GetComponent<RectTransform>();
		inventory = FindAnyObjectByType<InventoryPlayer>();
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
			return;
		ui.StopPlayerControls(itemInSlot.itemData);
		bool alternativeInput = Input.GetKey(KeyCode.LeftControl);

		if (alternativeInput)
		{
			inventory.RemoveItem(itemInSlot);
		}
		else
		{
			if (itemInSlot.itemData.itemType == ItemType.Consumable)
			{


				inventory.TryUseItem(itemInSlot);
			}
			else
				inventory.TryEquipItem(itemInSlot);
		}

		if (itemInSlot == null)
			ui.itemToolTip.ShowToolTip(false, null);
	}

	public void UpdateSlot(InventoryItem item)
	{
		itemInSlot = item;
		if (defaultIcon != null)
			defaultIcon.gameObject.SetActive(itemInSlot == null);

		if (itemInSlot == null)
		{
			itemStackSize.text = "";
			itemIcon.color = Color.clear;
			return;
		}

		Color color = Color.white;
		color.a = .9f;
		itemIcon.color = color;
		itemIcon.sprite = itemInSlot.itemData.itemIcon;
		itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (itemInSlot == null) return;

		ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		ui.itemToolTip.ShowToolTip(false, null);
	}
}
