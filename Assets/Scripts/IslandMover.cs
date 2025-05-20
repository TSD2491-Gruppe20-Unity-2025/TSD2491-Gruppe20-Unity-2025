using UnityEngine;

public class IslandMover : MonoBehaviour
{
    private float speed;
    private float destroyY;

    public void Init(float fallSpeed, float destroyAtY)
    {
        speed = fallSpeed;
        destroyY = destroyAtY;
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }
}
