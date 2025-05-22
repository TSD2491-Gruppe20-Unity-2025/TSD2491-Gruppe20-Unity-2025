using UnityEngine;

public class AdvancedUpgradeWeapon : MonoBehaviour
{
    public GameObject turretPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform player = other.transform;

            if (turretPrefab != null)
            {
                // Spawn turret slightly above the player's position
                Vector3 spawnOffset = new Vector3(0, -0.2f, 0); // Adjust Y as needed
                GameObject turret = Instantiate(turretPrefab, player.position + spawnOffset, Quaternion.identity);

                turret.transform.SetParent(player); // Attach to player
            }

            Destroy(gameObject); // Destroy pickup
        }
    }
}
