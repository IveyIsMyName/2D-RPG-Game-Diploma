using UnityEngine;

public class UIPlayerStats : MonoBehaviour
{
    private UIStatSlot[] uIStatSlots;
    private InventoryPlayer inventory;

	private void Awake()
	{
		uIStatSlots = GetComponentsInChildren<UIStatSlot>();

		inventory = FindAnyObjectByType<InventoryPlayer>();
		inventory.onInventoryChange += UpdateStatsUI;
	}

	private void Start()
	{
		UpdateStatsUI();
	}

	private void UpdateStatsUI()
	{
		foreach (var statSlot in uIStatSlots)
			statSlot.UpdateStatValue();
	}
}
