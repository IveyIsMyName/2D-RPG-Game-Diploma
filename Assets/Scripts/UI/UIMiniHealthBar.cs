using UnityEngine;

public class UIMiniHealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();

    private void OnEnable()
    {
        entity.OnFlipped += HadleFlip;
    }

    private void HadleFlip() => transform.rotation = Quaternion.identity;

    private void OnDisable()
    {
        entity.OnFlipped -= HadleFlip;
    }

}
