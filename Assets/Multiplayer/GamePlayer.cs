using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {

    [SyncVar]
    public Color color;
    [SyncVar]
    public string character;
    public Sprite image;

    public GameObject playerCamera;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStartLocalPlayer()
    {
        GameObject.Find("A*").GetComponent<Pathfinding>().seeker = gameObject.transform;
        GameObject cam = Instantiate(playerCamera);
        cam.transform.SetParent(gameObject.transform);
        cam.transform.localPosition = new Vector3(0, 10, 0);
        GameObject.Find("Gestione camera").GetComponent<SwitchCamera>().SetPlayerCamera(cam);
    }

    [Command]
    public void CmdFineTurno(GameObject manager)
    {
        manager.GetComponent<Communication>().CambioTurno();
    }

    [Command]
    public void CmdGiocatoreAttivo(GameObject player)
    {
        RpcAggiornaInterfaccia(player);
    }

    [ClientRpc]
    public void RpcAggiornaInterfaccia(GameObject player)
    {
        GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>().AggiornaInterfaccia(player);
    }
}
