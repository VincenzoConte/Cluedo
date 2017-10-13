using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInRoom : MonoBehaviour {

    Pathfinding aStar;
    public Room room;
    // Use this for initialization
    void Start () {
        aStar = GameObject.Find("A*").GetComponent<Pathfinding>();
    }
	
	// Update is called once per frame
	void Update () {
   
    }

    void OnMouseDown()
    {
        aStar.seeker.GetComponentInChildren<MoveCamera>().InitialPosition();
		aStar.MoveInRoom(room,false);
    }


}

