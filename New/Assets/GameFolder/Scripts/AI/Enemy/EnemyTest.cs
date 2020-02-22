using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private Pathfinding myPathfinding;

    public GameObject player;

    private RoomClass roomImIn;


    public void AssignRoom(RoomClass room)
    {
        roomImIn = room;

        myPathfinding = new Pathfinding(room);
        
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            List<Vector2> path = myPathfinding.FindPath(transform.position, player.transform.position);
            if (path != null)
            {
                Debug.DrawLine(transform.position, path[0], Color.red);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.red);
                }
            }
        }
    }


    void FollowPath(List<Vector2> path)
    {

    }


}
