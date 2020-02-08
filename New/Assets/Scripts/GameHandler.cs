using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public GameObject player;
    public Joystick jS;

    private void Start()
    {

        cameraFollow.SetUp( () => player.transform.position);
    }
    private void FixedUpdate()
    {
        cameraFollow.SetCameraFollowPosition(new Vector3(player.transform.position.x+ player.GetComponent<PlayerMovement>().dir.x + jS.Direction.x * 3
            , player.GetComponent<PlayerMovement>().dir.y + player.transform.position.y + jS.Direction.y * 3
            , player.transform.position.z));   
    }
}
