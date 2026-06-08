using UnityEngine;

public class ObjectChest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private EntityVFX fx => GetComponent<EntityVFX>();
    private EntityDropManager dropManager => GetComponent<EntityDropManager>();
    [Header("Open Details")]
    [SerializeField] private bool canDropItems = true;
    [SerializeField] private Vector2 knockback;
    public void TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (canDropItems == false)
            return;

        canDropItems = false;
        dropManager?.DropItems();
        fx.PlayOnDamageVFX();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);

    }
}
