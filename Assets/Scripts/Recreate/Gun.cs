using UnityEngine;

public class Gun : MonoBehaviour
{

    float maxSpeed = 5f;
    private void Update()
    {
        Vector3 pos = transform.position;

        Vector3 velosity = new Vector3(0 ,  maxSpeed * Time.deltaTime, 0);

        pos += transform.rotation * velosity;

        transform.position = pos;
    }
}
