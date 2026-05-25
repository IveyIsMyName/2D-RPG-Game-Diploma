using UnityEngine;

public interface IDamagable 
{
    public void TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer);
}
