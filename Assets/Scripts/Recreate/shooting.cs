using UnityEngine;

public class shooting : MonoBehaviour
{


    //Shooting Variables:

    //private
    private float shootColdownTimer = 0;
    private float shootDelay = 0.5f;
    int bulletLayer;

    //public
    public GameObject bulletPrefab;
    public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);


    private void Start()
    {
        bulletLayer = gameObject.layer;
    }

    private void Update()
    {


        //Shooting:
        shootColdownTimer -= Time.deltaTime;

        if (Input.GetButton("Fire1") && shootColdownTimer <= 0)
        {
            //Shoot
            shootColdownTimer = shootDelay;

            Debug.Log("Pew!");

            Vector3 offset = transform.rotation * bulletOffset;

            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, transform.position + offset, transform.rotation);
            bulletGO.layer = bulletLayer;
        }


    }
}
