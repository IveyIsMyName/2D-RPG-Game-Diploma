using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        base.TakeDamage(damage, elementalDamage, element, damageDealer);

        if (isDead)
            return;

        if (damageDealer.CompareTag("Player"))
            enemy.TryEnterBattleState(damageDealer);
    }
}
