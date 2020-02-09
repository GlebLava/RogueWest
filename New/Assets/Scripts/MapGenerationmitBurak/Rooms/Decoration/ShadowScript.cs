using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    GameObject player;
    SpriteRenderer sr;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y > transform.position.y + 0.85)
        {
            sr.sortingLayerName = "ShadowInFrontPlayer";
        }
        else sr.sortingLayerName = "ShadowBehindPlayer";
    }
}
