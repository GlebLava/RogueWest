using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
   
    [System.NonSerialized]
    public Vector2 entrance;

    private GameObject player;
    private new Camera camera;
    private GameHandler gameHandler;
    
    private void Start()
    {
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        player = GameObject.Find("Player");
        camera = Camera.main;
    } 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameHandler.JumpCameraToPosAndUnfreeze(new Vector3(entrance.x, entrance.y, camera.transform.position.z));
            player.transform.position = entrance;
        }
    }
}
