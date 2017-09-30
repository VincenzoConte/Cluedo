using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {

    [SyncVar]
    public Color color;
    [SyncVar]
    public string character;
    public Sprite playerImage;
    public GameObject playerCamera;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.color = color;
        switch (character)
        {
            case "Vincent Count":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Vincent");
                break;
            case "Mark Johnson":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Mark");
                break;
            case "Emma Stacy":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Emma");
                break;
            case "Dolphin Rouge":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Dolphin");
                break;
            case "Anne Marie":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Anne");
                break;
            case "Freddie Carneval":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Freddie");
                break;
            default:
                playerImage = null;
                break;
        }
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

	[Command]
	public void CmdIpotesi(string[] i){
		RpcIpotesi (i);
	}

	[ClientRpc]
	public void RpcIpotesi(string[] i){
		Debug.Log (i[0]+i[1]+i[2]+"");
	}

    [ClientRpc]
    public void RpcAggiornaInterfaccia(GameObject player)
    {
        GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>().AggiornaInterfaccia(player);
    }
}
