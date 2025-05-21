using UnityEngine;

public class EnemyBasic : EnemyController
{
    public float speed = 2f;

    void Start()
    {
        health = 1;
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1f)
            Destroy(gameObject);
    }
}
