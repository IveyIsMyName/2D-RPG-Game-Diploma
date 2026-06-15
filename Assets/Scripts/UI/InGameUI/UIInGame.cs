using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class UIInGame : MonoBehaviour
{
    private Player player;
	private InventoryPlayer inventory;
	private UISkillSlot[] skillSlots;
	private UI ui;

    [SerializeField] private RectTransform healthRect;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

	[Header("Quick Item Slots")]
	[SerializeField] private float yOffsetQuickItemParent = 150;
	[SerializeField] private Transform quickItemOptionsParent;
	private UIQuickItemSlotOption[] quickItemOptions;
	private UIQuickItemSlot[] quickItemSlots;

	private void Awake()
	{
		ui = GetComponentInParent<UI>();
	}
	private void Start()
	{
		quickItemSlots = GetComponentsInChildren<UIQuickItemSlot>();

		player = FindAnyObjectByType<Player>();
		player.health.OnHealthUpdate += UpdateHealthBar;

		inventory = player.inventory;
		inventory.onInventoryChange += UpdateQuickSlotsUI;
		inventory.OnQuickSlotUsed += PlayQuickSlotFeedback;
	}

	public void PlayQuickSlotFeedback(int slotNumber) => quickItemSlots[slotNumber].SimulateButtonFeedback();

	public void UpdateQuickSlotsUI()
	{
		InventoryItem[] quickItems = inventory.quickItems;

		for (int i = 0; i < quickItems.Length; i++)
			quickItemSlots[i].UpdateQuickSlotUI(quickItems[i]);
	}

	public void OpenQuickItemOptions(UIQuickItemSlot quickItemSlot, RectTransform targetRect)
	{
		if (ui != null)
			ui.StopPlayerControls(true);

		if (quickItemOptions == null)
			quickItemOptions = quickItemOptionsParent.GetComponentsInChildren<UIQuickItemSlotOption>(true);

		List<InventoryItem> consumables = inventory.itemList.FindAll(item => item != null && item.itemData != null && item.itemData.itemType == ItemType.Consumable);

		for (int i = 0; i < quickItemOptions.Length; i++)
		{
			if(i < consumables.Count)
			{
				quickItemOptions[i].gameObject.SetActive(true);
				quickItemOptions[i].SetupOption(quickItemSlot, consumables[i]);
			}
			else
				quickItemOptions[i].gameObject.SetActive(false);
		}

		quickItemOptionsParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
	}

	public void HideQuickItemOptions()
	{
		quickItemOptionsParent.position = new Vector3(0, 9999);

		if (ui != null)
			ui.StopPlayerControls(false);
	}

	public UISkillSlot GetSkillSlot(SkillType skillType)
	{
		if (skillSlots == null)
			skillSlots = GetComponentsInChildren<UISkillSlot>(true);

		foreach (var slot in skillSlots)
		{
			if (slot.skillType == skillType || slot.conflictSkillType == skillType)
			{
				slot.gameObject.SetActive(true);
				return slot;
			}
		}
		return null;
	}
	public void ClearAllSkillSlots()
	{
		if (skillSlots == null)
			skillSlots = GetComponentsInChildren<UISkillSlot>(true);

		foreach (var slot in skillSlots)
		{
			slot.ClearSlot();
			slot.gameObject.SetActive(true);
		}
	}
	private void UpdateHealthBar()
	{
		float currentHealth = Mathf.RoundToInt(player.health.GetCurrentHealth());
		float maxHealth = player.stats.GetMaxHealth();

		healthText.text = currentHealth + "/" + maxHealth;
		healthSlider.value = player.health.GetHealthPercent();
	}

}
