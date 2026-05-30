using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash dash { get; private set; }
    public SkillShard shard { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<SkillDash>();
        shard = GetComponentInChildren<SkillShard>();
    }

    public SkillBase GetSkillByType(SkillType type)
    {
        switch (type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;

            default:
                Debug.Log($"Skill type {type} is not implemented yet");
                return null;
        }
    }
}
