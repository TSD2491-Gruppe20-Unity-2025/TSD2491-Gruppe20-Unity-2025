using UnityEngine;

public class UpgradeWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.doubleShotEnabled = true;
                Debug.Log("Double Shot Activated!");
            }

            Destroy(gameObject); // Remove upgrade pickup
        }
    }
}
