using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal effect", fileName = "Item effect Data - Heal")]

public class ItemEffectHeal : ItemEffectDataSO
{
	[SerializeField] private float healPercent = .1f;

	public override void ExecuteEffect()
	{
		Player player = FindAnyObjectByType<Player>();

		float healAmount = player.stats.GetMaxHealth() * healPercent;

		player.health.IncreaseHealth(healAmount);
	}
}
