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
    public GameObject roomTarget;

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
        Room.radius = nodeRadius;
        Room salotto = new Room("salotto", GameObject.Find("area salotto").transform);
        Room pranzo = new Room("sala da pranzo", GameObject.Find("area pranzo").transform);
        Room ingresso = new Room("ingresso", GameObject.Find("area ingresso").transform);
        Room studio = new Room("studio", GameObject.Find("area studio").transform);
        Room biblioteca = new Room("biblioteca", GameObject.Find("area salotto").transform);
        Room cucina = new Room("cucina", GameObject.Find("area cucina").transform);
        Room serra = new Room("serra", GameObject.Find("area serra").transform);
        Room biliardo = new Room("sala del biliardo", GameObject.Find("area biliardo").transform);
        Room ballo = new Room("sala da ballo", GameObject.Find("area ballo").transform);

        grid[6, 6].room = salotto;
        grid[6, 8].room = pranzo;
        grid[8, 12].room = pranzo;
        grid[11, 7].room = ingresso;
        grid[12, 7].room = ingresso;
        grid[17, 4].room = studio;
        grid[16, 8].room = biblioteca;
        grid[20,11].room = biblioteca;
        grid[22,11].room = biliardo;
        grid[17,15].room = biliardo;
        grid[4, 17].room = cucina;
        grid[7, 19].room = ballo;
        grid[9, 16].room = ballo;
        grid[14, 16].room = ballo;
        grid[16, 19].room = ballo;
        grid[18, 18].room = serra;
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
            if (n.drawRoom)
            {
                n.drawRoom = false;
                clone = GameObject.Instantiate(
                roomTarget,
                new Vector3(n.room.area.position.x, 0.2f, n.room.area.position.z),
                new Quaternion());
                clone.transform.localScale =n.room.area.localScale;
                clone.GetComponent<MoveInRoom>().room = n.room;
                clone.tag = "target";
            }
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
                /*if(n.room)
                    Gizmos.color = Color.blue;*/
                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - .8f, 0.1f, nodeDiameter - .8f));
			}
		}

        //Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1) * (nodeDiameter - .1f));
    }
}