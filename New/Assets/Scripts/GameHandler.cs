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
        cameraFollow.SetCameraFollowPosition(new Vector3(player.transform.position.x+ player.GetComponent<Player>().Dir.x + jS.Direction.x * 3
            , player.GetComponent<Player>().Dir.y + player.transform.position.y + jS.Direction.y * 3
            , player.transform.position.z));   
    }

    public void JumpCameraToPosAndUnfreeze(Vector3 newPosition)
    {
        cameraFollow.JumpCameraToAndUnFreeze(newPosition);
    }

    public void FreezeCamera()
    {
        cameraFollow.stopFollow = true;
    }

    public void UnFreezeCamera()
    {
        cameraFollow.stopFollow = false;
    }

    public void ClampCamera(float clampWidthMin, float clampWidthMax,float clampHeightMin, float clampHeightMax)
    {
        Vector3 cameraClampPosition = new Vector3(Mathf.Clamp( cameraFollow.transform.position.x, clampWidthMin, clampWidthMax), Mathf.Clamp(cameraFollow.transform.position.y, clampHeightMin, clampHeightMax), cameraFollow.transform.position.z );
        cameraFollow.transform.position = cameraClampPosition;
    }
}
