using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public PlayerSkillManager skillManager { get; private set;  }
    public Player player {  get; private set; }
    public DamageScaleData damageScaleData { get; private set; }

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<PlayerSkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed -= cooldown;
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(SkillDataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScale;

		var slot = player.ui.inGameUI.GetSkillSlot(skillType);
		if (slot != null)
		{
			slot.SetupSkillSlot(skillData);
			ResetCooldown();
		}
		else
		{
			Debug.LogError($"[SkillBase] Не найден UI слот для навыка: {skillType}. Проверьте настройки UISkillSlot в Инспекторе!");
		}
	}

    public virtual bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCooldown())
        {
            Debug.LogWarning("On Cooldown");
            return false;
        }

        return true;
    }
	public void ResetSkill()
	{
		upgradeType = SkillUpgradeType.None;
		cooldown = 0f;
		damageScaleData = new DamageScaleData();

		// Очищаем слот в интерфейсе
		var slot = player.ui.inGameUI.GetSkillSlot(skillType);
		if (slot != null)
		{
			slot.ClearSlot(); 
		}

		ResetCooldown();
	}

	protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReducedCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }

    public SkillUpgradeType GetUpgrade() => upgradeType;
    public SkillType GetSkillType() => skillType;
}
