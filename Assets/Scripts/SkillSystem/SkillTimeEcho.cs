using Unity.VisualScripting;
using UnityEngine;

public class SkillTimeEcho : SkillBase
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = .3f;

    [Header("Heal Wisp Upgrades")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp() == false)
            return 0;

        return damagePercentHealed;
    }

    public float GetCooldownReducedInSeconds()
    {
        if (upgradeType != SkillUpgradeType.TimeEchoCooldownWisp)
            return 0;

        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEchoCleanseWisp;
    }

    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEchoHealWisp
            || upgradeType == SkillUpgradeType.TimeEchoCleanseWisp
            || upgradeType == SkillUpgradeType.TimeEchoCooldownWisp;
    }

    public float GetDuplicateChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEchoChanceToDuplicate)
            return 0;

        return duplicateChance;
    }
    public int GetMaxAttacks()
    {
        if (upgradeType == SkillUpgradeType.TimeEchoSingleAttack || upgradeType == SkillUpgradeType.TimeEchoChanceToDuplicate)
            return 1;

        if (upgradeType == SkillUpgradeType.TimeEchoMultiAttack)
            return maxAttacks;

        return 0;
    }
    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        CreateTimeEcho();
        SetSkillOnCooldown();
    }

    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObjectTimeEcho>().SetupEcho(this);

        SetSkillOnCooldown();
    }
}
