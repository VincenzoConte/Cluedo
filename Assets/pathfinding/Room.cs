using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public string name;
    public Transform area;
    public static float radius=0;

    public Room(string name, Transform area)
    {
        this.area = area;
        this.name = name;
    }

    public static bool CheckNode(Node n)
    {
        if (Physics.CheckSphere(n.worldPosition, radius - 0.4f, LayerMask.GetMask("room")))
            return true;
        return false;
    }

    public Node GetWalkableNode()
    {
        Grid grid = GameObject.Find("A*").GetComponent<Grid>();
        for(float x=area.position.x-area.localScale.x/2; x<= area.position.x + area.localScale.x / 2; x = x + radius * 2)
        {
            for (float z = area.position.z - area.localScale.z / 2; z <= area.position.z + area.localScale.z / 2; z = z + radius * 2)
            {
                Node n = grid.NodeFromWorldPoint(new Vector3(x, 0, z));
                if (n.walkable)
                    return n;
            }
        }
        return null;

    }
}
