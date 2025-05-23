using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public int healAmount = 3; // Change this value as needed

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected item!");

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healAmount);
                SFXManager.Instance.Play(SFXEvent.PowerupPickupS);
            }

            Destroy(gameObject);
        }
    }
}
