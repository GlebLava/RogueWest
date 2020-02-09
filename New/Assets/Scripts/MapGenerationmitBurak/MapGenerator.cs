using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int mapWidth, mapHeight, maxRooms;

    public int seed;

    private GameObject player;

    private RoomClass[,] roomGrid;

    public TileSetHolder[] tileSetHolders;

    private new Camera camera;

    public GameObject cactus;

    System.Random prng;

    private void Awake()
    {

        player = GameObject.FindWithTag("Player");
        camera = Camera.main;
    }

    private void Start()
    {
        int roomWidhtsMin = 20;
        int roomWidhtsMax = 40;

        int roomHeightsMin = 15;
        int roomHeightsMax = 30;

        prng = new System.Random(seed);


        MapGeneratorClass.RoomType[,] map = MapGeneratorClass.GenerateMap(seed, mapWidth, mapHeight, maxRooms);
        roomGrid = new RoomClass[mapWidth, mapHeight];

        //Initialise RoomGrid with an empty (default) Room
        RoomClass room = new RoomClass(prng.Next(-100000, 100000),0,0, Vector2.zero, false, false, false, false, false);
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                roomGrid[x, y] = room;
            }
        }

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == MapGeneratorClass.RoomType.DefaultRoom)
                {
                    GameObject Room = new GameObject("DefaultRoom");

                    RoomClass thisRoom = new RoomClass(prng.Next(-100000,100000), prng.Next(roomWidhtsMin, roomWidhtsMax), prng.Next(roomHeightsMin, roomHeightsMax), Vector2.zero, CheckDirectionLeft(map, x, y), CheckDirectionRight(map, x, y),
                        CheckDirectionUp(map, x, y), CheckDirectionDown(map, x, y), true);

                    thisRoom.thisRoom = Room;

                    thisRoom.GenerateRoomGrid();
                    thisRoom.roomType = RoomClass.RoomType.Default;

                    //add Room to RoomGrid
                    roomGrid[x, y] = thisRoom;

                }

                if (map[x, y] == MapGeneratorClass.RoomType.SpawnRoom)
                {
                    GameObject Room = new GameObject("SpawnRoom");

                    SpawnRoom thisRoom = new SpawnRoom(prng.Next(-100000, 100000), prng.Next(roomWidhtsMin, roomWidhtsMax), prng.Next(roomHeightsMin, roomHeightsMax), Vector2.zero, CheckDirectionLeft(map, x, y), CheckDirectionRight(map, x, y),
                        CheckDirectionUp(map, x, y), CheckDirectionDown(map, x, y), true);
                    thisRoom.thisRoom = Room;

                    thisRoom.GenerateRoomGrid();

                    thisRoom.roomType = RoomClass.RoomType.SpawnRoom;
                    //add Room to RoomGrid
                    roomGrid[x, y] = thisRoom;
                }
            }
        }

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                if (roomGrid[x, y].active)
                {

                    roomGrid[x, y].globalPosition = new Vector2(x * getBiggestRoomWidth(roomGrid), y * getBiggestRoomHeight(roomGrid));
                    VisualizeRoom(roomGrid[x, y], tileSetHolders[0]);

                    SpawnRoomDecoration(roomGrid[x, y], cactus);

                    SetEnterPoints(roomGrid[x, y]);
                }
            }
        }

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                if (roomGrid[x, y].active)
                {
                    SetupTriggers(roomGrid[x, y], x, y);


                    if (roomGrid[x, y].roomType == RoomClass.RoomType.SpawnRoom)
                    {
                        player.transform.position = new Vector2(roomGrid[x, y].globalPosition.x + roomGrid[x, y].finalRoomWidth / 2, roomGrid[x, y].globalPosition.y + roomGrid[x, y].finalRoomHeight / 2);
                        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {

    }


    void VisualizeRoom(RoomClass room, TileSetHolder defaultRoomTileset)
    {
        GameObject tileHolder = new GameObject("TileHolder");
        tileHolder.transform.SetParent(room.thisRoom.transform);
        RoomClass.RoomSprite[,] roomGrid = room.roomGrid;


        room.thisRoom.transform.position = room.globalPosition;

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                bool trig;
                trig = (prng.Next(11) < 9);

                switch (roomGrid[x, y])
                {
                    case RoomClass.RoomSprite.Empty:
                        Spawn(x, y, defaultRoomTileset.empty, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Border:
                        Spawn(x, y, defaultRoomTileset.border, tileHolder);
                        break;

                    case RoomClass.RoomSprite.Floor:
                        Spawn(x, y, defaultRoomTileset.floor, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Floor2:
                        Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;

                    case RoomClass.RoomSprite.WallBot:
                        Spawn(x, y, defaultRoomTileset.botWall, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallTop:
                        Spawn(x, y, defaultRoomTileset.topWall, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallRight:
                        Spawn(x, y, defaultRoomTileset.rightWall, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallLeft:
                        Spawn(x, y, defaultRoomTileset.leftWall, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopLeft:
                        Spawn(x, y, defaultRoomTileset.cornerTopLeft, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopRight:
                        Spawn(x, y, defaultRoomTileset.cornerTopRight, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotLeft:
                        Spawn(x, y, defaultRoomTileset.cornerBotLeft, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotRight:
                        Spawn(x, y, defaultRoomTileset.cornerBotRight, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoTopBot:
                        Spawn(x, y, defaultRoomTileset.twoTopBot, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoLeftRight:
                        Spawn(x, y, defaultRoomTileset.twoLeftRight, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeBot:
                        Spawn(x, y, defaultRoomTileset.threeBot, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeTop:
                        Spawn(x, y, defaultRoomTileset.threeTop, tileHolder);
                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeLeft:
                        Spawn(x, y, defaultRoomTileset.threeLeft, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeRight:
                        Spawn(x, y, defaultRoomTileset.threeRight, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Pillar:
                        Spawn(x, y, defaultRoomTileset.pillar, tileHolder);

                        if (trig) Spawn(x, y, defaultRoomTileset.floor, tileHolder); //Fill the Tile underneath
                        else Spawn(x, y, defaultRoomTileset.floor2, tileHolder);
                        break;
                    case RoomClass.RoomSprite.BorderWall:
                        Spawn(x, y, defaultRoomTileset.borderWall, tileHolder);
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
                if (grid[x,y].finalRoomWidth > max)
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


    void SetupTriggers(RoomClass room,  int PositionInRoomGridX, int PositionInRoomGridY)
    {
        BoxCollider2D roomColl = room.thisRoom.AddComponent<BoxCollider2D>();
        roomColl.size = new Vector2(room.finalRoomWidth - 1, room.finalRoomHeight - 1);
        roomColl.offset = new Vector2(room.finalRoomWidth / 2, room.finalRoomHeight / 2);
        roomColl.isTrigger = true;


        if (room.leftOpen)
        {
            GameObject leftTriggerHolder = new GameObject("Left Trigger");
            leftTriggerHolder.transform.SetParent(room.thisRoom.transform);
            leftTriggerHolder.transform.localPosition = room.enterPointLeft - room.globalPosition  + Vector2.left;

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
            rightTriggerHolder.transform.localPosition =  room.enterPointRight - room.globalPosition   + Vector2.right;

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
            botTriggerHolder.transform.localPosition =   room.enterPointBot - room.globalPosition +  Vector2.down;


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
            topTriggerHolder.transform.localPosition =  room.enterPointTop - room.globalPosition +  Vector2.up;

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
        return (map[x, y] == MapGeneratorClass.RoomType.DefaultRoom || map[x, y] == MapGeneratorClass.RoomType.SpawnRoom);
    }


    void SpawnRoomDecoration(RoomClass room, GameObject decoration)
    {
        RoomClass.RoomSprite[,] grid = room.roomGrid;
        int distanceX = 0;
        int distanceY = 0;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            distanceX++;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                distanceY++;

                if (!room.BorderCheck(x, y))
                {
                    if (prng.Next(0, 10) < 2 && distanceX > 1 && distanceY > 3 && (grid[x, y] == RoomClass.RoomSprite.Floor || grid[x, y] == RoomClass.RoomSprite.Floor2))
                    {
                        Spawn(x, y, decoration, room.thisRoom);
                        distanceX = 0;
                        distanceY = 0;
                    }


                }
            }

        }
    }

    public void Spawn(float x, float y, GameObject toSpawn, GameObject parent)
    {
        Vector2 spawnPos = new Vector2(x, y);
        GameObject thing = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
        thing.transform.parent = parent.transform;
        thing.transform.localPosition = spawnPos;
    }
}