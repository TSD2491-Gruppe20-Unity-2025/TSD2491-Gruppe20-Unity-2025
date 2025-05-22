//using UnityEngine;

//public class DamageHandler : MonoBehaviour
//{

//    public int health = 1;

//    float invulnerableTimer = 3;
//    int correctLayer;

//    private void Start()
//    {
//        correctLayer = gameObject.layer;
//    }
//    private void OnTriggerEnter2D()
//    {

       
//            health--;
//            invulnerableTimer = 2f;
//            gameObject.layer = 9;
       


//    }

//    private void Update()
//    {
//        invulnerableTimer -= Time.deltaTime;

//        if (invulnerableTimer < 0)
//        {
//            gameObject.layer = correctLayer;
//        }

//        if (health <= 0)
//        {
//            die();
//        }
//    }

//    private void die()
//    {
//        Destroy(gameObject);    
//    }
//}
