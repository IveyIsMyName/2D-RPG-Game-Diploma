using UnityEngine;

public class SkillDash : SkillBase
{
    public void OnStartEffect()
    {
        if(Unlocked(SkillUpgradeType.DashCloneOnStart) || Unlocked(SkillUpgradeType.DashCloneStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.DashShardOnStart) || Unlocked(SkillUpgradeType.DashShardOnStartAndArrival))
            CreateShard();
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.DashCloneStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.DashShardOnStartAndArrival))
            CreateShard();
    }

    private void CreateShard()
    {
        skillManager.shard.CreateRawShard();
    }

    private void CreateClone()
    {
        Debug.Log("Create a time echo!");
    }
}
