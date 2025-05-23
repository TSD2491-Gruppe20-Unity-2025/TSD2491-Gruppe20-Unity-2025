using UnityEngine;

public class BaseWeapon : Weapon
{
    //-----------------------------------------------------------------------------//
    // Inspector Variables

    [SerializeField] private GameObject projectilePrefabOverride;
    [SerializeField] private Transform firePointOverride;
    [SerializeField] private float overrideCooldown = 0.1f;

    //-----------------------------------------------------------------------------//
    // Public Properties

    public string ownerTag;        // Tag of the shooter (e.g., "Player" or "Enemy")
    public string projectileTag;   // Tag to assign to the projectile (e.g., "PlayerBullet")
    public Transform FirePoint => firePoint;
    private PlayerController playerOwner; // ✅ Reference to shooter

    //-----------------------------------------------------------------------------//
    // Unity Methods

    protected override void Awake()
    {
        base.Awake();

        // Apply overrides if assigned
        if (firePointOverride != null)
            firePoint = firePointOverride;

        fireCooldown = overrideCooldown;
        projectilePrefab = projectilePrefabOverride;

        ownerTag = gameObject.tag;

        // Determine projectile tag based on owner
        if (ownerTag == "Player")
            projectileTag = "PlayerBullet";
        else if (ownerTag == "Enemy")
            projectileTag = "EnemyBullet";
        else
            projectileTag = "Untagged";
    }

    //-----------------------------------------------------------------------------//
    // Firing Logic

    public override void Fire()
    {
        SFXManager.Instance.Play(SFXEvent.PrimaryFireS);
        Fire(null);
    }

    public void Fire(Vector2? directionOverride)
    {

        // Cooldown check
        if (Time.time < lastFireTime + fireCooldown) return;
        if (projectilePrefab == null || firePoint == null) return;

        // Spawn and initialize projectile
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile proj = bullet.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Initialize(ownerTag, playerOwner, directionOverride);
        }

        bullet.tag = projectileTag;
        lastFireTime = Time.time;
    }


    public void FireDouble()
    {
        if (Time.time < lastFireTime + fireCooldown) return;
        if (projectilePrefab == null || firePoint == null) return;

        Vector3 leftOffset = firePoint.position + new Vector3(-0.3f, 0, 0);
        Vector3 rightOffset = firePoint.position + new Vector3(0.3f, 0, 0);
        playerOwner = GetComponent<PlayerController>();


        // Left bullet
        GameObject bullet1 = Instantiate(projectilePrefab, leftOffset, Quaternion.identity);
        Projectile proj1 = bullet1.GetComponent<Projectile>();
        if (proj1 != null) proj1.Initialize(ownerTag, playerOwner, null);
        bullet1.tag = projectileTag;

        // Right bullet
        GameObject bullet2 = Instantiate(projectilePrefab, rightOffset, Quaternion.identity);
        Projectile proj2 = bullet2.GetComponent<Projectile>();
        if (proj2 != null) proj2.Initialize(ownerTag, playerOwner, null);
        bullet2.tag = projectileTag;

        SFXManager.Instance.Play(SFXEvent.Gun2S);
        
        lastFireTime = Time.time;
    }

}
