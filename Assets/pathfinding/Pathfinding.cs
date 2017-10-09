﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Pathfinding : MonoBehaviour {

	public Transform seeker;
    SwitchCamera changeView;
    public dice dado1, dado2;
	Grid grid;
	GameObject colliderr;
	public OperativaInterfaccia oi;

	void OnEnable() {
        changeView = GameObject.Find("Gestione camera").GetComponent<SwitchCamera>();
        grid = GetComponent<Grid> ();
		colliderr = GameObject.Find ("ColliderDadi");
    }

	void Update() {
        if (dado1.value > 0 && dado2.value > 0)
        {
            if (oi.IsMyTurn())
            {
                FindTargets(seeker.position, dado1.value + dado2.value);
                changeView.ActivePlayerView();
                //Setto invisibili i dadi e collider
                dado1.gameObject.SetActive(false);
                dado2.gameObject.SetActive(false);
                colliderr.gameObject.SetActive(false);
            }
            dado1.value = 0;
            dado2.value = 0;
        }
	}

	public void FindPath(Vector3 targetPos) {
        Vector3 startPos = seeker.position;
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
        if (Room.CheckNode(startNode))
        {
            List<Node> doors = grid.FindRoom(startNode).doors;
            Node min = doors[0];
            foreach (Node n in doors)
            {
                if (GetDistance(n, targetNode) < GetDistance(min, targetNode))
                    min = n;
            }
            //seeker.position = new Vector3(min.worldPosition.x, seeker.position.y, min.worldPosition.z);
            seeker.transform.DOMove(new Vector3(min.worldPosition.x, seeker.position.y, min.worldPosition.z), 2);
            startNode = min;
        }
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
		oi.setMiSonoSpostato ();
    }

    public void MoveInRoom(Room room)
    {
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("target"))
            GameObject.Destroy(t, 0f);
        Node n = room.GetWalkableNode();
        if (n != null)
        {
            Sequence seq = DOTween.Sequence();
            //seeker.transform.position = new Vector3(n.worldPosition.x, seeker.position.y, n.worldPosition.z);
            seq.Append(seeker.transform.DOMove(new Vector3(n.worldPosition.x, seeker.position.y, n.worldPosition.z), 2));
            Transform camera = seeker.GetChild(0);
            seq.Append(camera.DOLocalMove(new Vector3(0, 1, 0), 2));
            seq.Join(camera.DOLocalRotate(new Vector3(0, 0, 0), 2));
            seq.Append(camera.DOLocalRotate(new Vector3(0, 360, 0), 10, RotateMode.FastBeyond360));
            seq.Append(camera.DOLocalMove(new Vector3(0, 10, 0), 2));
            seq.Join(camera.DOLocalRotate(new Vector3(90, 0, 0), 2));

        }
		oi.setMiSonoSpostato ();
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
        //Debug.Log(startNode.gridX + " " + startNode.gridY);
        List<Node> openSet = new List<Node>();
        List<Node> targets = new List<Node>();
        Room room  = null;
        if (Room.CheckNode(startNode))
        {
            room = grid.FindRoom(startNode);
            foreach (Node n in room.doors)
            {
                n.position = 1;
                openSet.Add(n);
            }        
        }
        else
        {
            startNode.position = 0;
            openSet.Add(startNode);
        }

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].position < node.position)
                    node = openSet[i];
            }

            openSet.Remove(node);
            bool door = false;
            if (node.room != null)
            {
                if ((room == null || !room.Equals(node.room)) && node.position < length && !node.room.draw)
                {
                    Node n = node.room.GetWalkableNode();
                    if (n != null)
                    {
                        n.drawRoom = true;
                        n.room = node.room;
                        n.room.draw = true;
                        targets.Add(n);
                    }
                }
                door = true;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (neighbour.walkable && !targets.Contains(neighbour))
                {
                    bool isInRoom=false;
                    if (door && Room.CheckNode(neighbour))
                    {
                        isInRoom = true;
                    }

                    //int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (!isInRoom && (!openSet.Contains(neighbour) || node.position+1 < neighbour.position))
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
