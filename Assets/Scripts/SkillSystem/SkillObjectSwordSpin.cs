using UnityEngine;

public class SkillObjectSwordSpin : SkillObjectSword
{
    private int maxDistance;
    private float attackPerSecond;
    private float attackTimer;

    public override void SetupSword(SkillSwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);

        anim?.SetTrigger("spin");

        maxDistance = swordManager.maxDistance;
        attackPerSecond = swordManager.attackPerSecond;

        Invoke(nameof(GetSwordBackToPlayer), swordManager.maxSpinDuration);
    }

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if( attackTimer < 0 )
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1 / attackPerSecond;
        }
    }

    private void HandleStopping()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > maxDistance && rb.simulated == true)
            rb.simulated = false;

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
