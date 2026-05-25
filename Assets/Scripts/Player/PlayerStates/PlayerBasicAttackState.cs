using UnityEngine;

public class PlayerBasicAttackState : PlayerState
{
    private float attackVelocityTimer;

    private bool comboAttackQueued;
    private int attackDir;
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;

    private float lastTimeAttacked;
    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
            comboLimit = player.attackVelocity.Length;
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndex();
        SyncAttackSpeed();

        attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;
        
        anim.SetInteger("basicAttackIndex", comboIndex);
        attackVelocityTimer = player.attackVelocityDuration;
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
    }

    public override void Exit()
    {
        base.Exit();

        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }

    private void ResetComboIndex()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }
}
