﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	Vector2 gridWorldSize;
	float nodeRadius;
	Node[,] grid;
    public Transform bottomRight;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
        nodeRadius = (-transform.position.x+bottomRight.position.x) / 48;
		nodeDiameter = nodeRadius*2;
		gridSizeX = 24;
		gridSizeY = 25;
        gridWorldSize = new Vector2(24 * nodeDiameter, 25 * nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		//Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        Vector3 worldBottomLeft = transform.position;

        for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint ,nodeRadius-0.4f ,unwalkableMask));
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if ((x == 0 && y == 0) || (x != 0 && y != 0))
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	public List<Node> path;

	void OnDrawGizmos() {
        //Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
        if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, new Vector3(1,0.1f,1) * (nodeDiameter-.8f));
			}
		}

        //Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1) * (nodeDiameter - .1f));
    }
}