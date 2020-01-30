using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    public GameObject[] gameObjects;

    private void Awake()
    {
        Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], transform.position, Quaternion.identity);
    }
}
