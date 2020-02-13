using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRand : MonoBehaviour
{
    SpriteRenderer sR;
    public Sprite sprite1;
    public Sprite sprite2;
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        int rand = Random.Range(1, 3);
        if (rand == 1) sR.sprite = sprite1;
        else sR.sprite = sprite2; 
    }

   
}
