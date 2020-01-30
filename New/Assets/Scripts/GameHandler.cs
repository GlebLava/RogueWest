using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerTransform;
    public Joystick jS;

    private void Start()
    {

        cameraFollow.SetUp( () => playerTransform.position);
    }
    private void FixedUpdate()
    {
        cameraFollow.SetCameraFollowPosition(new Vector3(playerTransform.position.x + jS.Direction.x * 3, playerTransform.position.y + jS.Direction.y * 3, playerTransform.position.z));   
    }
}
