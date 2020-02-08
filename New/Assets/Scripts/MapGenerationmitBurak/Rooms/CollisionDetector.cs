using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
   
    [System.NonSerialized]
    public Vector2 entrance;

    private GameObject player;
    private new Camera camera;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        camera = Camera.main;
    } 

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            player.transform.position = entrance;
      
            camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,  camera.transform.position.z) ;
        }
            
    }
}
