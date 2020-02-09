using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass
{
    public enum RoomType { Empty, Default, SpawnRoom, }

    public int border = 15;
    public enum RoomSprite { Empty, Border, Floor, Floor2,
        WallTop, WallBot, WallLeft, WallRight, 
        CornerTopLeft, CornerTopRight, CornerBotLeft, CornerBotRight, 
        twoTopBot, twoLeftRight,
        threeBot, threeTop, threeLeft, threeRight, 
        Pillar, BorderWall}

    public struct RoomTileandUnderneath
    {
        public RoomSprite roomSprite;
        public RoomSprite underneath;
    }


    public  int roomWidth = 25;
    public  int roomHeight = 15;

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

    public RoomType roomType;

    public GameObject thisRoom;

    System.Random prng;

    public RoomClass(int seed, int roomWidth, int roomHeight, Vector2 globalPosition, bool leftOpen, bool rightOpen, bool topOpen, bool botOpen, bool active)
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

    public RoomSprite[,] GenerateRoomGrid()
    {

        // Um die eingrenzen zu können wo die eingänge gebaut werden

        int eingrenzungWidth = 2;
        int eingrenzungHeight = 5;


        RoomSprite[,] roomGrid = new RoomSprite[finalRoomWidth, finalRoomHeight];

        Setup();
        Variation();

        leftEntrance(leftOpen);
        rightEntrance(rightOpen);
        topEntrance(topOpen);
        botEntrance(botOpen);
        Floor2();
        MakeColliderBorder();

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

        void Variation()
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

            for (int y = border; y < rHBT; y++)
            {
                int dex = prng.Next(4);

                for (int x = rWBR; x > rWBR - dex + 1; x--)
                {
                    roomGrid[x, y] = RoomSprite.Border;
                }
            }

            for (int y = border; y < rHBT; y++)
            {
                int dex = prng.Next(4);

                for (int x = border; x < border + dex + 1; x++)
                {
                    roomGrid[x, y] = RoomSprite.Border;
                }
            }

        }

        void leftEntrance(bool t)
        {
            
            if (t)
            {
                int entranceMiddl = prng.Next(finalRoomHeight / 2 - eingrenzungWidth, finalRoomHeight / 2 + eingrenzungWidth -1);
                int entranceLowestY = entranceMiddl  - 1;
                int entranceHighestY = entranceMiddl + 1;
                
                for (int x = 0; x < finalRoomWidth / 2; x++)
                {
                    for (int y = entranceLowestY; y < entranceHighestY; y++)
                    {
                        roomGrid[x, y] = RoomSprite.Floor;
                    }
                }

                enterPointLeft = new Vector2(border - 1, entranceMiddl);
             
            }



        }

        void rightEntrance(bool t)
        {
            
            if (t)
            {
                int entranceMiddl = prng.Next(finalRoomHeight / 2 - eingrenzungWidth, finalRoomHeight / 2 + eingrenzungWidth - 1);
                int entranceLowestY = entranceMiddl  - 1;
                int entranceHighestY = entranceMiddl  + 1;
                for (int x = finalRoomWidth/2; x < finalRoomWidth; x++)
                {
                    for (int y = entranceLowestY; y < entranceHighestY; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }
                }

                enterPointRight = new Vector2(finalRoomWidth - border, entranceMiddl);
            }

        }



        void topEntrance(bool t)
        {
            
            if (t)
            {
                int entranceMiddl = prng.Next(finalRoomWidth / 2 - eingrenzungHeight, finalRoomWidth / 2 + eingrenzungHeight - 1);
                int entranceLowestX = entranceMiddl  - 1;
                int entranceHighestX = entranceMiddl  + 1;
                for (int x = entranceLowestX; x < entranceHighestX; x++)
                {
                    for (int y = finalRoomHeight/2; y < finalRoomHeight; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }
                }
  
                enterPointTop = new Vector2(entranceMiddl, finalRoomHeight - border);
            }

        }


        void botEntrance(bool t)
        {
            if (t)
            {
                int entranceMiddl = prng.Next(finalRoomWidth / 2 - eingrenzungHeight, finalRoomWidth/ 2 + eingrenzungHeight - 1);
                int entranceLowestX = entranceMiddl  - 1;
                int entranceHighestX = entranceMiddl  + 1;
                for (int x = entranceLowestX; x < entranceHighestX; x++)
                {
                    for (int y = 0; y < finalRoomHeight / 2; y++)
                    {
                        roomGrid[x, y] = RoomClass.RoomSprite.Floor;
                    }

                }

                enterPointBot = new Vector2(entranceMiddl , border - 1);
            }

        }

        void MakeColliderBorder()
        {
            for (int x = 0; x < finalRoomWidth - 1; x++) roomGrid[x, finalRoomHeight - 1] = RoomClass.RoomSprite.BorderWall;
            for (int x = 0; x < finalRoomWidth - 1; x++) roomGrid[x, 0] = RoomClass.RoomSprite.BorderWall;
            for (int y = 0; y < finalRoomHeight - 1; y++) roomGrid[finalRoomWidth - 1, y] = RoomClass.RoomSprite.BorderWall;
            for (int y = 0; y < finalRoomHeight - 1; y++) roomGrid[0, y] = RoomClass.RoomSprite.BorderWall;
        }

        void Floor2()
        {
            for (int x = border + 3; x < finalRoomWidth - border - 2; x++)
            {
                for (int y = border + 3; y < finalRoomHeight - border - 2; y++)
                {
                    if (roomGrid[x,y] == RoomSprite.Floor && prng.Next(11) < 2)
                    {
                        int cas = prng.Next(11);
                        if (cas < 3 ) roomGrid[x, y] = RoomSprite.Floor2;

                        if (cas  < 5 && cas > 3)
                        {
                            roomGrid[x, y] = RoomSprite.Floor2;
                            roomGrid[x + 1, y] = RoomSprite.Floor2;
                            roomGrid[x, y + 1] = RoomSprite.Floor2;
                            roomGrid[x + 1, y + 1] = RoomSprite.Floor2;

                        }
                        if (cas < 8 && cas > 6)
                        {
                            roomGrid[x, y] = RoomSprite.Floor2;
                            roomGrid[x + 1, y] = RoomSprite.Floor2;
                            roomGrid[x, y + 1] = RoomSprite.Floor2;
                            roomGrid[x + 1, y + 1] = RoomSprite.Floor2;
                            roomGrid[x - 1, y] = RoomSprite.Floor2;
                            roomGrid[x, y - 1] = RoomSprite.Floor2;
                        }
                    }
                }
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
