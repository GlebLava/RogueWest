using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetHolder : MonoBehaviour
{
    
    public GameObject topWall, botWall, leftWall, rightWall;
    public GameObject floor, border, empty;
    public GameObject cornerTopLeft, cornerTopRight, cornerBotRight, cornerBotLeft;
    public GameObject twoTopBot, twoLeftRight;
    public GameObject threeBot, threeTop, threeLeft, threeRight;
    public GameObject pillar;

    
    public enum TileSetType { SpawnRoomTileset, DefaultRoomTileset }
    public TileSetType tileSetType;
}
