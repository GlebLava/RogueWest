using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableDecoration : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;


    int lives = 3;

    private void FixedUpdate()
    {
            switch (lives)
        {
            case 3:
                obj1.SetActive(true);
                obj2.SetActive(false);
                obj3.SetActive(false);
                break;
            case 2:
                obj1.SetActive(false);
                obj2.SetActive(true);
                obj3.SetActive(false);
                break;
            case 1:
                obj1.SetActive(false);
                obj2.SetActive(false);
                obj3.SetActive(true);
                break;
            default:
                Destroy(gameObject);
                break;

        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            lives--;
        }
    }
}
