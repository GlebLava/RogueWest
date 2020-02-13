using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRoom : RoomClass
{

    System.Random prng;

    private Vector2 chestLocation; public Vector2 ChestLocation { get => chestLocation; }
    public ChestRoom(int seed, int roomWidth, int roomHeight, Vector2 globalPosition, bool leftOpen, bool rightOpen, bool topOpen, bool botOpen, bool active)
        : base(seed, roomWidth, roomHeight, globalPosition, leftOpen, rightOpen, topOpen, botOpen, active)
    {
        this.globalPosition = globalPosition;
        this.leftOpen = leftOpen;
        this.rightOpen = rightOpen;
        this.topOpen = topOpen;
        this.botOpen = botOpen;
        this.active = active;
        this.roomWidth = roomWidth;
        this.roomHeight = roomHeight;

        prng = new System.Random(seed);

        finalRoomWidth = roomWidth + border * 2;
        finalRoomHeight = roomHeight + border * 2;

    }

    public new RoomClass.RoomTile[,] GenerateRoomGrid()
    {
        RoomClass.RoomTile[,] roomGrid = new RoomClass.RoomTile[finalRoomWidth, finalRoomHeight];

        this.roomGrid = roomGrid;

        Setup(roomGrid);
        Variation(roomGrid);

        leftEntrance(leftOpen, roomGrid);
        rightEntrance(rightOpen, roomGrid);
        topEntrance(topOpen, roomGrid);
        botEntrance(botOpen, roomGrid);
        Floor2(roomGrid);
        MakeColliderBorder(roomGrid);

        roomGrid = BasicSpriteAssigner.AssignBasicSprites(roomGrid);

        SetChestLocation();

        return roomGrid;
    }

    private void SetChestLocation()
    {
        Vector2 middleOfRoom = new Vector2(finalRoomWidth / 2, finalRoomHeight / 2);
        chestLocation = middleOfRoom;
    }

   
}

