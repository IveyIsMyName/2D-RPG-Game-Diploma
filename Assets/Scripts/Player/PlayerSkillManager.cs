using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash dash { get; private set; }
    public SkillShard shard { get; private set; }
    public SkillSwordThrow swordThrow { get; private set; }
    public SkillTimeEcho timeEcho { get; private set; }
    public SkillDomainExpansion domainExpansion { get; private set; }

    public SkillBase[] allSkills { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<SkillDash>();
        shard = GetComponentInChildren<SkillShard>();
        swordThrow = GetComponentInChildren<SkillSwordThrow>();
        timeEcho = GetComponentInChildren<SkillTimeEcho>();
        domainExpansion = GetComponentInChildren<SkillDomainExpansion>();

        allSkills = GetComponentsInChildren<SkillBase>();
    }

    public void ReduceAllSkillsCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReducedCooldownBy(amount);
    }

    public SkillBase GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return swordThrow;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.DomainExpansion: return domainExpansion;

            default:
                Debug.Log($"Skill type {type} is not implemented yet");
                return null;
        }
    }
}
