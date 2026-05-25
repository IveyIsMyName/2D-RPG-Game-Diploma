using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    private EntityVFX vfx;
    private EntityStats stats;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status effect details")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillSlowMultiplier = .2f;
    [SerializeField] private float electrifyChargeBuildUp = .4f;
    [Space]
    [SerializeField] private float fireScale = .8f;
    [SerializeField] private float lighningScale = 1.2f;

    private void Awake()
    {
        vfx = GetComponent<EntityVFX>();
        stats = GetComponent<EntityStats>();
    }

    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            float elementalDamage = stats.GetElementalDamage(out ElementType element);
            float damage = stats.GetPhysicalDamage(out bool isCrit);

            damagable.TakeDamage(damage, elementalDamage, element, transform);

            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);

            vfx.UpdateOnHitColor(element);
            vfx.CreateOnHitVFX(target.transform, isCrit);
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType element, float scaleFactor = 1)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

        if (statusHandler == null)
            return;

        if (element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChillEffect(defaultDuration, chillSlowMultiplier);

        if(element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            scaleFactor = fireScale;
            float fireDamage = stats.offense.fireDamage.GetValue() * scaleFactor;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }

        if(element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
        {
            scaleFactor = lighningScale;
            float lightningDamage = stats.offense.lightningDamage.GetValue() * scaleFactor;
            statusHandler.ApplyElectrifyEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
