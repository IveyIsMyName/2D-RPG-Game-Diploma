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
    ShardTeleportHPRewind //When you swap places with shard, your HP % is the same as it was when you created shard.
}
