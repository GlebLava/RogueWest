using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding
{
    private RoomClass.RoomTile[,] roomTileGrid;

    private Node[,] nodeGrid;
    private int gridWidth;
    private int gridHeight;
    private int gridMaxSize;

    private Vector2 bottomLeftCornerGlobalPosition;
    private DecorationEventHandler myDecorationEventHandler;


    public Pathfinding(RoomClass room)
    {
        roomTileGrid = room.roomGrid;

        gridWidth = roomTileGrid.GetLength(0);
        gridHeight = roomTileGrid.GetLength(1);
        nodeGrid = new Node[gridWidth, gridHeight];
        myDecorationEventHandler = room.thisRoomsdecorationEventHandler;
        bottomLeftCornerGlobalPosition = room.globalPosition;

        gridMaxSize = gridHeight * gridWidth;

        myDecorationEventHandler.OnDecoationWithColliderDestroyed += WalkableChanged;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 thisNodesGlobalPosition = new Vector2(room.globalPosition.x + x, room.globalPosition.y + y);
                bool local_walkable;

                if (RoomClass.FloorSpriteCheck(roomTileGrid[x, y])) local_walkable = true;
                else local_walkable = false;

                nodeGrid[x, y] = new Node(local_walkable, thisNodesGlobalPosition, x, y);
            }
        }
    }

    private List<Node> FindPathNodes(Vector2 startingPosition, Vector2 targetPosition)
    {
        Node startNode = NodeFromGlobalorLocalPosition(startingPosition);
        Node targetNode = NodeFromGlobalorLocalPosition(targetPosition);

        if (startNode == targetNode) return null;

        if (!targetNode.walkable) targetNode = FindNearestNode(targetNode, targetPosition);

        if (targetNode == null)
        {
            return null;
        }



        Heap<Node> openSet = new Heap<Node>(gridMaxSize);
        Heap<Node> closedSet = new Heap<Node>(gridMaxSize);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();

           
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                }
            }
        }


        return null; //shouldnt reach this

    }

    public List<Vector2> FindPath(Vector2 startingPosition, Vector2 targetPosition)
    {
        List<Node> path = FindPathNodes(startingPosition, targetPosition);

        if (path == null) return null;

        List<Vector2> vectorPath = new List<Vector2>();

        foreach (Node node in path)
        {
            vectorPath.Add(node.globalPosition);
        }
        return vectorPath;
    }

    private void WalkableChanged(Vector2 position, bool _walkable)
    {
        NodeFromGlobalorLocalPosition(position).walkable = _walkable;
    }

    //help Methods
    public Node NodeFromGlobalorLocalPosition(Vector2 Position) // This only works when the Enemy has the Room's GameObject as a parent
    {
        try
        {
            int x = Mathf.RoundToInt(Position.x) - 1;
            int y = Mathf.RoundToInt(Position.y) - 1;

            return nodeGrid[x, y];
        }
        catch (IndexOutOfRangeException)
        {
            int x = Mathf.RoundToInt(Position.x - bottomLeftCornerGlobalPosition.x);
            int y = Mathf.RoundToInt(Position.y - bottomLeftCornerGlobalPosition.y);
            return nodeGrid[x, y];
        }
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
                {
                    neighbours.Add(nodeGrid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY) return 14 * distanceY + 10 * (distanceX - distanceY);
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    Node FindNearestNode(Node targetNode, Vector2 targetPosition)
    {

        int startRangeX = -targetNode.gridX;
        int endRangeX = gridWidth - targetNode.gridX;
        int startRangeY = -targetNode.gridY;
        int endRangeY = gridHeight - targetNode.gridY;

        int xMin = -1;
        int yMin = -1;

        int xMax = 1;
        int yMax = 1;

        int safetyCounter = 0;
        int endState = gridHeight + gridWidth;

        List<Node> availableNodes = new List<Node>();


        bool startAddingToList = false;
        while (safetyCounter < endState)
        {



            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    try { 
                    if (nodeGrid[targetNode.gridX + x, targetNode.gridY + y].walkable)
                        startAddingToList = true;
                    } catch (IndexOutOfRangeException)
                    {
                        return null;
                    }
                    if (startAddingToList && nodeGrid[targetNode.gridX + x, targetNode.gridY + y].walkable)
                        availableNodes.Add(nodeGrid[targetNode.gridX + x, targetNode.gridY + y]);
                }
            }

            if (startAddingToList)
                return FindNodeWithLowestDistance(availableNodes, targetPosition);

            if (xMin > startRangeX)
                xMin--;

            if (xMax < endRangeX)
                xMax++;

            if (yMin > startRangeY)
                yMin--;

            if (yMax < endRangeY)
                yMax++;

            safetyCounter++;
        }


        return null;
    } //targetNode stands for the UnwalkableNode to find the nearest walkable node to 

    Node FindNodeWithLowestDistance(List<Node> nodes, Vector2 targePosition)
    {
        int localLenght = nodes.Count;
        float[] distances = new float[localLenght];

        for (int i = 0; i < localLenght; i++)
        {
            distances[i] = Vector2.Distance(targePosition, nodes[i].globalPosition);
        }


        float lowestDistance = distances[0];
        int index = 0;

        for (int i = 1; i < localLenght; i++)
        {
            if (lowestDistance > distances[i])
            {
                lowestDistance = distances[i];
                index = i;
            }
        }

        return (nodes[index]);

    }
}
