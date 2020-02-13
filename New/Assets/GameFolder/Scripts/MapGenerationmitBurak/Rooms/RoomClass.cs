using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass
{
    public enum RoomType { Empty, Default, SpawnRoom, ChestRoom }

    public const int border = 12;

    public enum RoomSprite
    {
        Empty, Border, Floor, Floor2,
        WallTop, WallBot, WallLeft, WallRight,
        CornerTopLeft, CornerTopRight, CornerBotLeft, CornerBotRight,
        twoTopBot, twoLeftRight,
        threeBot, threeTop, threeLeft, threeRight,
        Pillar, BorderWall
    }

    public enum RoomSpriteBelow
    {
        Empty, Floor, Floor2
    }

    public struct RoomTile
    {
        public RoomSprite roomSpriteAbove;
        public RoomSpriteBelow roomSpriteUnderneath;
    }


    public int roomWidth = 25;
    public int roomHeight = 15;

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

    public RoomTile[,] roomGrid;

    public RoomType roomType;

    public GameObject thisRoom;


    private int middleWidth;
    private int middleHeight;
    System.Random prng; public System.Random Prng { get => prng; }

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

        middleWidth = finalRoomWidth / 2;
        middleHeight = finalRoomHeight / 2;


    }

    // Um die eingrenzen zu können wo die eingänge gebaut werden
    int eingrenzungWidth = 2;
    int eingrenzungHeight = 5;


    public RoomTile[,] GenerateRoomGrid()
    {






        RoomTile[,] roomGrid = new RoomTile[finalRoomWidth, finalRoomHeight];

        Setup(roomGrid);
        Variation(roomGrid);

        leftEntrance(leftOpen, roomGrid);
        rightEntrance(rightOpen, roomGrid);
        topEntrance(topOpen, roomGrid);
        botEntrance(botOpen, roomGrid);
        InRoomStructure(roomGrid);
        Floor2(roomGrid);
        MakeColliderBorder(roomGrid);

        roomGrid = BasicSpriteAssigner.AssignBasicSprites(roomGrid);



        this.roomGrid = roomGrid;
        return roomGrid;
    }

    public void Setup(RoomTile[,] grid_)
    {

        for (int x = 0; x < finalRoomWidth; x++)
        {
            for (int y = 0; y < finalRoomHeight; y++)
            {

                if (BorderCheck(x, y)) grid_[x, y].roomSpriteAbove = RoomSprite.Border;
                else grid_[x, y].roomSpriteAbove = RoomSprite.Floor;
            }
        }

    }

    public void Variation(RoomTile[,] grid_)
    {
        int rHBT = finalRoomHeight - border;
        int rWBR = finalRoomWidth - border;

        for (int x = border; x < rWBR; x++)
        {
            int dex = prng.Next(4);

            for (int y = rHBT; y > rHBT - dex; y--)
            {
                grid_[x, y].roomSpriteAbove = RoomSprite.Border;
            }
        }

        for (int x = border; x < rWBR; x++)
        {
            int dex = prng.Next(4);

            for (int y = border; y < border + dex + 1; y++)
            {
                grid_[x, y].roomSpriteAbove = RoomSprite.Border;
            }
        }

        for (int y = border; y < rHBT; y++)
        {
            int dex = prng.Next(4);

            for (int x = rWBR; x > rWBR - dex + 1; x--)
            {
                grid_[x, y].roomSpriteAbove = RoomSprite.Border;
            }
        }

        for (int y = border; y < rHBT; y++)
        {
            int dex = prng.Next(4);

            for (int x = border; x < border + dex + 1; x++)
            {
                grid_[x, y].roomSpriteAbove = RoomSprite.Border;
            }
        }

    }

    public void leftEntrance(bool t, RoomTile[,] grid_)
    {

        if (t)
        {
            int entranceMiddl = prng.Next(finalRoomHeight / 2 - eingrenzungWidth, finalRoomHeight / 2 + eingrenzungWidth - 1);
            int entranceLowestY = entranceMiddl - 1;
            int entranceHighestY = entranceMiddl + 1;

            for (int x = 0; x < finalRoomWidth / 2; x++)
            {
                for (int y = entranceLowestY; y < entranceHighestY; y++)
                {
                    grid_[x, y].roomSpriteAbove = RoomSprite.Floor;
                }
            }

            enterPointLeft = new Vector2(1, entranceMiddl);

        }



    }

    public void rightEntrance(bool t, RoomTile[,] grid_)
    {

        if (t)
        {
            int entranceMiddl = prng.Next(finalRoomHeight / 2 - eingrenzungWidth, finalRoomHeight / 2 + eingrenzungWidth - 1);
            int entranceLowestY = entranceMiddl - 1;
            int entranceHighestY = entranceMiddl + 1;
            for (int x = finalRoomWidth / 2; x < finalRoomWidth; x++)
            {
                for (int y = entranceLowestY; y < entranceHighestY; y++)
                {
                    grid_[x, y].roomSpriteAbove = RoomClass.RoomSprite.Floor;
                }
            }

            enterPointRight = new Vector2(finalRoomWidth - 2, entranceMiddl);
        }

    }



    public void topEntrance(bool t, RoomTile[,] grid_)
    {

        if (t)
        {
            int entranceMiddl = prng.Next(finalRoomWidth / 2 - eingrenzungHeight, finalRoomWidth / 2 + eingrenzungHeight - 1);
            int entranceLowestX = entranceMiddl - 1;
            int entranceHighestX = entranceMiddl + 1;
            for (int x = entranceLowestX; x < entranceHighestX; x++)
            {
                for (int y = finalRoomHeight / 2; y < finalRoomHeight; y++)
                {
                    grid_[x, y].roomSpriteAbove = RoomClass.RoomSprite.Floor;
                }
            }

            enterPointTop = new Vector2(entranceMiddl, finalRoomHeight - 2);
        }

    }
    public void botEntrance(bool t, RoomTile[,] grid_)
    {
        if (t)
        {
            int entranceMiddl = prng.Next(finalRoomWidth / 2 - eingrenzungHeight, finalRoomWidth / 2 + eingrenzungHeight - 1);
            int entranceLowestX = entranceMiddl - 1;
            int entranceHighestX = entranceMiddl + 1;
            for (int x = entranceLowestX; x < entranceHighestX; x++)
            {
                for (int y = 0; y < finalRoomHeight / 2; y++)
                {
                    grid_[x, y].roomSpriteAbove = RoomClass.RoomSprite.Floor;
                }

            }

            enterPointBot = new Vector2(entranceMiddl,  2 );
        }

    }
    public void MakeColliderBorder(RoomTile[,] grid_)
    {
        for (int x = 0; x < finalRoomWidth; x++) grid_[x, finalRoomHeight - 1].roomSpriteAbove = RoomClass.RoomSprite.BorderWall;
        for (int x = 0; x < finalRoomWidth; x++) grid_[x, 0].roomSpriteAbove = RoomClass.RoomSprite.BorderWall;
        for (int y = 0; y < finalRoomHeight; y++) grid_[finalRoomWidth - 1, y].roomSpriteAbove = RoomClass.RoomSprite.BorderWall;
        for (int y = 0; y < finalRoomHeight; y++) grid_[0, y].roomSpriteAbove = RoomClass.RoomSprite.BorderWall;
    }

    public void InRoomStructure(RoomTile[,] grid)
    {
        for (int x = middleWidth - roomWidth / 3; x < middleWidth + roomWidth / 3;x++)
        {
            for (int y = middleHeight - roomHeight / 3; y < middleHeight + roomHeight / 3;y++)
            {
                if ((grid[x, y].roomSpriteAbove == RoomSprite.Floor || grid[x, y].roomSpriteAbove == RoomSprite.Floor2) && OneNeighbour(x,y))
                {
                    grid[x, y].roomSpriteAbove = RoomSprite.Border;
                }
            }
        }


        bool OneNeighbour(int posX, int posY)
        {
            int counter = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (grid[posX + x, posY + y].roomSpriteAbove == RoomSprite.Border) counter++;
                }
            }

            switch (counter)
            {
                case 0:
                    return (prng.Next(101) > 95);
                case 1:
                    return (prng.Next(101) > 80);
                case 2:
                    return (prng.Next(101) > 80);
                case 3:
                    return (prng.Next(101) > 80);
                case 4:
                    return (prng.Next(101) > 80);
                case 5:
                    return (prng.Next(101) > 94);
                default:
                    return (prng.Next(101) > 98);
            }

        }

    }
    public void Floor2(RoomTile[,] grid_)
    {
        for (int x = border; x < finalRoomWidth - border; x++)
        {
            for (int y = border; y < finalRoomHeight - border; y++)
            {
                if (grid_[x, y].roomSpriteAbove == RoomSprite.Floor && prng.Next(11) < 2)
                {
                    int cas = prng.Next(11);
                    if (cas < 3) grid_[x, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;

                    if (cas < 5 && cas > 3)
                    {
                        grid_[x, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x + 1, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x, y + 1].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x + 1, y + 1].roomSpriteUnderneath = RoomSpriteBelow.Floor2;

                    }
                    if (cas < 8 && cas > 6)
                    {
                        grid_[x, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x + 1, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x, y + 1].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x + 1, y + 1].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x - 1, y].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                        grid_[x, y - 1].roomSpriteUnderneath = RoomSpriteBelow.Floor2;
                    }
                }
            }
        }
    }


    public bool BorderCheck(int x, int y) //returns True if the given position is within the border
    {

        return (x < border || x > finalRoomWidth - border - 1 || y < border || y > finalRoomHeight - border - 1);
    }

    public bool BorderCheckWithRange(int x, int y, int range) //returns True if the given position is within the border
    {

        return (x < border + range || x > finalRoomWidth - border - range - 1 || y < border + range || y > finalRoomHeight - border - range - 1);
    }
}
