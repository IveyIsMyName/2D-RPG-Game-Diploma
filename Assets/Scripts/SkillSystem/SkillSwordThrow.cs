using UnityEngine;

public class SkillSwordThrow : SkillBase
{
    private SkillObjectSword currentSword;
    private float currentThrowPower;

    [Header("Regular Sword Upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0, 10)]
    [SerializeField] private float regularThrowPower = 5;


    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int amountToPierce = 2;
    [Range(0, 10)]
    [SerializeField] private float pirceThrowPower = 5;

    [Header("Spin Sword Upgrage")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance = 5;
    public float attackPerSecond = 6;
    public float maxSpinDuration = 3;
    [Range(0, 10)]
    [SerializeField] private float spinThrowPower = 5;

    [Header("Bounce Sword Upgrade")]
    [SerializeField] private GameObject bounceSwordPrefab;
    public int bounceCount = 5;
    public float bounceSpeed = 12;
    [Range(0, 10)]
    [SerializeField] private float bounceThrowPower = 5;


    [Header("Trajectoty prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = .05f;
    private float swordGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();

        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();
        
        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSword()
    {
        GameObject swordPrefab = GetSwordPrefab();
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObjectSword>();
        currentSword.SetupSword(this, GetThrowPower());

        SetSkillOnCooldown();
    }

    private GameObject GetSwordPrefab()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return swordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrowPierce))
            return pierceSwordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrowSpin))
            return spinSwordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrowBounce))
            return bounceSwordPrefab;

        return null;
    }

    private void UpdateThrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.SwordThrow:
                currentThrowPower = regularThrowPower;
                break;
            case SkillUpgradeType.SwordThrowPierce:
                currentThrowPower = pirceThrowPower;
                break;
            case SkillUpgradeType.SwordThrowSpin:
                currentThrowPower = spinThrowPower;
                break;
            case SkillUpgradeType.SwordThrowBounce:
                currentThrowPower = bounceThrowPower;
                break;

        }
    }

    private Vector2 GetThrowPower() => confirmedDirection * (currentThrowPower * 10);

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentThrowPower * 10;

        Vector2 initialVelocity = direction * scaledThrowPower; //Initial velocity

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t); //Gravity

        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect; //How far the sword flies

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
