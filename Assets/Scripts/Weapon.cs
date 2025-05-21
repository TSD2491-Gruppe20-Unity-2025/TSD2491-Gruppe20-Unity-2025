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
        firePoint = transform; // default firePoint if none assigned
    }

    protected virtual void Start()
    {
        // Automatically assign target tag based on object's own tag
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
}
