using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRooms : MonoBehaviour
{


    private MapGen mapGenerator;
    private int[,] grid;

   
    void Start()
    {
       
        mapGenerator = GetComponent<MapGen>();
        grid = new int[mapGenerator.width, mapGenerator.height];
        grid = mapGenerator.grid;
       
        
        SpawnRooms();
    }


    void SpawnRooms()
    {
        for (int x = 2; x < mapGenerator.width - 2; x++) 
        {
            for (int y = 2; y < mapGenerator.height - 2; y++)
            {

                if (grid[x, y] == 1)
                {

                   

                }
            }
        }
    }
    void CheckEntrances()
    {
        for (int x = 2; x < grid.GetLength(0) - 2; x++)
        {
            for (int y = 2; x < grid.GetLength(1) - 2; y++)
            {
                if (grid[x,y] == 1)
                {

                }
            }
        }

    }


    void Spawn(float x, float y, GameObject toSpawn)
    {

        Vector2 spawnPos = new Vector2(transform.position.x + x, transform.position.y + y);
        //spawn object
        GameObject thisroom = Instantiate(toSpawn, spawnPos, Quaternion.identity) as GameObject;
        thisroom.transform.parent = transform;
    }

    bool CheckDirectionDown(int posX,int posY)
    {
        if (grid[posX, posY - 1] == 1) return true;
        else return false;
    }

    bool CheckDirectionUp(int posX, int posY)
    {
        if (grid[posX, posY +1] == 1) return true;
        else return false;
    }

    bool CheckDirectionLeft(int posX, int posY)
    {
        if (grid[posX - 1, posY] == 1) return true;
        else return false;
    }

    bool CheckDirectionRight(int posX, int posY)
    {
        if (grid[posX + 1, posY] == 1) return true;
        else return false;
    }
}
