using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public  enum gridSpace { empty, floor, walltop, wallleft, wallbottom, wallright, bottomleftCorner, bottomrightCorner, topleftCorner, toprightCorner, twoTopBot, twoLeftRight,
    threeTop, threeLeft, threeRight, threeBot, pillar, playerSpawn
    };
    enum Directions { left, right, up, down }
    
    [System.NonSerialized]
    public gridSpace[,] grid;

    public int roomHeight = 30;
    public int roomWidth = 30;

    public GameObject floorObj, topWallObj, leftWallObj, rightWallObj, bottomWallObj, bottomleftCornerObj, topleftCornerObj, toprightCornerObj, bottomrightCorner, emptyObj,
    twoLeftRightObj, twoTopDownObj, threeTopObj, threeLeftObj, threeRightObj, threeBotObj, pillarObj, playerSpawnObj;


    public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
    public float chanceWalkerDestoy = 0.05f;
    public int maxWalkers = 10;
    [Range(0, 0.8f)]
    public float percentToFill = 0.5f;







    struct Walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    List<Walker> walkers;





    private void Start()
    {
   
        Setup();
        CreateFloors();
        CreateTripleFloors();
        CreateWallsUDLR();
        CreateCornerWalls();
        CreateTwoWalls();
        CreateThreeWalls();
        CreatePillars();
        DownestRightestCorner();
        SpawnLevel();
        
        SpawnPlayer();

    }

    void Setup()
    {

        grid = new gridSpace[roomWidth, roomHeight];
        for (int i = 0; i < roomWidth - 1; i++)
        {
            for (int j = 0; j < roomHeight - 1; j++)
                grid[i, j] = gridSpace.empty;
        }

        walkers = new List<Walker>();
        // first walker
        Walker newWalker = new Walker();
        // asign direction to first walker
        newWalker.dir = RandomDirection();

        //find center of made grid
        Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f), Mathf.RoundToInt(10f));
        //spawn walker in center of grid
        newWalker.pos = spawnPos;

        //add walker to list
        walkers.Add(newWalker);

    }
    void CreateFloors()
    {
        int limit = 0;//loop will not run forever
        do
        {
            //create floor at position of every walker
            foreach (Walker myWalker in walkers)
            {
                grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = gridSpace.floor;
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
            //chance: walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    Walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }
            //chance: spawn new walker
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
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 9, roomWidth - 10);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 9, roomHeight - 10);
                walkers[i] = thisWalker;
            }
            //check to exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            limit++;
        } while (limit < 100000);
    }




    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch (grid[x, y])
                {
                    case gridSpace.empty:
                        Spawn(x, y, emptyObj);
                        break;
                    case gridSpace.floor:
                        Spawn(x, y, floorObj);
                        break;
                    case gridSpace.wallleft:
                        Spawn(x, y, leftWallObj);
                        break;
                    case gridSpace.wallright:
                        Spawn(x, y, rightWallObj);
                        break;
                    case gridSpace.walltop:
                        Spawn(x, y, topWallObj);
                        break;
                    case gridSpace.wallbottom:
                        Spawn(x, y, bottomWallObj);
                        break;
                    case gridSpace.bottomleftCorner:
                        Spawn(x, y, bottomleftCornerObj);
                        break;
                    case gridSpace.bottomrightCorner:
                        Spawn(x, y, bottomrightCorner);
                        break;
                    case gridSpace.topleftCorner:
                        Spawn(x, y, topleftCornerObj);
                        break;
                    case gridSpace.toprightCorner:
                        Spawn(x, y, toprightCornerObj);
                        break;
                    case gridSpace.twoTopBot:
                        Spawn(x, y, twoTopDownObj);
                        break;
                    case gridSpace.twoLeftRight:
                        Spawn(x, y, twoLeftRightObj);
                        break;
                    case gridSpace.threeBot:
                        Spawn(x, y, threeBotObj);
                        break;
                    case gridSpace.threeTop:
                        Spawn(x, y, threeTopObj);
                        break;
                    case gridSpace.threeLeft:
                        Spawn(x, y, threeLeftObj);
                        break;
                    case gridSpace.threeRight:
                        Spawn(x, y, threeRightObj);
                        break;
                    case gridSpace.pillar:
                        Spawn(x, y, pillarObj);
                        break;
                    case gridSpace.playerSpawn:
                        Spawn(x, y, playerSpawnObj);
                        break;
                }
            }
        }
    }

    void CreateTripleFloors()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (grid[x, y] == gridSpace.floor)
                {

                    if (grid[x - 1, y] == gridSpace.empty && grid[x + 1, y] == gridSpace.empty)
                    {
                        grid[x - 1, y] = gridSpace.floor;
                       // grid[x + 1, y] = gridSpace.floor;
                    }

                    if (grid[x, y - 1] == gridSpace.empty && grid[x, y + 1] == gridSpace.empty)
                    {
                        grid[x, y - 1] = gridSpace.floor;
                        //grid[x, y + 1] = gridSpace.floor;
                    }



                    }

            }
        }
    }
    void CreateWallsUDLR()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (grid[x, y] == gridSpace.floor)
                {

                    if (grid[x - 1, y] == gridSpace.empty) grid[x - 1, y] = gridSpace.wallright;
                    if (grid[x + 1, y] == gridSpace.empty) grid[x + 1, y] = gridSpace.wallleft;
                    if (grid[x, y - 1] == gridSpace.empty) grid[x, y - 1] = gridSpace.wallbottom;
                    if (grid[x, y + 1] == gridSpace.empty) grid[x, y + 1] = gridSpace.walltop;
                }

            }
        }
    }

    void CreateCornerWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (WallCheck(x,y))
                {

                    if (grid[x - 1, y] == gridSpace.floor && grid[x, y + 1] == gridSpace.floor && EmptyandWallandCornerCheck(x, y - 1) && EmptyandWallandCornerCheck(x + 1, y))
                        grid[x, y] = gridSpace.topleftCorner;

                    if (grid[x , y+1] == gridSpace.floor && grid[x+1, y] == gridSpace.floor && EmptyandWallandCornerCheck(x, y - 1) && EmptyandWallandCornerCheck(x -1, y))
                        grid[x, y] = gridSpace.toprightCorner;

                    if (grid[x -1, y] == gridSpace.floor && grid[x, y -1] == gridSpace.floor && EmptyandWallandCornerCheck(x, y+1) && EmptyandWallandCornerCheck(x + 1, y))
                        grid[x, y] = gridSpace.bottomleftCorner;
                    if (grid[x +1, y] == gridSpace.floor && grid[x, y - 1] == gridSpace.floor && EmptyandWallandCornerCheck(x, y+1) && EmptyandWallandCornerCheck(x -1, y))
                        grid[x, y] = gridSpace.bottomrightCorner;

                }



            }
        }
    }

    void CreateTwoWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (WallCheck(x, y) || CornerCheck(x, y))
                {

                    if (grid[x -1, y] == gridSpace.floor && grid[x + 1, y] == gridSpace.floor && EmptyandWallandCornerandTwoCheck(x, y-1) && EmptyandWallandCornerandTwoCheck(x, y+1))
                        grid[x, y] = gridSpace.twoLeftRight;

                    if (grid[x, y + 1] == gridSpace.floor && grid[x, y + -1] == gridSpace.floor && EmptyandWallandCornerandTwoCheck(x+1,y) && EmptyandWallandCornerandTwoCheck(x-1,y))
                        grid[x, y] = gridSpace.twoTopBot;
                    
                }

            }
        }

    }

    void CreateThreeWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (WallCheck(x, y) || CornerCheck(x, y) || TwoWallCheck(x,y) || ThreeWallCheck(x, y))
                {

                    if (ThreeEmpty(Directions.down, x, y))
                        grid[x, y] = gridSpace.threeBot;
                    if (ThreeEmpty(Directions.left, x, y))
                        grid[x, y] = gridSpace.threeLeft;
                    if (ThreeEmpty(Directions.right, x, y))
                        grid[x, y] = gridSpace.threeRight;
                    if (ThreeEmpty(Directions.up, x, y))
                        grid[x, y] = gridSpace.threeTop;

                }

            }
        }

    }

    void CreatePillars()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)

            {
                if (CornerCheck(x, y) || WallCheck(x, y))
                {

                    if (grid[x - 1, y] == gridSpace.floor && grid[x + 1, y] == gridSpace.floor && grid[x, y + 1] == gridSpace.floor && grid[x, y - 1] == gridSpace.floor)
                        grid[x, y] = gridSpace.pillar;

                }



            }
        }

    }


    void Spawn(float x, float y, GameObject toSpawn)
    {
  
        Vector2 spawnPos = new Vector2(x, y);
        //spawn object
        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    void DownestRightestCorner()
    {

        for (int x = roomWidth - 1 ; x > roomWidth/2 ; x--)
        {
  
            for (int y = 0; y < (roomHeight - 1) / 2; y++)

            {

                if (grid[x, y] == gridSpace.floor)
                {


                    grid[x, y] = gridSpace.playerSpawn;

                    return;
                }

            }
        }
    }

    void SpawnPlayer()
    {
        //vorrübergehend
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
        player.transform.position = spawnPoint.transform.position;
        enemy.transform.position = new Vector3(spawnPoint.transform.position.x - 2, spawnPoint.transform.position.y - 1 ,0) ;

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

    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.floor)
            {
                count++;
            }
        }
        return count;
    }

    //all the Checks start
    //
    //
    bool EmptyCheck(int posX, int posY)
    {
        if (grid[posX, posY] == gridSpace.empty) return true;
        else return false;
    }
    bool WallCheck(int posX, int posY)
    {
        if (grid[posX, posY] == gridSpace.wallbottom || grid[posX, posY] == gridSpace.wallleft || grid[posX, posY] == gridSpace.wallright ||
            grid[posX, posY] == gridSpace.walltop) return true;
        else return false;
    }
   
    bool CornerCheck(int posX, int posY)
    {
        if (grid[posX, posY] == gridSpace.bottomleftCorner || grid[posX, posY] == gridSpace.bottomrightCorner
            || grid[posX, posY] == gridSpace.topleftCorner || grid[posX, posY] == gridSpace.toprightCorner) return true;
        else return false;
    }

    bool TwoWallCheck(int posX, int posY)
    {
        if (grid[posX, posY] == gridSpace.twoTopBot || grid[posX, posY] == gridSpace.twoLeftRight) return true;
        else return false;
    }
    bool ThreeWallCheck(int posX, int posY)
    {
        if (grid[posX, posY] == gridSpace.threeBot || grid[posX, posY] == gridSpace.threeTop
            || grid[posX, posY] == gridSpace.threeLeft || grid[posX, posY] == gridSpace.threeRight) return true;
        else return false;
            

    }
 
    //
    //
    //all the Checks by themselves end
    // Checkcombos start
    //
    //
    bool EmptyandWallandCornerCheck(int posX, int posY)
    {
        if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY))
            return true;
        else return false;
    }
     bool EmptyandWallandCornerandTwoCheck(int posX, int posY)
    {
        if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX,posY))
            return true;
        else return false;
    }

    bool WallandCornerandTwoandThreeCheck(int posX, int posY)
    {
        if (EmptyCheck(posX, posY) || WallCheck(posX, posY) || CornerCheck(posX, posY) || TwoWallCheck(posX, posY) || ThreeWallCheck(posX, posY))
            return true;
        else return false;
    }

    //Checkcombos end
    //
    //





    bool ThreeEmpty(Directions dir, int posX, int posY)
        {
         switch (dir)
        {
            case Directions.down:
                if (WallandCornerandTwoandThreeCheck(posX, posY+1)  && grid[posX, posY - 1] == gridSpace.floor && grid[posX + 1 , posY ] == gridSpace.floor
                    && grid[posX - 1, posY] == gridSpace.floor)
                    return true;
                break;
            case Directions.up:
                if (WallandCornerandTwoandThreeCheck(posX, posY - 1) && grid[posX, posY + 1] == gridSpace.floor && grid[posX + 1, posY] == gridSpace.floor
                    && grid[posX - 1, posY] == gridSpace.floor)
                    return true;
                break;
            case Directions.left:
                if (WallandCornerandTwoandThreeCheck(posX + 1, posY) && grid[posX, posY - 1] == gridSpace.floor && grid[posX - 1, posY] == gridSpace.floor
                    && grid[posX, posY + 1] == gridSpace.floor)
                    return true;
                break;
            case Directions.right:
                if (WallandCornerandTwoandThreeCheck(posX - 1, posY) && grid[posX, posY - 1] == gridSpace.floor && grid[posX, posY +1] == gridSpace.floor
                    && grid[posX + 1, posY] == gridSpace.floor)
                    return true;
                break;
            default:
                return false;
        }

        return false;


        }













}

