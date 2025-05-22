using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Protected Fields

    protected GameObject projectilePrefab;
    protected Transform firePoint;
    protected string targetTag;
    protected float fireCooldown = 0.1f;
    protected float lastFireTime;

    //-----------------------------------------------------------------------------//
    // Unity Methods

    protected virtual void Awake()
    {
        // Set default firePoint if not overridden
        firePoint = transform;
    }

    protected virtual void Start()
    {
        // Assign target tag based on object's own tag
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

    //-----------------------------------------------------------------------------//
    // Abstract Method

    public abstract void Fire();
}