using UnityEngine;

public class UISkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UITreeConnectHandler[] parentNodes;
    public PlayerSkillManager skillManager { get; private set; }

    private void Awake()
    {
        skillManager = FindAnyObjectByType<PlayerSkillManager>();
    }

    private void Start()
    {
        UpdateAllConnections();
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
    public void RemoveSkillPoints(int cost) => skillPoints -= cost;
    public void AddSkillPoints(int points) => skillPoints += points;

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
