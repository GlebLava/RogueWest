using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHolder : MonoBehaviour
{
    public GameObject chest;


    public void SpawnRandomChest(Vector2 position, GameObject parent)
    {
         MyUtils.Spawn(position.x, position.y ,chest, parent);
    }
}
