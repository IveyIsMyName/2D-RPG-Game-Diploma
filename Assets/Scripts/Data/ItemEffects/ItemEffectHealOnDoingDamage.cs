using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal on doing damage", fileName = "Item effect Data - Heal on doing phys damage")]
public class ItemEffectHealOnDoingDamage : ItemEffectDataSO
{
	[SerializeField] private float percentHealedOnAttack = .2f;


	public override void Subscribe(Player player)
	{
		base.Subscribe(player);
		player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
	}

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
		player = null;
	}
	private void HealOnDoingDamage(float damage)
	{
		player.health.IncreaseHealth(damage * percentHealedOnAttack);
	}
}
