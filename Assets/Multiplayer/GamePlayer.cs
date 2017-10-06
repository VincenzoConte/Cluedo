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
	static int idPlayerQuestioned;
	NetworkConnection[] players;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.color = color;
		if(NetworkServer.active){
			players = new NetworkConnection[NetworkServer.connections.Count];
			NetworkServer.connections.CopyTo(players, 0);
		}
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

    public override void OnStartLocalPlayer() {
        GameObject.Find("A*").GetComponent<Pathfinding>().seeker = gameObject.transform;
        GameObject cam = Instantiate(playerCamera);
        cam.transform.SetParent(gameObject.transform);
        cam.transform.localPosition = new Vector3(0, 10, 0);
        GameObject.Find("Gestione camera").GetComponent<SwitchCamera>().SetPlayerCamera(cam);
    }

    [Command]
    public void CmdFineTurno(GameObject manager) {
        manager.GetComponent<Communication>().CambioTurno();
    }

    [Command]
    public void CmdGiocatoreAttivo(GameObject player) {
        RpcAggiornaInterfaccia(player);
    }

	[Command]
	public void CmdIpotesi(string[] ipotesi) {
		RpcIpotesi (ipotesi);
		idPlayerQuestioned = (GameObject.Find ("GameManager").GetComponent<Communication> ().turno + 1)%players.Length;
		TargetChiediCarta (players[idPlayerQuestioned],0,ipotesi);
	}

	[Command]
	public void CmdMostraCarta(string nomePlayer, string carta, string[] ipotesi){
		if(carta == null || carta == ""){
			Debug.Log ("la carta è null");
			idPlayerQuestioned = (idPlayerQuestioned + 1)%players.Length;
			if (idPlayerQuestioned == (GameObject.Find ("GameManager").GetComponent<Communication> ().turno)) {
				//po verimm
			} else
				TargetChiediCarta (players [idPlayerQuestioned], 0, ipotesi);
		}
		RpcNotificaCarteIp (nomePlayer,carta);
	}

    [Command]
    public void CmdLanciaDadi(GameObject dado1, GameObject dado2) {
        dado1.GetComponent<dice>().RollTheDice();
        dado2.GetComponent<dice>().RollTheDice();
    }

	[ClientRpc]
	public void RpcIpotesi(string[] ipotesi) {
		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ()
			.messaggioUI.text = "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in "+ ipotesi[2];
	}

    [ClientRpc]
    public void RpcAggiornaInterfaccia(GameObject player) {
		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ()
			.AggiornaInterfaccia(player);
    }

	[TargetRpc]
	public void TargetChiediCarta(NetworkConnection target, int extra, string[] ipotesi) {
        GamePlayer localPlayer = GameObject.Find("A*").GetComponent<Pathfinding>().seeker.GetComponent<GamePlayer>();
        localPlayer.CmdMostraCarta(localPlayer.character, null, ipotesi);
	}

	[ClientRpc]
	public void RpcNotificaCarteIp(string nomePlayer, string carta){
		if (carta == null || carta == "")
			Debug.Log (nomePlayer + " non ha carte da mostrare!");
		else{
			if(GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().IsMyTurn ())
				Debug.Log (nomePlayer + " ha mostrato: "+carta+"");
			else
				Debug.Log (nomePlayer + " ha mostrato una carta");
		}
	}
}
