using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UISkillTree : MonoBehaviour, ISaveable
{
    [SerializeField] private int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UITreeConnectHandler[] parentNodes;
    private UITreeNode[] allTreeNodes;
    public PlayerSkillManager skillManager { get; private set; }

	private void Start()
	{
		UpdateAllConnections();
		UpdateSkillPointsUI();
	}

	private void UpdateSkillPointsUI()
	{
		skillPointsText.text = skillPoints.ToString();
	}

	public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UITreeNode>(true);
		skillManager = FindAnyObjectByType<PlayerSkillManager>();

		foreach (var node in allTreeNodes)
            node.UnlockDefaultSkill();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
		UITreeNode[] skillNodes = GetComponentsInChildren<UITreeNode>();

		foreach (var node in skillNodes)
		{
			node.Refund();
		}

		// Дополнительно: убедимся, что все слоты видны
		if (UI.instance?.inGameUI != null)
		{
			var slots = UI.instance.inGameUI.GetComponentsInChildren<UISkillSlot>(true);
			foreach (var slot in slots)
			{
				slot.gameObject.SetActive(true); // Гарантируем, что слоты не скрыты
			}
		}
	}

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }

    public void AddSkillPoints(int points)
    {
        skillPoints += points;
		UpdateSkillPointsUI();
	}

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

	public void LoadData(GameData data)
	{
		skillPoints = data.skillPoints;

        foreach(var node in allTreeNodes)
        {
			string skillName = node.skillData.displayName;

            if (data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUpgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);
                if (upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }

        }
	}

	public void SaveData(ref GameData data)
	{
		data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }

        foreach(var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
	}
}
