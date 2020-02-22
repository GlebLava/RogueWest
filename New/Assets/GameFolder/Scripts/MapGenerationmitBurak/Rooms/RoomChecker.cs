using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    private GameHandler gameHandler;
    private bool playerIsInRoom;
    public RoomClass ParentRoom;

    private float paddingX = 0f;
    private float paddingY = 2.5f;
    void Start()
    {
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        
    }

    public void Update()
    {
        if (playerIsInRoom) gameHandler.ClampCamera(ParentRoom.globalPosition.x + RoomClass.border - paddingX,  ParentRoom.globalPosition.x + ParentRoom.finalRoomWidth - RoomClass.border + paddingX, 
            ParentRoom.globalPosition.y +  RoomClass.border - paddingY, ParentRoom.globalPosition.y + ParentRoom.finalRoomHeight - RoomClass.border + paddingY);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {

            playerIsInRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsInRoom = false;
        }
    }
}


