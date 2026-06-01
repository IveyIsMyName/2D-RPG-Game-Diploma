using UnityEngine;

public class SkillObjectBase : MonoBehaviour
{
    [SerializeField] private GameObject onHitVFX; 
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected EntityStats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in GetEnemiesAround(t, radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

            float physicalDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);
            
            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            lastTarget = target.transform;
            Instantiate(onHitVFX, target.transform.position, Quaternion.identity);

            usedElement = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if (targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
