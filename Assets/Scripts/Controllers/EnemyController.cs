using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField]
    public int health;

    public float contactDamageCooldown = 1f;

    protected Dictionary<GameObject, float> lastDamageTime = new();

    public void TakeDamage(int damage, PlayerController attacker)
    {
        health -= damage;
        SFXManager.Instance.Play(SFXEvent.EnemyHitS);
        if (health <= 0)
        {
            Die(attacker);
        }
    }

    protected virtual void Die(PlayerController killer)
    {
        GameController.Instance.RegisterEnemyKill(killer);
        SFXManager.Instance.Play(SFXEvent.EnemyDeathS);
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
                    TakeDamage(1, pc);  // Pass attacker here!
                    lastDamageTime[player] = Time.time;
                }
            }
        }
    }
}
