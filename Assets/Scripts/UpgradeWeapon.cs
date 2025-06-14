using UnityEngine;

public class UpgradeWeapon : Weapon
{
    protected override void ApplyUpgrade(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            SFXManager.Instance.Play(SFXEvent.PowerupPickup2S);
            playerController.doubleShotEnabled = true;
            Debug.Log("Double Shot Activated!");
        }
    }

    public override void Fire()
    {
        // Optionally implement if this upgrade type needs to fire
    }
}
