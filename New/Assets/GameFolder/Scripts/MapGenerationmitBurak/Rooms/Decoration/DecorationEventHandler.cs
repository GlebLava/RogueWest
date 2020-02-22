using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationEventHandler : MonoBehaviour
{

    public event Action<Vector2 , bool> OnDecoationWithColliderDestroyed;


    public void DecorationIsThere(Vector2 position)
    {
        OnDecoationWithColliderDestroyed?.Invoke(position, false);
    }

    public void DecorationGotDestroyed(Vector2 position)
    {
        OnDecoationWithColliderDestroyed?.Invoke(position, true);
    }

}
