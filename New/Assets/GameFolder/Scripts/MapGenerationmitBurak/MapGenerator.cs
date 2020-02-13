using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int mapWidth, mapHeight, maxRooms;

    public int seed;



    public TileSetHolder[] tileSetHolders;
    public RoomDecorator roomDecorator;
    public ChestHolder chestHolder;


    private GameObject player;
    private RoomClass[,] roomGrid;
    private new Camera camera;
    System.Random prng;

    private void Awake()
    {

        player = GameObject.FindWithTag("Player");
        camera = Camera.main;
    }

    private void Start()
    {
        int roomWidhtsMin = 40;
        int roomWidhtsMax = 60;

        int roomHeightsMin = 30;
        int roomHeightsMax = 50;

        prng = new System.Random(seed);

        MapGeneratorClass.RoomType[,] map = InitiliazeRoomGrid();

        SetupRoomsInMapGrid(map, roomWidhtsMin, roomWidhtsMax, roomHeightsMin, roomHeightsMax);

        SetEnterPointsAndVisualizeRooms(); //SetSetEnterPoints() has to be called AFTER SetupRoomsInMapGrid 

        SetupExits(); // SetupTriggers() has to be called AFTER SetSetEnterPoints() because it connects a Rooms Exit with the EnterPoints of the Rooms around it

        LastSetup();
    }




    private MapGeneratorClass.RoomType[,] InitiliazeRoomGrid()
    {
        MapGeneratorClass.RoomType[,] map = MapGeneratorClass.GenerateMap(seed, mapWidth, mapHeight, maxRooms);
        roomGrid = new RoomClass[mapWidth, mapHeight];

        //Initialise RoomGrid with an empty (default) Room
        RoomClass room = new RoomClass(prng.Next(-100000, 100000), 0, 0, Vector2.zero, false, false, false, false, false);
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                roomGrid[x, y] = room;
            }
        }

        return map;
    }

    private MapGeneratorClass.RoomType[,] SetupRoomsInMapGrid(MapGeneratorClass.RoomType[,] map, int roomWidhtsMin, int roomWidhtsMax, int roomHeightsMin, int roomHeightsMax)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == MapGeneratorClass.RoomType.DefaultRoom)
                {
                    //add Room to RoomGrid and Setup
                    roomGrid[x, y] = DefaultRoomSetup(map, x, y, roomWidhtsMin, roomWidhtsMax, roomHeightsMin, roomHeightsMax);
                }

                if (map[x, y] == MapGeneratorClass.RoomType.SpawnRoom)
                {
                    //add Room to RoomGrid and Setup
                    roomGrid[x, y] = SpawnRoomSetup(map, x, y, 15, 20, 10, 15);
                }

                if (map[x, y] == MapGeneratorClass.RoomType.ChestRoom)
                {
                    //add Room to RoomGrid and Setup
                    roomGrid[x, y] = ChestRoomSetup(map, x, y, 15, 20, 10, 15);
                }
            }
        }

        return map;
    }

    private void LastSetup() //Everything that can be Setup in one last Loop over the RoomGrid to avoid extended processing
    {
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                SpawnChests(x,y);
                SpawnPlayerInSpawnRoom(x,y);
            }
        }
    }
    private void SetEnterPointsAndVisualizeRooms()
    {
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                if (roomGrid[x, y].active)
                {

                    roomGrid[x, y].globalPosition = new Vector2(x * getBiggestRoomWidth(roomGrid), y * getBiggestRoomHeight(roomGrid));
                    VisualizeRoom(roomGrid[x, y], tileSetHolders[0]);

                    roomDecorator.SpawnRoomDecoration(roomGrid[x, y]);

                    SetEnterPoints(roomGrid[x, y]);
                }
            }
        }
    }
    private void SetupExits()
    {
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                if (roomGrid[x, y].active)
                {
                    SetupTriggers(roomGrid[x, y], x, y);
                }
            }
        }
    }


    
    //for LastSetup()
    private void SpawnPlayerInSpawnRoom(int x, int y)
    {

        if (roomGrid[x, y].roomType == RoomClass.RoomType.SpawnRoom)
        {
            player.transform.position = new Vector2(roomGrid[x, y].globalPosition.x + roomGrid[x, y].finalRoomWidth / 2, roomGrid[x, y].globalPosition.y + roomGrid[x, y].finalRoomHeight / 2);
            camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
            return;
        }

    }

    private void SpawnChests(int x, int y)
    { 
        if (roomGrid[x, y].roomType == RoomClass.RoomType.ChestRoom)
        {
            ChestRoom temporaryCast = (ChestRoom)roomGrid[x, y];
            chestHolder.SpawnRandomChest( temporaryCast.ChestLocation , roomGrid[x, y].thisRoom );
        }
    }


    // RoomSetups for every RoomType
    private RoomClass DefaultRoomSetup(MapGeneratorClass.RoomType[,] map, int posX, int posY, int roomWidhtsMin, int roomWidhtsMax, int roomHeightsMin, int roomHeightsMax)
    {
        GameObject Room = new GameObject("DefaultRoom");

        RoomClass thisRoom = new RoomClass(prng.Next(-100000, 100000), prng.Next(roomWidhtsMin, roomWidhtsMax), prng.Next(roomHeightsMin, roomHeightsMax), Vector2.zero, CheckDirectionLeft(map, posX, posY), CheckDirectionRight(map, posX, posY),
            CheckDirectionUp(map, posX, posY), CheckDirectionDown(map, posX, posY), true);

        thisRoom.thisRoom = Room;

        thisRoom.GenerateRoomGrid();
        thisRoom.roomType = RoomClass.RoomType.Default;

        return thisRoom;
    }

    private RoomClass SpawnRoomSetup(MapGeneratorClass.RoomType[,] map, int posX, int posY, int roomWidhtsMin, int roomWidhtsMax, int roomHeightsMin, int roomHeightsMax)
    {
        GameObject Room = new GameObject("SpawnRoom");

        SpawnRoom thisRoom = new SpawnRoom(prng.Next(-100000, 100000), prng.Next(roomWidhtsMin, roomWidhtsMax), prng.Next(roomHeightsMin, roomHeightsMax), Vector2.zero, CheckDirectionLeft(map, posX, posY), CheckDirectionRight(map, posX, posY),
            CheckDirectionUp(map, posX, posY), CheckDirectionDown(map, posX, posY), true)
        {
            thisRoom = Room,
            roomType = RoomClass.RoomType.SpawnRoom
        };

        thisRoom.GenerateRoomGrid();


        return thisRoom;
    }

    private RoomClass ChestRoomSetup(MapGeneratorClass.RoomType[,] map, int posX, int posY, int roomWidhtsMin, int roomWidhtsMax, int roomHeightsMin, int roomHeightsMax)
    {
        GameObject Room = new GameObject("ChestRoom");

        ChestRoom thisRoom = new ChestRoom(prng.Next(-100000, 100000), prng.Next(roomWidhtsMin, roomWidhtsMax), prng.Next(roomHeightsMin, roomHeightsMax), Vector2.zero, CheckDirectionLeft(map, posX, posY), CheckDirectionRight(map, posX, posY),
            CheckDirectionUp(map, posX, posY), CheckDirectionDown(map, posX, posY), true)
        {
            thisRoom = Room,
            roomType = RoomClass.RoomType.ChestRoom
        };

        thisRoom.GenerateRoomGrid();


        return thisRoom;
    }




    // start Help Methods
    void VisualizeRoom(RoomClass room, TileSetHolder defaultRoomTileset)
    {
        GameObject tileHolder = new GameObject("TileHolder");
        tileHolder.transform.SetParent(room.thisRoom.transform);
        RoomClass.RoomTile[,] roomGrid = room.roomGrid;


        room.thisRoom.transform.position = room.globalPosition;

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {

                switch (roomGrid[x, y].roomSpriteAbove)
                {
                    case RoomClass.RoomSprite.Empty:
                        MyUtils.Spawn(x, y, defaultRoomTileset.empty, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Border:
                        MyUtils.Spawn(x, y, defaultRoomTileset.border, tileHolder);
                        break;

                    case RoomClass.RoomSprite.Floor:
                        MyUtils.Spawn(x, y, defaultRoomTileset.floor, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Floor2:
                        MyUtils.Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;

                    case RoomClass.RoomSprite.WallBot:
                        MyUtils.Spawn(x, y, defaultRoomTileset.botWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallTop:
                        MyUtils.Spawn(x, y, defaultRoomTileset.topWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallRight:
                        MyUtils.Spawn(x, y, defaultRoomTileset.rightWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallLeft:
                        MyUtils.Spawn(x, y, defaultRoomTileset.leftWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopLeft:
                        MyUtils.Spawn(x, y, defaultRoomTileset.cornerTopLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopRight:
                        MyUtils.Spawn(x, y, defaultRoomTileset.cornerTopRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotLeft:
                        MyUtils.Spawn(x, y, defaultRoomTileset.cornerBotLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotRight:
                        MyUtils.Spawn(x, y, defaultRoomTileset.cornerBotRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoTopBot:
                        MyUtils.Spawn(x, y, defaultRoomTileset.twoTopBot, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoLeftRight:
                        MyUtils.Spawn(x, y, defaultRoomTileset.twoLeftRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeBot:
                        MyUtils.Spawn(x, y, defaultRoomTileset.threeBot, tileHolder);
                        break;

                    case RoomClass.RoomSprite.threeTop:
                        MyUtils.Spawn(x, y, defaultRoomTileset.threeTop, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeLeft:
                        MyUtils.Spawn(x, y, defaultRoomTileset.threeLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeRight:
                        MyUtils.Spawn(x, y, defaultRoomTileset.threeRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Pillar:
                        MyUtils.Spawn(x, y, defaultRoomTileset.pillar, tileHolder);
                        break;
                    case RoomClass.RoomSprite.BorderWall:
                        MyUtils.Spawn(x, y, defaultRoomTileset.borderWall, tileHolder);
                        break;
                }

                switch (roomGrid[x, y].roomSpriteUnderneath)
                {
                    case RoomClass.RoomSpriteBelow.Floor:
                        MyUtils.Spawn(x, y, defaultRoomTileset.floor, tileHolder);
                        break;
                    case RoomClass.RoomSpriteBelow.Floor2:
                        MyUtils.Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                }
            }
        }
    }

    int getBiggestRoomWidth(RoomClass[,] grid)
    {
        int max = grid[0, 0].finalRoomWidth;
        for (int x = 1; x < grid.GetLength(0); x++)
        {
            for (int y = 1; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].finalRoomWidth > max)
                {
                    max = grid[x, y].finalRoomWidth;
                }
            }
        }
        return max;
    }

    int getBiggestRoomHeight(RoomClass[,] grid)
    {
        int max = grid[0, 0].finalRoomHeight;

        for (int x = 1; x < grid.GetLength(0); x++)
        {
            for (int y = 1; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].finalRoomHeight > max)
                {
                    max = grid[x, y].finalRoomHeight;
                }
            }
        }

        return max;
    }

    bool CheckDirectionDown(MapGeneratorClass.RoomType[,] map, int posX, int posY)
    {
        return (RoomCheck(map, posX, posY - 1));
    }

    bool CheckDirectionUp(MapGeneratorClass.RoomType[,] map, int posX, int posY)
    {
        return (RoomCheck(map, posX, posY + 1));
    }

    bool CheckDirectionLeft(MapGeneratorClass.RoomType[,] map, int posX, int posY)
    {
        return (RoomCheck(map, posX - 1, posY));
    }

    bool CheckDirectionRight(MapGeneratorClass.RoomType[,] map, int posX, int posY)
    {
        return (RoomCheck(map, posX + 1, posY));
    }


    void SetupTriggers(RoomClass room, int PositionInRoomGridX, int PositionInRoomGridY)
    {
        BoxCollider2D roomColl = room.thisRoom.AddComponent<BoxCollider2D>();
        RoomChecker thisRoomsChecker = room.thisRoom.AddComponent<RoomChecker>();
        thisRoomsChecker.ParentRoom = room;
        roomColl.size = new Vector2(room.finalRoomWidth, room.finalRoomHeight);
        roomColl.offset = new Vector2(room.finalRoomWidth / 2, room.finalRoomHeight / 2);
        roomColl.isTrigger = true;


        if (room.leftOpen)
        {
            GameObject leftTriggerHolder = new GameObject("Left Trigger");
            leftTriggerHolder.transform.SetParent(room.thisRoom.transform);
            leftTriggerHolder.transform.localPosition = room.enterPointLeft - room.globalPosition + Vector2.left;

            BoxCollider2D boxColl = leftTriggerHolder.AddComponent<BoxCollider2D>();
            CollisionDetector collDet = leftTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX - 1, PositionInRoomGridY].enterPointRight;
            boxColl.size = new Vector2(1, 4);
            boxColl.isTrigger = true;

        }

        if (room.rightOpen)
        {
            GameObject rightTriggerHolder = new GameObject("Right Trigger");
            rightTriggerHolder.transform.SetParent(room.thisRoom.transform);
            rightTriggerHolder.transform.localPosition = room.enterPointRight - room.globalPosition + Vector2.right;

            CollisionDetector collDet = rightTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX + 1, PositionInRoomGridY].enterPointLeft;
            BoxCollider2D boxColl = rightTriggerHolder.AddComponent<BoxCollider2D>();
            boxColl.size = new Vector2(1, 4);
            boxColl.isTrigger = true;
        }

        if (room.botOpen)
        {
            GameObject botTriggerHolder = new GameObject("Bot Trigger");
            botTriggerHolder.transform.SetParent(room.thisRoom.transform);
            botTriggerHolder.transform.localPosition = room.enterPointBot - room.globalPosition + Vector2.down;


            BoxCollider2D boxColl = botTriggerHolder.AddComponent<BoxCollider2D>();
            CollisionDetector collDet = botTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX, PositionInRoomGridY - 1].enterPointTop;
            boxColl.size = new Vector2(4, 1);
            boxColl.isTrigger = true;
        }

        if (room.topOpen)
        {
            GameObject topTriggerHolder = new GameObject("Top Trigger");
            topTriggerHolder.transform.SetParent(room.thisRoom.transform);
            topTriggerHolder.transform.localPosition = room.enterPointTop - room.globalPosition + Vector2.up;

            BoxCollider2D boxColl = topTriggerHolder.AddComponent<BoxCollider2D>();
            CollisionDetector collDet = topTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX, PositionInRoomGridY + 1].enterPointBot;
            boxColl.size = new Vector2(4, 1);
            boxColl.isTrigger = true;
        }
    }

    void SetEnterPoints(RoomClass room)
    {
        if (room.leftOpen)
        {
            room.enterPointLeft += room.globalPosition;
        }

        if (room.rightOpen)
        {
            room.enterPointRight += room.globalPosition;
        }

        if (room.topOpen)
        {
            room.enterPointTop += room.globalPosition;
        }

        if (room.botOpen)
        {
            room.enterPointBot += room.globalPosition;
        }
    }

    bool RoomCheck(MapGeneratorClass.RoomType[,] map, int x, int y)
    {
        return (map[x, y] == MapGeneratorClass.RoomType.DefaultRoom || map[x, y] == MapGeneratorClass.RoomType.SpawnRoom || map[x, y] == MapGeneratorClass.RoomType.ChestRoom);
    }

    // end Help Methods
}