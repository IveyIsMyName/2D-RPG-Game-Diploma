using UnityEngine;

public class SkillObjectAnimationTriggers : MonoBehaviour
{
    private SkillObjectTimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObjectTimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    private void TryToTerminate(int currentAttackIndex)
    {
        if (currentAttackIndex == timeEcho.maxAttacks)
            timeEcho.HandleDeath();
    }
}
