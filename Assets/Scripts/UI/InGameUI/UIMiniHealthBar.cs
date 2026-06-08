using UnityEngine;

public class UIMiniHealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();

    private void OnEnable()
    {
        if (entity == null)
            return;

        entity.OnFlipped += HandleFlip;
    }

    private void HandleFlip() => transform.rotation = Quaternion.identity;

    private void OnDisable()
    {
        if (entity == null)
            return;

        entity.OnFlipped -= HandleFlip;
    }

}
