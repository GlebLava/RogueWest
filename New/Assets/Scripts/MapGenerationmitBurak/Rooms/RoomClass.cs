using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass
{

    public int border = 15;
    public enum RoomSprite { Empty, Border, Floor, WallTop, WallBot, WallLeft, WallRight, CornerTopLeft, CornerTopRight, CornerBotLeft, CornerBotRight, twoTopBot, twoLeftRight
    , threeBot, threeTop, threeLeft, threeRight, Pillar}



    public  int roomWidth = 40;
    public  int roomHeight = 25;

    public int finalRoomWidth;
    public int finalRoomHeight;

    public bool leftOpen;
    public bool rightOpen;
    public bool topOpen;
    public bool botOpen;

    public bool active = false; //false by default
    public Vector2 globalPosition;

    public Vector2 enterPointLeft;
    public Vector2 enterPointRight;
    public Vector2 enterPointTop;
    public Vector2 enterPointBot;

    public RoomSprite[,] roomGrid;

    private GameObject thisRoom; public GameObject ThisRoom { get => thisRoom; }

    public RoomClass(int seed, Vector2 globalPosition, bool leftOpen, bool rightOpen, bool topOpen, bool botOpen, bool active, GameObject thisRoom)
    {
        this.globalPosition = globalPosition;
        this.leftOpen = leftOpen;
        this.rightOpen = rightOpen;
        this.topOpen = topOpen;
        this.botOpen = botOpen;
        this.active = active;
        this.thisRoom = thisRoom;

        System.Random prng = new System.Random(seed);

        finalRoomWidth = roomWidth + border * 2;
        finalRoomHeight = roomHeight + border * 2;

    }

    public RoomSprite[,] GenerateRoomGrid()
    {

        RoomSprite[,] roomGrid = new RoomSprite[finalRoomWidth, finalRoomHeight];

        Setup();

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

                    if (BorderCheck(x, y)) roomGrid[x, y] = RoomSprite.Border;
                    else roomGrid[x, y] = RoomSprite.Floor;
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
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
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

        public bool BorderCheck(int x, int y) //returns True if the given position is within the border
        {

            return (x < border || x > finalRoomWidth - border - 1 || y < border || y > finalRoomHeight - border - 1);
        }

    }
