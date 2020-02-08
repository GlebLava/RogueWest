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

       prng = new System.Random(seed);


        MapGeneratorClass.RoomType[,] map = MapGeneratorClass.GenerateMap(seed, mapWidth, mapHeight, maxRooms);
        roomGrid = new RoomClass[mapWidth, mapHeight];

        //Initialise RoomGrid with an empty (default) Room
        GameObject platzHalter = new GameObject();
        RoomClass room = new RoomClass(0, Vector2.zero, false, false, false, false, false, platzHalter);
        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                roomGrid[x,y] = room;
            }
        }

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x,y] == MapGeneratorClass.RoomType.DefaultRoom)
                {
                    GameObject Room = new GameObject("DefaultRoom");

                    RoomClass thisRoom = new RoomClass(0, Vector2.zero, CheckDirectionLeft(map, x, y), CheckDirectionRight(map, x, y),
                        CheckDirectionUp(map, x, y), CheckDirectionDown(map, x, y), true, Room);

                    thisRoom.globalPosition = new Vector2(x * thisRoom.finalRoomWidth, y * thisRoom.finalRoomHeight);
                    thisRoom.GenerateRoomGrid();
                    VisualizeRoom(thisRoom, tileSetHolders[0]);

                    SpawnRoomDecoration(thisRoom, cactus, Room);

                    SetEnterPoints(thisRoom);

                    //add Room to RoomGrid
                    roomGrid[x, y] = thisRoom;
                    
                }

                if (map[x, y] == MapGeneratorClass.RoomType.SpawnRoom)
                {
                    GameObject Room = new GameObject("SpawnRoom");

                    SpawnRoom thisRoom = new SpawnRoom(0, Vector2.zero, CheckDirectionLeft(map, x, y), CheckDirectionRight(map, x, y),
                        CheckDirectionUp(map, x, y), CheckDirectionDown(map, x, y), true, Room);

                    thisRoom.globalPosition = new Vector2(x * thisRoom.finalRoomWidth, y * thisRoom.finalRoomHeight);
                    thisRoom.GenerateRoomGrid();
                    VisualizeRoom(thisRoom, tileSetHolders[0]);

                    SpawnRoomDecoration(thisRoom, cactus, Room);

                    SetEnterPoints(thisRoom);

                    //add Room to RoomGrid
                    roomGrid[x, y] = thisRoom;

                    player.transform.position = new Vector2(thisRoom.globalPosition.x + thisRoom.finalRoomWidth / 2, thisRoom.globalPosition.y + thisRoom.finalRoomHeight / 2);
                    camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
                    
                }
            }
        }

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
                if (roomGrid[x,y].active)
                {
                    SetupTriggers(roomGrid[x, y],roomGrid[x,y].ThisRoom , x, y);
                }
            }
        }

        

    }

    private void FixedUpdate()
    {
        
    }


    void VisualizeRoom( RoomClass room,  TileSetHolder defaultRoomTileset)
    {
        GameObject tileHolder = new GameObject("TileHolder");
        tileHolder.transform.SetParent(room.ThisRoom.transform);
        RoomClass.RoomSprite[,] roomGrid = room.roomGrid;


        room.ThisRoom.transform.position = room.globalPosition;

        for (int x = 0; x < roomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < roomGrid.GetLength(1); y++)
            {
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

                    case RoomClass.RoomSprite.WallBot:
                        Spawn(x, y, defaultRoomTileset.botWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallTop:
                        Spawn(x, y, defaultRoomTileset.topWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallRight:
                        Spawn(x, y, defaultRoomTileset.rightWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.WallLeft:
                        Spawn(x, y, defaultRoomTileset.leftWall, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopLeft:
                        Spawn(x, y, defaultRoomTileset.cornerTopLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerTopRight:
                        Spawn(x, y, defaultRoomTileset.cornerTopRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotLeft:
                        Spawn(x, y, defaultRoomTileset.cornerBotLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.CornerBotRight:
                        Spawn(x, y, defaultRoomTileset.cornerBotRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoTopBot:
                        Spawn(x, y, defaultRoomTileset.twoTopBot, tileHolder);
                        break;
                    case RoomClass.RoomSprite.twoLeftRight:
                        Spawn(x, y, defaultRoomTileset.twoLeftRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeBot:
                        Spawn(x, y, defaultRoomTileset.threeBot, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeTop:
                        Spawn(x, y, defaultRoomTileset.threeTop, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeLeft:
                        Spawn(x, y, defaultRoomTileset.threeLeft, tileHolder);
                        break;
                    case RoomClass.RoomSprite.threeRight:
                        Spawn(x, y, defaultRoomTileset.threeRight, tileHolder);
                        break;
                    case RoomClass.RoomSprite.Pillar:
                        Spawn(x, y, defaultRoomTileset.pillar, tileHolder);
                        break;
                }
            }
        }
    }

    int getBiggestRoomWidth(RoomClass[,] grid)
    {
        int max = grid[0, 0].finalRoomWidth;
        return max;
    }

    int getBiggestRoomHeight(RoomClass[,] grid)
    {
        int max = grid[0, 0].finalRoomHeight;
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


    void SetupTriggers(RoomClass room, GameObject parent, int PositionInRoomGridX, int PositionInRoomGridY)
    {
        BoxCollider2D roomColl = parent.AddComponent<BoxCollider2D>();
        roomColl.size = new Vector2(room.finalRoomWidth - 1, room.finalRoomHeight - 1);
        roomColl.offset = new Vector2(room.finalRoomWidth / 2, room.finalRoomHeight / 2);
        roomColl.isTrigger = true;


        if (room.leftOpen)
        {
            GameObject leftTriggerHolder = new GameObject("Left Trigger");
            leftTriggerHolder.transform.SetParent(parent.transform);
            leftTriggerHolder.transform.localPosition = new Vector2(room.border / 3 * 2, room.finalRoomHeight / 2);

            BoxCollider2D boxColl = leftTriggerHolder.AddComponent<BoxCollider2D>();
            CollisionDetector collDet = leftTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX - 1, PositionInRoomGridY].enterPointRight;
            boxColl.size = new Vector2(1, 4);
            boxColl.isTrigger = true;

        }

        if (room.rightOpen)
        {
            GameObject rightTriggerHolder = new GameObject("Right Trigger");
            rightTriggerHolder.transform.SetParent(parent.transform);
            rightTriggerHolder.transform.localPosition = new Vector2(room.roomWidth + (room.border * 2 / 3) * 2, room.finalRoomHeight / 2);

            CollisionDetector collDet = rightTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX + 1, PositionInRoomGridY].enterPointLeft;
            BoxCollider2D boxColl = rightTriggerHolder.AddComponent<BoxCollider2D>();
            boxColl.size = new Vector2(1, 4);
            boxColl.isTrigger = true;
        }

        if (room.botOpen)
        {
            GameObject botTriggerHolder = new GameObject("Bot Trigger");
            botTriggerHolder.transform.SetParent(parent.transform);
            botTriggerHolder.transform.localPosition = new Vector2(room.finalRoomWidth / 2, (room.border / 3 * 2));


            BoxCollider2D boxColl = botTriggerHolder.AddComponent<BoxCollider2D>();
            CollisionDetector collDet = botTriggerHolder.AddComponent<CollisionDetector>();
            collDet.entrance = roomGrid[PositionInRoomGridX, PositionInRoomGridY - 1].enterPointTop;
            boxColl.size = new Vector2(4, 1);
            boxColl.isTrigger = true;
        }

        if (room.topOpen)
        {
            GameObject topTriggerHolder = new GameObject("Top Trigger");
            topTriggerHolder.transform.SetParent(parent.transform);
            topTriggerHolder.transform.localPosition = new Vector2(room.finalRoomWidth / 2, room.roomHeight + room.border / 2 * 3);

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


    void SpawnRoomDecoration(RoomClass room, GameObject decoration, GameObject parent)
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
                    if (prng.Next(0,10) < 2 && distanceX > 1 && distanceY > 3 && grid[x,y] == RoomClass.RoomSprite.Floor)
                    {
                        Spawn(x, y, decoration, parent);
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
