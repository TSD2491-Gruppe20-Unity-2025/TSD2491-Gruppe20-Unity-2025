using UnityEngine;

public class IslandMover : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Private Fields

    private float speed;
    private float destroyY;

    //-----------------------------------------------------------------------------//
    // Initialization

    public void Init(float fallSpeed, float destroyAtY)
    {
        speed = fallSpeed;
        destroyY = destroyAtY;
    }

    //-----------------------------------------------------------------------------//
    // Unity Methods

    void Update()
    {
        // Move the island downward
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Destroy the island once it moves below the threshold
        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }
}