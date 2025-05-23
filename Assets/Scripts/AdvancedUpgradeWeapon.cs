using UnityEngine;

public class AdvancedUpgradeWeapon : Weapon
{
    public GameObject turretPrefab;

    protected override void ApplyUpgrade(GameObject player)
    {
        if (turretPrefab != null)
        {
            SFXManager.Instance.Play(SFXEvent.PowerupPickup3S);
            Vector3 spawnOffset = new Vector3(0, -0.2f, 0);
            GameObject turret = Instantiate(turretPrefab, player.transform.position + spawnOffset, Quaternion.identity);
            turret.transform.SetParent(player.transform);
            Debug.Log("Turret Deployed!");
        }
    }

    public override void Fire()
    {
        // Optionally implement if this upgrade type needs to fire
    }
}
