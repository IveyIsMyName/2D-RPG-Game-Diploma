using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private EntityVFX fx => GetComponent<EntityVFX>();
    public void TakeDamage(float damage, Transform damageDealer)
    {
        fx.PlayOnDamageVFX();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = new Vector2(0, 3);
        rb.angularVelocity = Random.Range(-200f, 200f);

    }
}
