using System.Collections.Generic;
using UnityEngine;

public class Room {

    public string name;
    public Transform area;
    public List<Node> doors;
    public bool draw = false;
	public GameObject botola;

    public Room(string name, Transform area)
    {
        this.area = area;
        this.name = name;
        doors = new List<Node>();
		botola = null;
    }

    public string getNomeStanza()
    {
		return this.name;
    }

    public static bool CheckNode(Node n)
    {
        return Physics.CheckSphere(n.worldPosition, 0.1f, LayerMask.GetMask("room"));
    }

    public bool IsInRoom(Node n)
    {
        if (!Room.CheckNode(n))
            return false;
        float x = area.localScale.x / 2;
        float z = area.localScale.z / 2;
        if (n.worldPosition.x > area.position.x - x && n.worldPosition.x < area.position.x + x)
            if (n.worldPosition.z > area.position.z - z && n.worldPosition.z < area.position.z + z)
                return true;
        return false;
    }

    public Node GetWalkableNode()
    {
        Grid grid = GameObject.Find("A*").GetComponent<Grid>();
        Node n = grid.NodeFromWorldPoint(area.position);
        if (n.walkable)
            return n;
        List<Node> list = new List<Node>();
        list.Add(n);
        int i = 0;
        while (i < 600)
        {
            foreach(Node neigh in grid.GetNeighbours(n))
            {
                if (IsInRoom(neigh))
                {
                    if (neigh.walkable)
                        return neigh;
                    else if (!list.Contains(neigh))
                        list.Add(neigh);
                }
            }
            list.Remove(n);
            if (list.Count == 0)
                break;
            n = list[0];
            i++;
        }
        return null;

    }
}
