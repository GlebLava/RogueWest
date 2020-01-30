using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    private MapGenerator mapGen;
    private MapGenerator.gridSpace[,] grid;



    private void Awake()
    {
        mapGen = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        grid = mapGen.grid;
    }
    void Update()
    {
        


    }
}
