using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Move Variables:

    //private:
    private float ShipRadius = 0.1f;

    //public:
    public float maxSpeed = 10f;

    



    

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

    //-------------------------------------------------//
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




        //MOVE SHIP:

        //returns flote -1 to +1
        Input.GetAxis("Vertical");
        Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;

        pos.y += maxSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        pos.x += maxSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.position = pos;



        //restrict player to the camera view



        //Restrict Vertical bounds
        if (pos.y + ShipRadius >= Camera.main.orthographicSize)
        {
            pos.y = Camera.main.orthographicSize - ShipRadius;
        }

        if (pos.y - ShipRadius <= -Camera.main.orthographicSize)
        {
            pos.y = -Camera.main.orthographicSize + ShipRadius;
        }


        //calculate ortographic width based on screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;


        //Restrict Horizontal bounds
        if (pos.x + ShipRadius >= widthOrtho)
        {
            pos.x = widthOrtho - ShipRadius;
        }

        if (pos.x - ShipRadius <= -widthOrtho)
        {
            pos.x = -widthOrtho + ShipRadius;
        }


        //update position
        transform.position = pos;
    }
}
