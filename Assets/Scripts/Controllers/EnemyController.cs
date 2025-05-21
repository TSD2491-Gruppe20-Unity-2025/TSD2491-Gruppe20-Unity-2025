using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public int health = 1;
    private Dictionary<GameObject, float> lastDamageTime = new();
    public float contactDamageCooldown = 1f;

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
     
        // Touched by player
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            if (!lastDamageTime.ContainsKey(player)) lastDamageTime[player] = -100f;

            if (Time.time - lastDamageTime[player] >= contactDamageCooldown)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null)
                {
                    //pc.TakeDamage(1);   // Damage the player
                    TakeDamage(1);      // Damage the enemy
                    lastDamageTime[player] = Time.time;
                }
            }
        }
    }
}
