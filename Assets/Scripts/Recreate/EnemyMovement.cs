using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType { Straight, ZigZag, SineWave }
    public MovementType movementType = MovementType.Straight;

    public float speed = 2f;
    public float frequency = 2f; // for sine and zigzag
    public float magnitude = 1f; // for sine and zigzag

    private Vector3 startPosition;
    private float timeSinceSpawn;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        switch (movementType)
        {
            case MovementType.Straight:
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;

            case MovementType.ZigZag:
                float zigzagX = Mathf.PingPong(timeSinceSpawn * frequency, magnitude * 2) - magnitude;
                transform.position = startPosition + new Vector3(zigzagX, -speed * timeSinceSpawn, 0);
                break;

            case MovementType.SineWave:
                float sineX = Mathf.Sin(timeSinceSpawn * frequency) * magnitude;
                transform.position = startPosition + new Vector3(sineX, -speed * timeSinceSpawn, 0);
                break;
        }
    }
}
