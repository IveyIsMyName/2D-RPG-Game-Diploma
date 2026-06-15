using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamagable
{
    public event Action OnTakingDamage;
    public event Action OnHealthUpdate;

    private Slider healthBar;
    private EntityVFX entityVFX;
    private EntityStats entityStats;
    private Entity entity;
    private EntityDropManager dropManager;

    [SerializeField] private float currentHealth;
    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamageTaken {  get; private set; }
    public bool isDead { get; private set; }
    protected bool canTakeDamage = true;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [Header("On Heavy Damage KnockBack")]
    [SerializeField] private float heavyDamageTreshold = .3f; //percentage of maxHP you should lose to get heavy knockback

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        healthBar = GetComponentInChildren<Slider>();
        dropManager = GetComponent<EntityDropManager>();

        SetupHealth();
    }

    private void SetupHealth()
    {
        if (entityStats == null)
            return;

        currentHealth = entityStats.GetMaxHealth();
        OnHealthUpdate += UpdateHealthBar;

        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual void TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false)
            return;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats != null ? entityStats.GetElementalResistance(element) : 0;
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockBack(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;

        OnTakingDamage?.Invoke();
    }

    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;

    private bool AttackEvaded()
    {
        if (entityStats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        OnHealthUpdate?.Invoke();
    }

    public void ReduceHealth(float damage)
    {
		if (isDead) return;

		entityVFX?.PlayOnDamageVFX();
        currentHealth -= damage;
        OnHealthUpdate?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
		if (isDead) return;

		isDead = true;
        entity?.EntityDeath();
        dropManager?.DropItems();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }
    private void TakeKnockBack(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockBack(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockBack(knockback, duration);
    }
    private Vector2 CalculateKnockBack(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;

        return knockback;
    }

    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        OnHealthUpdate?.Invoke();
    }
    public float GetCurrentHealth() => currentHealth;

    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHealth();
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null)
            return false;
        else
            return damage / entityStats.GetMaxHealth() > heavyDamageTreshold;
    }
}
