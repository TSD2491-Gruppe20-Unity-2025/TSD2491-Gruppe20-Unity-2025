using UnityEngine;

public class DamageHandler : MonoBehaviour
{

    public int health = 1;

    public float invulnerablePeriod = 0;
    float invulnerableTimer = 3;
    int correctLayer;
    public int CurrentHealth => health; // Public getter for health



    private void Start()
    {

        int health = GetComponent<DamageHandler>().CurrentHealth;
        correctLayer = gameObject.layer;
    }
    private void OnTriggerEnter2D()
    {


        health--;
        invulnerableTimer = invulnerablePeriod;
        gameObject.layer = 14;



    }

    private void Update()
    {
        invulnerableTimer -= Time.deltaTime;

        if (invulnerableTimer < 0)
        {
            gameObject.layer = correctLayer;
        }

        if (health <= 0)
        {
            die();
        }





       

    }

    private void die()
    {
        Destroy(gameObject);
    }
}
