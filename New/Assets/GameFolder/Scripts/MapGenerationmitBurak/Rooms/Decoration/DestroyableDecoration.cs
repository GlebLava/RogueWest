using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableDecoration : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public DecorationEventHandler myDecoEventHandler;
  
    int lives = 3;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        InvokeImThere();
    }
    private void FixedUpdate()
    {
            switch (lives)
        {
            case 3:
                obj1.SetActive(true);
                LayerHandler(obj1.GetComponent<SpriteRenderer>());
                obj2.SetActive(false);
                obj3.SetActive(false);
                break;
            case 2:
                obj1.SetActive(false);
                obj2.SetActive(true);
                LayerHandler(obj2.GetComponent<SpriteRenderer>());
                obj3.SetActive(false);
                break;
            case 1:
                obj1.SetActive(false);
                obj2.SetActive(false);
                obj3.SetActive(true);
                LayerHandler(obj3.GetComponent<SpriteRenderer>());
                break;
            default:
                InvokeIGotDestroyed();



                Destroy(gameObject);
                break;

        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            lives--;
        }
    }

    private void LayerHandler(SpriteRenderer spriteRenderer)
    {
        if (player.transform.position.y > transform.position.y + 0.25f)
        {
            spriteRenderer.sortingLayerName = "DecorationInFrontPlayer";
        }else
        {
            spriteRenderer.sortingLayerName = "DecorationBehindPlayer";
        }
    }

    private void InvokeIGotDestroyed()
    {
       myDecoEventHandler.DecorationGotDestroyed(transform.position);
    }
    private void InvokeImThere()
    {
        myDecoEventHandler.DecorationIsThere(transform.position);
    }
}
