using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillToolTip skillToolTip { get; private set; }
    public UIItemToolTip itemToolTip { get; private set; }
    public UIStatToolTip statToolTip { get; private set; }

    public UISkillTree skillTreeUI { get; private set; }
    public UIInventory inventoryUI { get; private set; }
    public UIInGame inGameUI { get; private set; }
    public UIOptions optionsUI { get; private set; }

    private PlayerInputSet input;
    public bool alternativeInput {  get; private set; }

    private bool inventoryEnabled;
    private bool skillTreeEnabled;

    [SerializeField] private GameObject[] uiElements;

    private void Awake()
    {
        itemToolTip = GetComponentInChildren<UIItemToolTip>();
        skillToolTip = GetComponentInChildren<UISkillToolTip>();
        statToolTip = GetComponentInChildren<UIStatToolTip>();

        skillTreeUI = GetComponentInChildren<UISkillTree>(true);
        inventoryUI = GetComponentInChildren<UIInventory>(true);
        inGameUI = GetComponentInChildren<UIInGame>(true);
        optionsUI = GetComponentInChildren<UIOptions>(true);

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

	private void Start()
	{
        skillTreeUI.UnlockDefaultSkills();
	}

    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTreeUI();
        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.OptionsUI.performed += ctx =>
        {
            foreach(var element in uiElements)
            {
                if(element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }

    public void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

	public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControls(skillTreeEnabled);
    }

    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllTooltips();

        StopPlayerControls(inventoryEnabled);
    }

    public void OpenOptionsUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(true);
        optionsUI.gameObject.SetActive(true);
    }

    public void SwitchToInGameUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(false);
        inGameUI.gameObject.SetActive(true);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

	public void HideAllTooltips()
	{
		itemToolTip.ShowToolTip(false, null);
		skillToolTip.ShowToolTip(false, null);
		statToolTip.ShowToolTip(false, null);
	}

    private void SetTooltipsAsLastSibling()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }

}
