using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGeneratorClass
{
    const float chanceWalkerDestoy = 0.4f;
    const float chanceWalkerChangeDir = 0.4f;
    const float chanceWalkerSpawn = 0.4f;

    const int maxWalkers = 8;

    struct Walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    public enum RoomType { Empty, CleanUp, SpawnRoom, DefaultRoom, ChestRoom }

    public static RoomType[,] GenerateMap(int seed, int mapWidth, int mapHeight, int maxRooms)
    {


        System.Random prng = new System.Random(seed);

        RoomType[,] map = new RoomType[mapWidth, mapHeight];

        List<Walker> walkers;


        Setup();
        CreateBasicLayout();
        DeleteMiddleRooms();

        AddSpawnRoom();
        AddChestRoom();

        void Setup()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    map[x, y] = RoomType.Empty;
                }
            }

            walkers = new List<Walker>();
            // first walker
            Walker newWalker = new Walker();
            // asign direction to first walker
            newWalker.dir = RandomDirection();

            //find center of made grid
            Vector2 spawnPos = new Vector2(Mathf.RoundToInt(mapWidth / 2.0f), Mathf.RoundToInt(mapHeight / 2));
            //spawn walker in center of grid
            newWalker.pos = spawnPos;

            //add walker to list
            walkers.Add(newWalker);
        }

        void CreateBasicLayout()
        {
            int limit = 0;
            do
            {
                foreach (Walker myWalker in walkers)
                {
                    map[(int)myWalker.pos.x, (int)myWalker.pos.y] = RoomType.DefaultRoom;
                }

                //chance: destroy walker
                int numberChecks = walkers.Count; //might modify count while in this loop

                for (int i = 0; i < numberChecks; i++)
                {
                    //only if its not the only one, and at a low chance
                    if (prng.NextDouble() < chanceWalkerDestoy && walkers.Count > 1)
                    {
                        walkers.RemoveAt(i);
                        break; //only destroy one per iteration
                    }
                }

                for (int i = 0; i < walkers.Count; i++)
                {
                    if (prng.NextDouble() < chanceWalkerChangeDir)
                    {
                        Walker thisWalker = walkers[i];
                        thisWalker.dir = RandomDirection();
                        walkers[i] = thisWalker;
                    }
                }
                //chance to spawn a new walker
                numberChecks = walkers.Count; //might modify count while in this loop
                for (int i = 0; i < numberChecks; i++)
                {
                    //only if # of walkers < max, and at a low chance
                    if (prng.NextDouble() < chanceWalkerSpawn && walkers.Count < maxWalkers)
                    {
                        //create a walker 
                        Walker newWalker = new Walker();
                        newWalker.dir = RandomDirection();
                        newWalker.pos = walkers[i].pos;
                        walkers.Add(newWalker);
                    }
                }
                //move walkers
                for (int i = 0; i < walkers.Count; i++)
                {
                    Walker thisWalker = walkers[i];
                    thisWalker.pos += thisWalker.dir;
                    walkers[i] = thisWalker;
                }
                //avoid boarder of grid
                for (int i = 0; i < walkers.Count; i++)
                {
                    Walker thisWalker = walkers[i];
                    //clamp x,y to leave a 3 space boarder: leave room for walls
                    thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 5, mapWidth - 5);
                    thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 5, mapHeight - 5);
                    walkers[i] = thisWalker;
                }
                //check to exit loop
                if ((float)NumberOfRooms(map) > maxRooms)
                {
                    break;
                }
                limit++;
            } while (limit < 100);

        }


        void DeleteMiddleRooms()
        {
            for (int x = 2; x < mapWidth - 3; x++)
            {
                for (int y = 1; y < mapHeight - 2; y++)
                {
                    if (map[x, y] == RoomType.DefaultRoom)
                    {
                        if (NumberOfNeighboors(x, y, RoomType.DefaultRoom, RoomType.CleanUp) > 7)
                        {
                            map[x, y] = RoomType.CleanUp;

                        }

                    }
                }
            }

            CleanUp();
        }

        int NumberOfRooms(RoomType[,] gridLocal)
        {
            int count = 0;
            foreach (RoomType space in gridLocal)
            {
                if (space == RoomType.DefaultRoom)
                {
                    count++;
                }
            }
            return count;
        }

        int ChestRoomCount(RoomType[,] gridLocal)
        {
            int count = 0;
            foreach (RoomType space in gridLocal)
            {
                if (space == RoomType.ChestRoom)
                {
                    count++;
                }
            }
            return count;
        }

        void CleanUp()
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == RoomType.CleanUp) map[x, y] = RoomType.Empty;

                }
            }
        }

        Vector2 RandomDirection()
        {
            //pick random int between 0 and 3
            int choice = prng.Next(0, 4); //can give the numbers: 0,1,2,3 
            //use that int to chose a direction
            switch (choice)
            {
                case 0:
                    return Vector2.down;
                case 1:
                    return Vector2.left;
                case 2:
                    return Vector2.up;
                default:
                    return Vector2.right;
            }

        }


        int NumberOfNeighboors(int posX, int posY, RoomType typeOfNeighbour, RoomType typeOfNeighbour2)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (map[posX + x, posY + y] == typeOfNeighbour || map[posX + x, posY + y] == typeOfNeighbour2)
                    {
                        if (!(x == 0 && y == 0))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }


        int NumberOfAccesibleNeighboors(int posX, int posY, RoomType typeOfNeighbour, RoomType typeOfNeighbour2)
        {
            int count = 0;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (map[posX + x, posY + y] == typeOfNeighbour || map[posX + x, posY + y] == typeOfNeighbour2)
                    {
                        if (!(x == 0 && y == 0) && !(x != 0 && y != 0))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        void AddSpawnRoom()
        {
            for (int x = 2; x < mapWidth - 1; x++)
            {
                for (int y = 2; y < mapHeight - 1; y++)
                {
                    if (map[x, y] == RoomType.Empty && prng.Next(0, 101) < 20 && NumberOfAccesibleNeighboors(x, y, RoomType.DefaultRoom, RoomType.DefaultRoom) != 0)
                    {
                        map[x, y] = RoomType.SpawnRoom;
                        return;
                    }
                }
            }

            // if first Loop fails
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (map[x, y] == RoomType.DefaultRoom)
                    {
                        map[x, y] = RoomType.SpawnRoom;
                        return;
                    }
                }
            }

        }

        void AddChestRoom()
        {
            while (ChestRoomCount(map) < 1)
            {
                for (int x = 2; x < mapWidth - 1; x++)
                {
                    for (int y = 2; y < mapHeight - 1; y++)
                    {
                        if (map[x, y] == RoomType.DefaultRoom && prng.Next(0, 101) < 20)
                        {
                            map[x, y] = RoomType.ChestRoom;
                            return;
                        }
                    }
                }
            }
        }



        return map;


    }




}