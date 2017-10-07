using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public float nodeRadius;
	Node[,] grid;
    public Transform bottomRight;
    public GameObject target;
    public GameObject roomTarget;
    Room[] rooms;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {

        rooms = new Room[9];
        rooms[0] = new Room("Salotto", GameObject.Find("area salotto").transform);
        rooms[1] = new Room("Sala da pranzo", GameObject.Find("area pranzo").transform);
        rooms[2] = new Room("Ingresso", GameObject.Find("area ingresso").transform);
        rooms[3] = new Room("Studio", GameObject.Find("area studio").transform);
        rooms[4] = new Room("Biblioteca", GameObject.Find("area biblioteca").transform);
        rooms[5] = new Room("Cucina", GameObject.Find("area cucina").transform);
        rooms[6] = new Room("Serra", GameObject.Find("area serra").transform);
        rooms[7] = new Room("Sala del biliardo", GameObject.Find("area biliardo").transform);
        rooms[8] = new Room("Sala da ballo", GameObject.Find("area ballo").transform);
        nodeRadius = (-transform.position.x+bottomRight.position.x) / 48;
		nodeDiameter = nodeRadius*2;
		gridSizeX = 24;
		gridSizeY = 25;
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
        rooms[0].doors.Add(grid[6, 6]);
        grid[6, 6].room = rooms[0];

        rooms[1].doors.Add(grid[6, 8]);
        rooms[1].doors.Add(grid[8, 12]);
        grid[6, 8].room = rooms[1];
        grid[8, 12].room = rooms[1];

        rooms[2].doors.Add(grid[11, 7]);
        rooms[2].doors.Add(grid[12, 7]);
        grid[11, 7].room = rooms[2];
        grid[12, 7].room = rooms[2];

        rooms[3].doors.Add(grid[17, 4]);
        grid[17, 4].room = rooms[3];

        rooms[4].doors.Add(grid[16, 8]);
        rooms[4].doors.Add(grid[20, 11]);
        grid[16, 8].room = rooms[4];
        grid[20,11].room = rooms[4];

        rooms[7].doors.Add(grid[22, 11]);
        rooms[7].doors.Add(grid[17, 15]);
        grid[22,11].room = rooms[7];
        grid[17,15].room = rooms[7];

        rooms[5].doors.Add(grid[4, 17]);
        grid[4, 17].room = rooms[5];

        rooms[8].doors.Add(grid[7, 19]);
        rooms[8].doors.Add(grid[9, 16]);
        rooms[8].doors.Add(grid[14, 16]);
        rooms[8].doors.Add(grid[16, 19]);
        grid[7, 19].room = rooms[8];
        grid[9, 16].room = rooms[8];
        grid[14, 16].room = rooms[8];
        grid[16, 19].room = rooms[8];

        rooms[6].doors.Add(grid[18, 18]);
        grid[18, 18].room = rooms[6];
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

    public Room FindRoom(Node n)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsInRoom(n))
            {
                return rooms[i];
            }

        }
        return null;
    }
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
        /*float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];*/

        int x = Mathf.FloorToInt((-transform.position.x + worldPosition.x) / nodeDiameter);
        int y = Mathf.FloorToInt((-transform.position.z + worldPosition.z) / nodeDiameter);
        return grid[x, y];
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
            GameObject clone;
            if (n.drawRoom)
            {
                n.drawRoom = false;
                n.room.draw = false;
                clone = GameObject.Instantiate(
                roomTarget,
                new Vector3(n.room.area.position.x, 0.2f, n.room.area.position.z),
                new Quaternion());
                clone.transform.localScale = n.room.area.localScale;
                clone.GetComponent<MoveInRoom>().room = n.room;
                clone.tag = "target";
            }
            else
            {
                clone = GameObject.Instantiate(
                    target,
                    n.worldPosition,
                    new Quaternion());
                clone.transform.localScale = new Vector3(nodeDiameter - .8f, 0.2f, nodeDiameter - .8f);
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