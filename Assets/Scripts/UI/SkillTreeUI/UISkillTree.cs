using TMPro;
using UnityEngine;

public class UISkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UITreeConnectHandler[] parentNodes;
    private UITreeNode[] allTreeNodes;
    public PlayerSkillManager skillManager { get; private set; }

    private void Start()
	{
		UpdateAllConnections();
		UpdatSkillPointsUI();
	}

	private void UpdatSkillPointsUI()
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
    }

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdatSkillPointsUI();
    }

    public void AddSkillPoints(int points)
    {
        skillPoints += points;
		UpdatSkillPointsUI();
	}

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
