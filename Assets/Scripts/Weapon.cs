using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected GameObject projectilePrefab;
    protected Transform firePoint;
    protected string targetTag;
    protected float fireCooldown = 0.1f;
    protected float lastFireTime;

    protected virtual void Awake()
    {
        firePoint = transform;
    }

    protected virtual void Start()
    {
        switch (gameObject.tag)
        {
            case "Player":
                targetTag = "Enemy";
                break;
            case "Enemy":
                targetTag = "Player";
                break;
            default:
                targetTag = "Untagged";
                break;
        }
    }

    public abstract void Fire();

    protected virtual void ApplyUpgrade(GameObject player)
    {
        // Override in derived classes to apply upgrade
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyUpgrade(other.gameObject);
            Destroy(gameObject);
        }
    }
}
