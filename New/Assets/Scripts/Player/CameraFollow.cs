using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
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

}
