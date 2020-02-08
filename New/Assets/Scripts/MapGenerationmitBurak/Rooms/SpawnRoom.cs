using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : RoomClass
{


    public new int roomWidth = 40;
    public new int roomHeight = 25;

    public new int finalRoomWidth;
    public new int finalRoomHeight;


    System.Random prng;

    
    public SpawnRoom(int seed, Vector2 globalPosition, bool leftOpen, bool rightOpen, bool topOpen, bool botOpen, bool active, GameObject thisRoom) 
        : base(seed, globalPosition, leftOpen, rightOpen, topOpen, botOpen, active, thisRoom)
    {
       prng = new System.Random(seed);
       finalRoomWidth = roomWidth + border * 2;
        finalRoomHeight = roomHeight + border * 2;
    }
    
    public new RoomClass.RoomSprite[,] GenerateRoomGrid()
    {
        RoomClass.RoomSprite[,] roomGrid = new RoomClass.RoomSprite[finalRoomWidth, finalRoomHeight];

        Setup();

        Test();

        leftEntrance(leftOpen);
        rightEntrance(rightOpen);
        topEntrance(topOpen);
        botEntrance(botOpen);

        roomGrid = BasicSpriteAssigner.AssignBasicSprites(roomGrid);

        

        void Setup()
        {

            for (int x = 0; x < finalRoomWidth; x++)
            {
                for (int y = 0; y < finalRoomHeight; y++)
                {

                    if (BorderCheck(x, y)) roomGrid[x, y] = RoomClass.RoomSprite.Border;
                    else roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                }
            }
        }

        void Test()
        {
            int rHBT = finalRoomHeight - border;
            int rWBR = finalRoomWidth - border;

            for (int x = border; x < rWBR; x++)
            {
                int dex = prng.Next(4);

                for (int y = rHBT; y > rHBT - dex; y--)
                {
                    roomGrid[x, y] = RoomSprite.Border;
                }
            }

            for (int x = border; x < rWBR; x++)
            {
                int dex = prng.Next(4);

                for (int y = border; y < border + dex + 1; y++)
                {
                    roomGrid[x, y] = RoomSprite.Border;
                }
            }
        }

        void leftEntrance(bool t)
        {
            int entranceLowestY = finalRoomHeight / 2 - 1;
            int entranceHighestY = finalRoomHeight / 2 + 1;
            if (t)
            {
                for (int x = 0; x < border; x++)
                {
                    for (int y = entranceLowestY; y < entranceHighestY; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Border;
                    }
                }

                enterPointLeft = new Vector2(border - 1, finalRoomHeight / 2);
            }



        }

        void rightEntrance(bool t)
        {
            int entranceLowestY = finalRoomHeight / 2 - 1;
            int entranceHighestY = finalRoomHeight / 2 + 1;
            if (t)
            {
                for (int x = roomWidth + border - 1; x < finalRoomWidth; x++)
                {
                    for (int y = entranceLowestY; y < entranceHighestY; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }
                }

                enterPointRight = new Vector2(finalRoomWidth - border, finalRoomHeight / 2);
            }

        }



        void topEntrance(bool t)
        {
            int entranceLowestX = finalRoomWidth / 2 - 1;
            int entranceHighestX = finalRoomWidth / 2 + 1;
            if (t)
            {
                for (int x = entranceLowestX; x < entranceHighestX; x++)
                {
                    for (int y = roomHeight; y < finalRoomHeight; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }
                }

                enterPointTop = new Vector2(finalRoomWidth / 2, finalRoomHeight - border);
            }

        }


        void botEntrance(bool t)
        {
            int entranceLowestX = finalRoomWidth / 2 - 1;
            int entranceHighestX = finalRoomWidth / 2 + 1;
            if (t)
            {
                for (int x = entranceLowestX; x < entranceHighestX; x++)
                {
                    for (int y = 0; y < border; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }
                    
                }

                enterPointBot = new Vector2(finalRoomWidth / 2, border - 1);
            }

        }
        this.roomGrid = roomGrid;

        return roomGrid;
    }

    public new bool BorderCheck(int x, int y) //returns True if the given position is within the border
    {

        return (x < border || x > finalRoomWidth - border - 1 || y < border || y > finalRoomHeight - border - 1);
    }
}
