using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Enemy Configuration

    public int health = 1;
    public float contactDamageCooldown = 1f;

    //-----------------------------------------------------------------------------//
    // Private Fields

    private Dictionary<GameObject, float> lastDamageTime = new();

    //-----------------------------------------------------------------------------//
    // Damage Handling

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy hit! Remaining HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    //-----------------------------------------------------------------------------//
    // Collision Handling

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        HandleHit(other);
    }

    private void HandleHit(Collider2D other)
    {
        // Handle player collision
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            if (!lastDamageTime.ContainsKey(player))
                lastDamageTime[player] = -100f;

            if (Time.time - lastDamageTime[player] >= contactDamageCooldown)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    // Optional: damage the player
                    // pc.TakeDamage(1);

                    // Self-damage on contact
                    TakeDamage(1);

                    lastDamageTime[player] = Time.time;
                }
            }
        }
    }
}
