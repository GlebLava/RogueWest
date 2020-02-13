using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : RoomClass
{


    public new int roomWidth = 25;
    public new int roomHeight = 15;

    public new int finalRoomWidth;
    public new int finalRoomHeight;


    System.Random prng;


    public SpawnRoom(int seed, int roomWidth, int roomHeight, Vector2 globalPosition, bool leftOpen, bool rightOpen, bool topOpen, bool botOpen, bool active)
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

        Setup(roomGrid);
        Variation(roomGrid);

        leftEntrance(leftOpen, roomGrid);
        rightEntrance(rightOpen, roomGrid);
        topEntrance(topOpen, roomGrid);
        botEntrance(botOpen, roomGrid);
        Floor2(roomGrid);
        MakeColliderBorder(roomGrid);

        roomGrid = BasicSpriteAssigner.AssignBasicSprites(roomGrid);

       

        this.roomGrid = roomGrid;

        return roomGrid;
    }
}
