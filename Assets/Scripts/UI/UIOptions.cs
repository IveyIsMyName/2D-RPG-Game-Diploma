using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    private UI ui;
    [SerializeField] private Button resetSkillsBtn;

	private void Start()
	{
		ui = FindAnyObjectByType<UI>();
	}

	public void ResetAllSkills()
	{
		ui.skillTreeUI.RefundAllSkills();
	}

	public void GoMainMenuBtn() => GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
}
