using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public float enterSpeed = 1.5f;
    public float moveSpeed = 2f;
    public float moveRange = 6f;

    private BaseWeapon baseWeapon;
    private float fireInterval = 0.5f;
    private float nextFireTime;
    private Vector3 startPos;
    private bool hasEntered = false;
    private float direction = 1f;

    

    void Start()
    {
        startPos = transform.position;
        baseWeapon = GetComponent<BaseWeapon>();
        nextFireTime = Time.time + fireInterval;
    }

    void Update()
    {
        if (!hasEntered) //Bossen skal fly nedover til den er helt inne i skjermen, så fly side til side under bossfighten
        {
            transform.Translate(Vector2.down * enterSpeed * Time.deltaTime);

            if (transform.position.y <= Camera.main.ViewportToWorldPoint(new Vector2(0, 0.8f)).y)
            {
                hasEntered = true;
                startPos = transform.position;
            }
        }
        else
        {
            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            if (Mathf.Abs(transform.position.x - startPos.x) > moveRange)
                direction *= -1f;
            if (baseWeapon != null && Time.time >= nextFireTime)
            {   GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector2 directionToPlayer = (player.transform.position - baseWeapon.FirePoint.position).normalized;
                baseWeapon.Fire(directionToPlayer);
                nextFireTime = Time.time + fireInterval;
                
                }
        }
    }
}
