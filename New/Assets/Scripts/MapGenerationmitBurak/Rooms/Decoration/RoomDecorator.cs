using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDecorator : MonoBehaviour
{
    public GameObject skull;
    public GameObject[] cacti;
    public GameObject[] inRoomStones;

    public GameObject[] outRoomMiddleStones;

    System.Random prng;


    enum DecoRation { empty, basicDeco };

    RoomClass.RoomTile[,] grid;
    DecoStruct[,] decoGrid;

    int minRangeonOutRoomDeco;

    struct DecoStruct
    {
        public DecoRation decoType;
        public bool inRoomSpawnable; //false by default
        public bool BorderSpawnable;
    }

    public void SpawnRoomDecoration(RoomClass room)
    {
        prng = room.Prng;
        grid = room.roomGrid;
        decoGrid = new DecoStruct[room.roomGrid.GetLength(0), room.roomGrid.GetLength(1)];


        //Setup
        minRangeonOutRoomDeco = 2;
        for (int x = 0 ; x < grid.GetLength(0) ; x++)
        {
            for (int y = 0 ; y < grid.GetLength(1) ; y++)
            {
                if (!room.BorderCheckWithRange(x, y, 2))
                {
                    if ((grid[x, y].roomSpriteAbove == RoomClass.RoomSprite.Floor || grid[x, y].roomSpriteAbove == RoomClass.RoomSprite.Floor2)) decoGrid[x, y].inRoomSpawnable = true;
                    else decoGrid[x, y].inRoomSpawnable = false;
                }
                decoGrid[x, y].BorderSpawnable = !decoGrid[x, y].inRoomSpawnable;
            }
        }

        for (int x = minRangeonOutRoomDeco; x < decoGrid.GetLength(0) - minRangeonOutRoomDeco; x++)
        {
            for (int y = minRangeonOutRoomDeco; y < decoGrid.GetLength(1) - minRangeonOutRoomDeco; y++)
            { 

                //inRoomDeco
                if (decoGrid[x, y].inRoomSpawnable && prng.Next(101) > 80)
                {
                    Spawn(x + Random.Range(-0.3f,0.3f), y + Random.Range(-0.3f, 0.3f), inRoomStones[prng.Next(inRoomStones.Length)], room.thisRoom);
                    MakeInRoomUnspawnable(x, y, 1);
                }
                if (decoGrid[x,y].inRoomSpawnable && prng.Next(101) > 85)
                {
                    Spawn(x, y, cacti[prng.Next(cacti.Length)], room.thisRoom);
                    MakeInRoomUnspawnable(x, y,2);
                }

                if (decoGrid[x, y].inRoomSpawnable && prng.Next(101) > 99)
                {
                    Spawn(x, y, skull, room.thisRoom);
                    MakeInRoomUnspawnable(x, y, 2);
                }

               
                //outRoomDeco
                if (decoGrid[x, y].BorderSpawnable && prng.Next(101) > 90 && RangeNotBorderCheck(x,y,room,2))
                {
                    Spawn(x, y, outRoomMiddleStones[prng.Next(outRoomMiddleStones.Length)], room.thisRoom);
                    MakeOutRoomUnspawnable(x, y, 2);
                }
            }
        }
    }

    void Spawn(float x, float y, GameObject toSpawn, GameObject parent)
    {
        Vector2 spawnPos = new Vector2(x, y);
        GameObject thing = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
        thing.transform.parent = parent.transform;
        thing.transform.localPosition = spawnPos;
    }

    void MakeInRoomUnspawnable(int posX, int posY, int range)
    {
        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                decoGrid[posX + x, posY + y].inRoomSpawnable = false;
            }
        }
    }

    void MakeOutRoomUnspawnable(int posX, int posY, int range)
    {
        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                decoGrid[posX + x, posY + y].BorderSpawnable = false;
            }
        }
    }
    bool RangeNotBorderCheck(int posX, int posY, RoomClass room, int range) // returns true if anything else then borderTiles are in given range
    {
        bool local = true;
        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                if (room.roomGrid[posX + x, posY + y].roomSpriteAbove != RoomClass.RoomSprite.Border) return local = false;
            }
        }
        return local;
    }

    bool MinRangeNotBorderCheck(int posX, int posY, RoomClass room)
    {
        bool local = true;
        for (int x = -minRangeonOutRoomDeco; x < minRangeonOutRoomDeco + 1; x++)
        {
            for (int y = -minRangeonOutRoomDeco; y < minRangeonOutRoomDeco + 1; y++)
            {
                if (room.roomGrid[posX + x,posY+ y].roomSpriteAbove != RoomClass.RoomSprite.Border) return local = false;
            }
        }
        return local;
    } //for big outRoomTiles
}
