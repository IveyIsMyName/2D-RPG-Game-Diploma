using UnityEngine;

public class SkillObjectTimeEcho : SkillObjectBase
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVFX;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPlayer = false;

    private Transform playerTransform;
    private SkillTimeEcho echoManager;
    private TrailRenderer wispTrail;
    private EntityHealth playerHealth;
    private SkillObjectHealth echoHealth;
    private PlayerSkillManager skillManager;
    private EntityStatusHandler statusHandler;

    public int maxAttacks { get; private set; }

    public void SetupEcho(SkillTimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;
        playerHealth = echoManager.player.health;
        skillManager = echoManager.skillManager;
        statusHandler = echoManager.player.statusHandler;

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
        FlipToTarget();

        echoHealth = GetComponent<SkillObjectHealth>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack", maxAttacks > 0);
    }
    private void Update()
    {
        if (shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }

    }

    private void HandlePlayerTouch()
    {
        float healAmount = echoHealth.lastDamageTaken * echoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);

        float amountInSeconds = echoManager.GetCooldownReducedInSeconds();
        skillManager.ReduceAllSkillsCooldownBy(amountInSeconds);

        if (echoManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects();
    }


    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playerTransform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttack()
    {
        lastTarget = null;
        DamageEnemiesInRadius(targetCheck, 1);

        if (lastTarget == null)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
            TurnIntoWisp();
        else
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPlayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
