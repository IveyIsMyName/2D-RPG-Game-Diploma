using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect Data - Ice blast on taking damage")]
public class ItemEffectIceBlastOnTakingDamage : ItemEffectDataSO
{
	[SerializeField] private ElementalEffectData effectData;
	[SerializeField] private float iceDamage;
	[SerializeField] private LayerMask whatIsEnemy;

	[Space]
	[SerializeField] private float healthPercentTrigger = .25f;
	[SerializeField] private float cooldown;
	private float lastTimeUsed = -999;
	[Header("VFX Objects")]
	[SerializeField] private GameObject iceBlastVFX;
	[SerializeField] private GameObject onHitVFX;

	public override void ExecuteEffect()
	{
		bool noCooldown = Time.time >= lastTimeUsed + cooldown;
		bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

		if (noCooldown && reachedThreshold)
		{
			player.vfx.CreateEffectOf(iceBlastVFX, player.transform);
			lastTimeUsed = Time.time;
			DamageEnemiesWithIce();
		}
	}

	private void DamageEnemiesWithIce()
	{
		Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

		foreach (var target in enemies)
		{
			IDamagable damagable = target.GetComponent<IDamagable>();

			if (damagable == null) continue;

			damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
			EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();
			statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

			
			player.vfx.CreateEffectOf(onHitVFX, target.transform);
		}
	}

	public override void Subscribe(Player player)
	{
		base.Subscribe(player);
		player.health.OnTakingDamage += ExecuteEffect;
	}

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		player.health.OnTakingDamage -= ExecuteEffect;
		player = null;
	}
}
