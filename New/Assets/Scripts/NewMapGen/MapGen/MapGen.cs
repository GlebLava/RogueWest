using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public GameObject obj;
    public GameObject obj2;
    public GameObject obj3;

    public int width, height;
    public int maxRooms = 10;
   
    [System.NonSerialized]
    public int[,] grid;

    private float chanceWalkerDestoy = 0.3f;
    private float chanceWalkerChangeDir = 0.15f;
    private int maxWalkers = 5;
    private float chanceWalkerSpawn = 0.3f;



    struct Walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    List<Walker> walkers;

    void Start()
    {
        Setup();
        CreateBasicLayout();
        DeleteRandomRooms();
        AddRooms();
        AddRooms();
        DeleteMiddleRooms();
        DeleteUnreachableRooms();
        //Visualize();

        Debug.Log(NumberOfRooms());
    }

    void Setup()
    {
        grid = new int[width, height];
        for (int x = 0; x < width ; x++)
        {
            for (int y = 0; y < height ; y++)
            {
                grid[x, y] = -1;
            }
        }

        walkers = new List<Walker>();
        // first walker
        Walker newWalker = new Walker();
        // asign direction to first walker
        newWalker.dir = RandomDirection();

        //find center of made grid
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(width / 2.0f), Mathf.RoundToInt(10f));
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
                grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = 1;
            }
            //chance: destroy walker
            int numberChecks = walkers.Count; //might modify count while in this loop
            
            for (int i = 0; i < numberChecks; i++)
            {
                //only if its not the only one, and at a low chance
                if (Random.value < chanceWalkerDestoy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; //only destroy one per iteration
                }
            }

            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
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
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
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
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 5, width - 5);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 5, height - 5);
                walkers[i] = thisWalker;
            }
            //check to exit loop
            if ((float)NumberOfRooms() > maxRooms)
            {
                break;
            }

            limit++;
        } while (limit < 10000);

    }

    void DeleteRandomRooms()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1 && Random.value < 0.7)
                {
  
                    if (NumberOfNeighboors2(x, y, 1, 2) == 8)
                    {
                        grid[x, y] = 2;

                    }
                    

                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 2)
                {
                    grid[x, y] = -1;
                }
            }
        }

    }

    void AddRooms()
    {
        for (int x = 2; x < width - 3; x++)
        {
            for (int y = 1; y < height - 2; y++)
            {
                if (grid[x, y] == -1)
                {
                    if (NumberOfNeighboors2(x,y,1, 1) > 1 &&  (Random.value < 0.2))
                    {
                        grid[x, y] = 1;
                        
                    }
                    
                }
            }
        }

    }

    void DeleteMiddleRooms()
    {
        for (int x = 2; x < width - 3; x++)
        {
            for (int y = 1; y < height - 2; y++)
            {
                if (grid[x, y] == 1)
                {
                    if (NumberOfNeighboors2(x, y, 1, 2) > 7 )
                    {
                        grid[x, y] = 2;

                    }

                }
            }
        }
    }

    void DeleteUnreachableRooms()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1)
                {
                    if (grid[x - 1, y] != 1 && grid[x + 1, y] != 1 && grid[x, y - 1] != 1 && grid[x, y + 1] != 1)
                         grid[x, y] = -1;
                }
                
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 2)
                {
                    grid[x, y] = -1;
                }

            }
        }

    }
 


    void Visualize()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1)
                {
                    Vector2 spawnPos = new Vector2(x, y);
                    Instantiate(obj, spawnPos, Quaternion.identity);
                }

                if (grid[x, y] == -1)
                {
                    Vector2 spawnPos1 = new Vector2(x, y);
                    Instantiate(obj2, spawnPos1, Quaternion.identity);
                }

                if (grid[x, y] == 2)
                {
                    Vector2 spawnPos1 = new Vector2(x, y);
                    Instantiate(obj3, spawnPos1, Quaternion.identity);
                }
            }
        }
    }



    Vector2 RandomDirection()
    {
        //pick random int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
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

    int NumberOfRooms()
    {
        int count = 0;
        foreach (int space in grid)
        {
            if (space == 1)
            {
                count++;
            }
        }
        return count;
    }

    int NumberOfNeighboors2(int posX,int posY, int typeOfNeighbour, int typeOfNeighbour2)
    {
        int count = 0;

        for ( int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
                { 
                if (grid[posX + x, posY + y] == typeOfNeighbour || grid[posX + x, posY + y] == typeOfNeighbour2)
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

}
