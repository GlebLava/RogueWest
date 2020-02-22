using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool stopFollow = false;
    private  Func<Vector3> getcameraFollowPositionFunc;

    public void SetUp(Func<Vector3> getcameraFollowPositionFunc)
    {
        this.getcameraFollowPositionFunc = getcameraFollowPositionFunc;
    }
    public void SetCameraFollowPosition(Vector3 cameraFollowPosition)
    {
        SetGetCameraFollowPositionFunc(() => cameraFollowPosition);
    }

    public void SetGetCameraFollowPositionFunc(Func<Vector3> getCameraFollowingPositionFunc)
    {
        this.getcameraFollowPositionFunc = getCameraFollowingPositionFunc;
    }

    public void JumpCameraToAndFreeze(Vector3 jumpToPosition)
    {
        transform.position = jumpToPosition;
        stopFollow = true;
    }

    public void JumpCameraToAndWait(Vector3 jumpToPosition)
    {
        transform.position = jumpToPosition;
        StartCoroutine(StopFollowing(0.25f));
    }

    public void JumpCameraToAndUnFreeze(Vector3 jumpToPosition)
    {
        transform.position = jumpToPosition;
        stopFollow = false;
    }

    public void JumpCameraToAndFreezeFor(Vector3 jumpToPosition, float seconds)
    {
        transform.position = jumpToPosition;
        StartCoroutine(StopFollowing(seconds));
    }


    void Update()
    {
        if (!stopFollow) HandleMovement();
        
    }

    private void HandleMovement()
    {
        Vector3 cameraFollowPosition = getcameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMovementDir = (cameraFollowPosition - transform.position).normalized;
        
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMs = 2f;


        if (distance > 0)
        {
            Vector3 newCameraPosition = transform.position + cameraMovementDir * distance * cameraMs * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

            if (distanceAfterMoving > distance) newCameraPosition = cameraFollowPosition;

            transform.position = newCameraPosition;
        }

    }

    IEnumerator StopFollowing(float seconds)
    {
        stopFollow = true;
        yield return new WaitForSeconds(seconds);
        stopFollow = false;

    }

}
