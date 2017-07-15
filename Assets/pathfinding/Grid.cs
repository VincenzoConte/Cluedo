using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	Vector2 gridWorldSize;
	float nodeRadius;
	Node[,] grid;
    public Transform bottomRight;
    public GameObject target;

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
        CreateDoors();
	}

    void CreateDoors()
    {
        grid[6, 6].room = true;
        grid[6, 8].room = true;
        grid[8,12].room = true;
        grid[11,7].room = true;
        grid[12,7].room = true;
        grid[17,4].room = true;
        grid[16,8].room = true;
        grid[20,11].room = true;
        grid[22,11].room = true;
        grid[17,15].room = true;
        grid[4, 17].room = true;
        grid[7, 19].room = true;
        grid[9, 16].room = true;
        grid[14, 16].room = true;
        grid[16, 19].room = true;
        grid[18, 18].room = true;
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

    public void DrawTargets()
    {
        //testing
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("target"))
            GameObject.Destroy(t, 0f);
        //end testing
        foreach(Node n in path)
        {
            GameObject clone=GameObject.Instantiate(
                target,
                n.worldPosition,
                new Quaternion());
            clone.transform.localScale = new Vector3(nodeDiameter - .8f, 0.2f, nodeDiameter - .8f);
            clone.tag = "target";
            //clone.GetComponent<MoveOnClick>().parent = n;
        }
    }

	void OnDrawGizmos() {
        //Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
        if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
				/*if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;*/
                if(n.room)
                    Gizmos.color = Color.blue;
                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - .8f, 0.1f, nodeDiameter - .8f));
			}
		}

        //Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1) * (nodeDiameter - .1f));
    }
}