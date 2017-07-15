using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnClick : MonoBehaviour {

    Pathfinding aStar;
	// Use this for initialization
	void Start () {
        aStar = GameObject.Find("A*").GetComponent<Pathfinding>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    /*void OnMouseOver()
    {
        Debug.Log("over");
        GetComponent<Renderer>().material.color=new Color(0,1,0,0);
    }*/

    void OnMouseDown()
    {
        aStar.FindPath(transform.position);
    }
}
