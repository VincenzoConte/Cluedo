using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {

    [SyncVar]
    public Color color;
    [SyncVar]
    public string character;			//nome Player
	public string [] carteInMano;
    public Sprite playerImage;
    public GameObject playerCamera;
    public byte turniDaSaltare;
	static int idPlayerQuestioned;
    GameObject aStar = null;
    // Use this for initialization
    void Start () { 
        GetComponent<Renderer>().material.color = color;
        turniDaSaltare = 0;
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
            case "Dolphin Rogue":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Dolphin");
                break;
            case "Anne Marie":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Anne");
                break;
            case "Freddie Carnival":
                playerImage = Resources.Load<Sprite>("Immagini/PG/Freddie");
                break;
            default:
                playerImage = null;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (aStar == null)
            aStar = GameObject.Find("A*");
	}

    public override void OnStartLocalPlayer() {
        GameObject.Find("A*").GetComponent<Pathfinding>().seeker = gameObject.transform;
        GameObject cam = Instantiate(playerCamera);
        cam.transform.SetParent(gameObject.transform);
        cam.transform.localPosition = new Vector3(0, 30, 0);
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
        GameObject gm = GameObject.Find("GameManager");
        int turno = gm.GetComponent<Communication>().turno;
        NetworkConnection[] players = gm.GetComponent<Connections>().connections;
        idPlayerQuestioned = (turno + 1)%players.Length;
        while (idPlayerQuestioned!=turno && players[idPlayerQuestioned] == null)
        {
            string card = GameObject.Find("CardsDealer").GetComponent<DistribuzioneCarte>().GetCard(ipotesi, idPlayerQuestioned);
            RpcNotificaCarteIp("Giocatore " + (idPlayerQuestioned + 1), null, card);
            if (card != null)
                return;
            idPlayerQuestioned = (idPlayerQuestioned + 1) % players.Length;
        }
        if(idPlayerQuestioned != turno)
		    TargetChiediCarta (players[idPlayerQuestioned],0,ipotesi);
	}

    [Command]
    public void CmdAccusa(string[] accusa)
    {
        string[] soluzione = GameObject.Find("CardsDealer").GetComponent<DistribuzioneCarte>().GetSolution();
		Debug.Log ("Soluzione: " + soluzione [0] + " " + soluzione [1] + " " + soluzione [2]);
		Debug.Log ("Accusa: " + accusa [0] + " " + accusa [1] + " " + accusa [2]);
        if (accusa[0].Equals(soluzione[0]) && accusa[1].Equals(soluzione[1]) && accusa[2].Equals(soluzione[2]))
        {
            RpcEsitoAccusa(accusa, true);
            //fine partita
        }
        else
            RpcEsitoAccusa(accusa, false);
    }

	[Command]
	public void CmdMostraCarta(string nomePlayer, string playerImage, string carta, string[] ipotesi){
		if(carta == null || carta == ""){
            GameObject gm = GameObject.Find("GameManager");
            NetworkConnection[] players = gm.GetComponent<Connections>().connections;
            idPlayerQuestioned = (idPlayerQuestioned + 1)%players.Length;
            int turno = gm.GetComponent<Communication>().turno;
            if (idPlayerQuestioned == (turno))
            {
                //po verimm
            }
            else
            {
                while (idPlayerQuestioned != turno && players[idPlayerQuestioned] == null)
                {
                    string card = GameObject.Find("CardsDealer").GetComponent<DistribuzioneCarte>().GetCard(ipotesi, idPlayerQuestioned);
                    RpcNotificaCarteIp("Giocatore " + (idPlayerQuestioned + 1), null, card);
                    if (card != null)
                        return;
                    idPlayerQuestioned = (idPlayerQuestioned + 1) % players.Length;
                }
                if (idPlayerQuestioned != turno)
                    TargetChiediCarta(players[idPlayerQuestioned], 0, ipotesi);
            }
		}else
		{
			//Debug.Log (" Il giocatore: " + nomePlayer + " mi ha mostrato la carta: " + carta);
			string messaggio = " Il giocatore: " + nomePlayer + " mi ha mostrato la carta: " + carta;
			GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().ShowMessaggiPanel ();
			GameObject.Find ("PanelMessaggi").GetComponent<MessageDealer> ().setMessageOnScreen (playerImage, messaggio);
			GameObject.Find ("PanelMessaggi").GetComponent<MessageDealer> ().decrementaCounter ();
		}
		RpcNotificaCarteIp (nomePlayer, playerImage, carta);
	}

    [Command]
    public void CmdLanciaDadi(GameObject dado1, GameObject dado2) {
        dado1.GetComponent<dice>().RollTheDice();
        dado2.GetComponent<dice>().RollTheDice();
    }

    [Command]
    public void CmdAzzeraDadi(GameObject dado1, GameObject dado2)
    {
        dado1.GetComponent<dice>().value = 0;
        dado2.GetComponent<dice>().value = 0;
    }

	[ClientRpc]
	public void RpcIpotesi(string[] ipotesi) {
        OperativaInterfaccia oi = GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>();
        oi.messaggioUI.text = "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in "+ ipotesi[2];
		oi.ShowMessaggiPanel ();
		GameObject.Find ("PanelMessaggi").GetComponent<MessageDealer> ().resetMessagePanel ();
        Pathfinding pf = aStar.GetComponent<Pathfinding>();
        GamePlayer myPlayer = pf.seeker.GetComponent<GamePlayer>();
        if (!oi.IsMyTurn() && myPlayer.character.Equals(ipotesi[0]))
        {
            Room room = pf.grid.FindRoomByName(ipotesi[2]);
            if (room != null)
            {
                pf.MoveAfterHypothesis(room);
                oi.movedAfterHypothesis = true;
            }
            else
                Debug.Log("errore: stanza non trovata");
        }
	}

    [ClientRpc]
    public void RpcEsitoAccusa(string[] accusa, bool esito)
    {
        OperativaInterfaccia oi = GameObject.Find("GameManager").GetComponent<OperativaInterfaccia>();
        oi.messaggioUI.text = "Accuso " + accusa [0] + " con " + accusa [1] + " in " + accusa [2];
        if (esito)
        {
			if (oi.IsMyTurn ())
				oi.messaggioUI.text ="hai vinto";
            else
				oi.messaggioUI.text += ": <b>"+character+" ha vinto!</b>";
            //fine partita
			GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().fineGiocoPanel.SetActive (true);
        }
        else
        {
            // messaggio accusa sbagliata
			if (oi.IsMyTurn ())
			{
				oi.messaggioUI.text ="Accusa errata, perdi due turni! ";
				turniDaSaltare = 2;
				//GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().fineTurno ();
			}else{
				oi.messaggioUI.text += ": "+"<b>Sbagliato!</b>";
			}
        }
    }

    [ClientRpc]
    public void RpcAggiornaInterfaccia(GameObject player) {
		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().AggiornaInterfaccia(player);
    }

	[TargetRpc]
	public void TargetChiediCarta(NetworkConnection target, int extra, string[] ipotesi) {
        GamePlayer localPlayer =aStar.GetComponent<Pathfinding>().seeker.GetComponent<GamePlayer>();

		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().mostraCartaPanel.SetActive (true);
		MostraCartaScript mc = GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().mostraCartaPanel.GetComponent<MostraCartaScript> ();
		mc.ShowCardsOnPanel (ipotesi, localPlayer.carteInMano);
       // localPlayer.CmdMostraCarta(localPlayer.character, null, ipotesi);
	}

	[ClientRpc]
	public void RpcNotificaCarteIp(string nomePlayer, string playerImage, string carta){
		string messaggio;
		if (carta == null || carta == "") 
		{
			//Debug.Log (nomePlayer + " non ha carte da mostrare!");
			messaggio = nomePlayer + " non ha carte da mostrare!";
		}
		else
		{
			if (GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().IsMyTurn ())
			{
				messaggio = nomePlayer + " ha mostrato: <b>" + carta + "</b>";
			} else
				{
					//Debug.Log (nomePlayer + " ha mostrato una carta");
					messaggio = nomePlayer + " ha mostrato una carta";
				}	
		}
		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().ShowMessaggiPanel();
		GameObject.Find ("PanelMessaggi").GetComponent<MessageDealer> ().setMessageOnScreen(playerImage, messaggio);
	}
}
