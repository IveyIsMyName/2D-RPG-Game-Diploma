public enum SkillUpgradeType
{
    None,

    //----- Dash Tree -----
    Dash, //Dash to avoid damage
    DashCloneOnStart, //Create a clone when dash starts
    DashCloneStartAndArrival, //Create a clone when dash starts and ends
    DashShardOnStart, //Create a shard when dash starts
    DashShardOnStartAndArrival, // Create a shard when dash start and ends

    //----- Shard Tree -----
    Shard, //The shard explodes when touched by an enemy or time goes up
    ShardMoveToEnemy, //Shard moves towards nearest enemy
    ShardMultiCast, //Shard ability can have up to N charges. You can cast them all in a raw
    ShardTeleport, //You can swap places with the last shard you created
    ShardTeleportHPRewind, //When you swap places with shard, your HP % is the same as it was when you created shard.

    //----- Sword Tree -----
    SwordThrow, //You can throw sword to damage enemies from range
    SwordThrowSpin, //Your sword will spin at one point and damage enemies
    SwordThrowPierce, //Your sword will pierce through N targets
    SwordThrowBounce, //Sword will bounce between enemies

    //----- Time Echo -----
    TimeEcho, //Create a clone of a player. It can take damage from enemies
    TimeEchoSingleAttack, //Time echo can perform a single attack
    TimeEchoMultiAttack, //Time echo can perform N attacks
    TimeEchoChanceToDuplicate, //Time echo has a chance to create another time echo when attacking
    TimeEchoHealWisp, //When time echo dies, it creates a wisp that flies towards the player and heal it. 
                      //Heal is the percantage of damage taken when died
    TimeEchoCleanseWisp, //Wisp will now remove all negative effect from player
    TimeEchoCooldownWisp, //Wisp will reduce cooldown of all skills by N second

    // ----- Domain Expansion ----
    DomainSlowingDown, //Create an area in which you slow down enemies by 90%-100%
    DomainEchoSpam, //You can no longer move, but you spam enemies with Time Echo ability
    DomainShardSpam //You can no longer move, but you spam enemies with Time Shard ability




}
