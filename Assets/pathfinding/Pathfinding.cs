﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Pathfinding : MonoBehaviour {

	public Transform seeker;
    public int length;
	Grid grid;

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	void Update() {
		FindTargets (seeker.position, length);
	}

	public void FindPath(Vector3 targetPos) {
        Vector3 startPos = seeker.position;
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
                targetNode.walkable = false;
                startNode.walkable = true;
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node)) {
                if (neighbour.walkable && !closedSet.Contains(neighbour))
                {

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (!openSet.Contains(neighbour) || newCostToNeighbour < neighbour.gCost)
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
        Move(path);
        //testing
        //seeker.transform.position = endNode.worldPosition;

	}

    void Move(List<Node> path)
    {
        //cammino
        /*foreach (GameObject t in GameObject.FindGameObjectsWithTag("target"))
            GameObject.Destroy(t, 0f);
        Vector3[] points= new Vector3[path.Count];
        int i = 0;
        float y = seeker.position.y;
        foreach(Node n in path)
        {
            points[i] = new Vector3(n.worldPosition.x, y, n.worldPosition.z);
            i++;
        }
        seeker.transform.DOPath(points, path.Count, PathType.CatmullRom, PathMode.Full3D, 5);*/

        //salti
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("target"))
            GameObject.Destroy(t, 0f);
        Sequence seq = DOTween.Sequence();
        float y = seeker.position.y;
        foreach (Node n in path)
        {
            seq.Append(seeker.DOJump(new Vector3(n.worldPosition.x, y, n.worldPosition.z),1 , 1, 1));
        }


    }

    public void MoveInRoom(Room room)
    {
        Node n = room.GetWalkableNode();
        if (n != null)
        {
            seeker.transform.position = n.worldPosition;
        }
    }

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

    //cerca celle raggiungibili con length spostamenti
    void FindTargets(Vector3 startPos, int length)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);

        List<Node> openSet = new List<Node>();
        List<Node> targets = new List<Node>();
        startNode.position = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].position < node.position)
                    node = openSet[i];
            }

            openSet.Remove(node);
            bool room = false;
            if (node.room != null)
            {
                if (node.position < length)
                    node.drawRoom = true;
                room = true;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (neighbour.walkable && !targets.Contains(neighbour))
                {
                    bool isInRoom=false;
                    if (room && Room.CheckNode(neighbour))
                    {
                        isInRoom = true;
                    }

                    //int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (!isInRoom && (!openSet.Contains(neighbour) || /*newCostToNeighbour < neighbour.gCost*/ node.position+1 < neighbour.position))
                    {
                        //neighbour.gCost = newCostToNeighbour;
                        //neighbour.hCost = 0;
                        //neighbour.parent = node;
                        neighbour.position = node.position + 1;
                        if (neighbour.position == length)
                        {
                            targets.Add(neighbour);
                            openSet.Remove(neighbour);
                        }
                        else if (neighbour.position > length)
                        {
                            openSet.Remove(neighbour);
                        }
                        else if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        targets.Remove(startNode);
        grid.path = targets;
        grid.DrawTargets();
    }

}
