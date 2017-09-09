using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {

    [SyncVar]
    public Color color;
    private GameObject manager;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*public override void OnStartLocalPlayer()
    {
        Debug.Log("localplayer");
        manager = GameObject.Find("GameManger");
        GameObject.Find("A*").GetComponent<Pathfinding>().seeker = gameObject.transform;
    }*/

    [Command]
    public void CmdFineTurno()
    {
        manager.GetComponent<Communication>().CambioTurno();
    }
}
