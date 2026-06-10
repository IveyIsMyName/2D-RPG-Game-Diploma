using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UISkillToolTip skillToolTip { get; private set; }
    public UIItemToolTip itemToolTip { get; private set; }
    public UIStatToolTip statToolTip { get; private set; }

    public UISkillTree skillTreeUI { get; private set; }
    public UIInventory inventoryUI { get; private set; }
    public UIInGame inGameUI { get; private set; }
    public UIOptions optionsUI { get; private set; }
    public UIDeathScreen deathScreenUI { get; private set; }
    public UIFadeScreen fadeScreenUI { get; private set; }

    private PlayerInputSet input;
    public bool alternativeInput {  get; private set; }

    private bool inventoryEnabled;
    private bool skillTreeEnabled;

    [SerializeField] private GameObject[] uiElements;

    private void Awake()
    {
        instance = this;

        itemToolTip = GetComponentInChildren<UIItemToolTip>();
        skillToolTip = GetComponentInChildren<UISkillToolTip>();
        statToolTip = GetComponentInChildren<UIStatToolTip>();

        skillTreeUI = GetComponentInChildren<UISkillTree>(true);
        inventoryUI = GetComponentInChildren<UIInventory>(true);
        inGameUI = GetComponentInChildren<UIInGame>(true);
        optionsUI = GetComponentInChildren<UIOptions>(true);
        deathScreenUI = GetComponentInChildren<UIDeathScreen>(true);
        fadeScreenUI = GetComponentInChildren<UIFadeScreen>(true);

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
		fadeScreenUI.transform.SetAsLastSibling();

		skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControls(skillTreeEnabled);
    }

    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllTooltips();

        StopPlayerControls(inventoryEnabled);
    }

    public void OpenOptionsUI()
    {
        HideAllTooltips();
        StopPlayerControls(true);
        SwitchTo(optionsUI.gameObject);
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        StopPlayerControls(true);
        input.Disable();
    }

    public void SwitchToInGameUI()
    {
        HideAllTooltips();
        StopPlayerControls(false);

        SwitchTo(inGameUI.gameObject);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    private void SwitchTo(GameObject objectToSwitchOn)
    {
		foreach (var element in uiElements)
			element.gameObject.SetActive(false);

        objectToSwitchOn.SetActive(true);
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
