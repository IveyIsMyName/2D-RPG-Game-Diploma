using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = .2f;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVFXColor = Color.white;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject critHitVFX;

    [Header("Element Colors")]
    [SerializeField] private Color chillVFX = Color.cyan;
    [SerializeField] private Color burnVFX = Color.red;
    [SerializeField] private Color shockVFX = Color.yellow;
    private Color originalHitVFXColor;

    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        originalHitVFXColor = hitVFXColor;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, ElementType element)
    {
        GameObject hitPrefab = isCrit ? critHitVFX : hitVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        //vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);

        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice: return chillVFX;
            case ElementType.Fire: return burnVFX;
            case ElementType.Lightning: return shockVFX;

            default: return Color.white;
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);

        onDamageVFXCoroutine = StartCoroutine(onDamageVFXCo());
    }

    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if(element == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCo(duration, chillVFX));

        if(element == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCo(duration, burnVFX));
        
        if(element == ElementType.Lightning)
            StartCoroutine(PlayStatusVFXCo(duration, shockVFX));
    }

    public void StopAllVFX()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator onDamageVFXCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVFXDuration);

        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVFXCo(float duration, Color colorEffect)
    {
        float tickInterval = .25f;
        float timeHasPassed = 0;

        Color lightColor = colorEffect * 1.2f;
        Color darkColor = colorEffect * .9f;

        bool toggle = false;

        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);

            timeHasPassed = timeHasPassed + tickInterval;
        }

        sr.color = Color.white;
    }
}
