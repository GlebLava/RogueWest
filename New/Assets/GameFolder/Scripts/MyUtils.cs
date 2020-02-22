using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtils : MonoBehaviour
{
    public static GameObject Spawn(float x, float y, GameObject toSpawn, GameObject parent)
    {
        Vector2 spawnPos = new Vector2(x, y);
        GameObject thing = Instantiate(toSpawn, parent.transform);
        thing.transform.localPosition = spawnPos;
        return thing;
    }

    public static GameObject Spawn(float x, float y, GameObject toSpawn)
    {
        GameObject thing = Instantiate(toSpawn, new Vector2(x,y), Quaternion.identity);   
        return thing;
    }

    public static GameObject MakeEmptyGameObject(string name)
    {
        GameObject deleteThis = new GameObject(name);
        Destroy(deleteThis);
        return deleteThis;
    }

   

}
