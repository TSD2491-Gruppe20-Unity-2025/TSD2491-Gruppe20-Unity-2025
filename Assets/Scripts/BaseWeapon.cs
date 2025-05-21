using UnityEngine;

public class BaseWeapon : Weapon
{
    [SerializeField] private GameObject projectilePrefabOverride;
    [SerializeField] private Transform firePointOverride;
    [SerializeField] private float overrideCooldown = 0.1f;

    public string ownerTag;        // Tag of the shooter ("Player" or "EnemyBasic")
    public string projectileTag;   // Tag to assign to projectile ("PlayerBullet", "EnemyBullet")
    public Transform FirePoint => firePoint;


    protected override void Awake()
    {
        base.Awake();
        if (firePointOverride != null)
            firePoint = firePointOverride;

        fireCooldown = overrideCooldown;
        projectilePrefab = projectilePrefabOverride;

        ownerTag = gameObject.tag;

        // Set projectileTag based on ownerTag by default
        if (ownerTag == "Player")
            projectileTag = "PlayerBullet";
        else if (ownerTag == "Enemy")
            projectileTag = "EnemyBullet";
        else
            projectileTag = "Untagged";
    }

    public override void Fire()
{
    Fire(null);
}

public void Fire(Vector2? directionOverride)
{
    if (Time.time < lastFireTime + fireCooldown) return;
    if (projectilePrefab == null || firePoint == null) return;

    GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    Projectile proj = bullet.GetComponent<Projectile>();

    if (proj != null)
    {
        proj.Initialize(ownerTag, directionOverride);
    }

    bullet.tag = projectileTag;
    lastFireTime = Time.time;
}
}


