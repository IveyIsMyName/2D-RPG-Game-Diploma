using System.Collections;
using UnityEngine;

public class SkillShard : SkillBase
{
    private SkillObjectShard currentShard;
    private EntityHealth playerHealth;

    [SerializeField] private GameObject sharedPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isRecharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<EntityHealth>();
    }
    public void CreateShard()
    {
        float detonateTime = GetDetonateTime();

        GameObject shard = Instantiate(sharedPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObjectShard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.ShardTeleport) || Unlocked(SkillUpgradeType.ShardTeleportHPRewind))
            currentShard.onExplode += ForceCooldown;
    }

    public void CreateRawShard(Transform target = null, bool shardsCanMove = false)
    {
        bool canMove = shardsCanMove == false ?
            Unlocked(SkillUpgradeType.ShardMoveToEnemy) || Unlocked(SkillUpgradeType.ShardMultiCast)
            : shardsCanMove;

        GameObject shard = Instantiate(sharedPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObjectShard>().SetupShard(this, detonateTime, canMove, shardSpeed, target);
    }

    public void CreateDomainShard(Transform target)
    {

    }

    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.ShardMoveToEnemy))
            HandleShardMoving();

        if(Unlocked(SkillUpgradeType.ShardMultiCast))
            HandleShardMulticast();

        if (Unlocked(SkillUpgradeType.ShardTeleport))
            HandleShardTeleport();

        if(Unlocked(SkillUpgradeType.ShardTeleportHPRewind))
            HandleShardHealthRewind();
    }

    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if(currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();

        player.TeleportPlayer(shardPosition);
    }

    private void HandleShardMulticast()
    {
        if (currentCharges <= 0)
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCooldown();
    }

    private IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

   

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.ShardTeleport) || Unlocked(SkillUpgradeType.ShardTeleportHPRewind))
            return shardExistDuration;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.onExplode -= ForceCooldown;
        }
    }
}
