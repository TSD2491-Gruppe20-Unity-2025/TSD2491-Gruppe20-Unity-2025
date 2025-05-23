using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float detectionRange = 10f;
    public string enemyTag = "Enemy";

    private float lastFireTime = 0f;

    private PlayerController playerOwner; // ✅ Reference to shooter


    void Update()
    {
        GameObject target = FindClosestEnemy();
        if (target != null && Time.time >= lastFireTime + fireRate)
        {
            FireAt(target);
            SFXManager.Instance.Play(SFXEvent.Gun3S);
            lastFireTime = Time.time;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= detectionRange)
            {
                closest = enemy;
                shortestDistance = distance;
            }
        }

        return closest;
    }

    void FireAt(GameObject target)
    {
        if (projectilePrefab == null) return;

        Vector2 direction = (target.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Initialize("Player", playerOwner, direction); // Assume "Player" bullets
        }

        bullet.tag = "PlayerBullet";
    }
}
