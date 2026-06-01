using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SkillDomainExpansion : SkillBase
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5;

    [Header("Shard Cast Upgrade")]
    [SerializeField] private int shardsToCast = 10;
    [SerializeField] private float shardCastDomainSlowDown = 1;
    [SerializeField] private float shardCastDomainDuration = 8;
    private float spellCastTimer;
    private float spellPerSecond;

    [Header("Time Echo Cast Upgrage")]
    [SerializeField] private int echoToCast = 8;
    [SerializeField] private float echoCastDomainSlow = 1;
    [SerializeField] private float echoCastDomainDuration = 6;
    [SerializeField] private float healthToRestoreWithEcho = .05f;

    [Header("Domain details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        spellPerSecond = GetSpellsToCast() / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObjectDomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if(currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.DomainEchoSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if(upgradeType == SkillUpgradeType.DomainShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if (trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }

    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.DomainSlowingDown)
            return slowDownDomainDuration;
        else if (upgradeType == SkillUpgradeType.DomainShardSpam)
            return shardCastDomainDuration;
        else if (upgradeType == SkillUpgradeType.DomainEchoSpam)
            return echoCastDomainDuration;

        return 0;
    }

    public float GetSlowPercentage()
    {
        if (upgradeType == SkillUpgradeType.DomainSlowingDown)
            return slowDownPercent;
        else if (upgradeType == SkillUpgradeType.DomainShardSpam)
            return shardCastDomainSlowDown;
        else if (upgradeType == SkillUpgradeType.DomainEchoSpam)
            return echoCastDomainSlow;

        return 0;
    }

    private int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.DomainShardSpam)
            return shardsToCast;
        else if (upgradeType == SkillUpgradeType.DomainEchoSpam)
            return echoToCast;

        return 0;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.DomainEchoSpam
            && upgradeType != SkillUpgradeType.DomainShardSpam;
    }

   
    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();

        trappedTargets = new List<Enemy>();
    }
}
