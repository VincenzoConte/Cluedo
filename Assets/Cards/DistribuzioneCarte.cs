using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using UnityEngine.UI;

public class DistribuzioneCarte : NetworkBehaviour {
	Sequence seq;
	public GameObject card;
	GameObject[] cards;
	bool oneTimeDeal = true;

	public GameObject realCard;
	NetworkConnection[] players;
	public static short msgNum = MsgType.Highest + 11;
	static ArrayList receivedCards = new ArrayList();
	bool oneStamp = true;
	bool createdCards = false;

	[SyncVar]
		public int numPlayers = 4;

	int dirCardDecider = 0;
	// Use this for initialization
	void Start () {
		seq = DOTween.Sequence ();
		int i = 20;
		float y = 43;
		for (i = 20; i < 38; i++) {
			Instantiate (card, new Vector3 (1.8f, y, 0), Quaternion.Euler(0,0,180));
			y = y + 0.2f;
		}

		cards = GameObject.FindGameObjectsWithTag ("falseCard");
		if (NetworkServer.active) {
			numPlayers = NetworkServer.connections.Count;
			players = new NetworkConnection[NetworkServer.connections.Count];
			NetworkServer.connections.CopyTo(players, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (oneTimeDeal) {
			Debug.Log ("NumCarte " + cards.Length);
			FalseDealCards ();
			oneTimeDeal = false;
		}

		if(oneStamp && NetworkServer.active){
			seq.OnComplete(DealerCards);
			oneStamp = false;
		}

		if(receivedCards.Contains ("end") && !createdCards){
            seq = DOTween.Sequence();
            seq.OnComplete(InizioPartita);
			float startX = 0;
			if (receivedCards.Count - 1 == 3)
				startX = 1.8f - 7.5f;
			else if (receivedCards.Count - 1 == 4)
				startX = 1.8f - 11.25f;
			else if (receivedCards.Count - 1 == 5)
				startX = 1.8f - 15.0f;
			else if(receivedCards.Count - 1 == 6)
				startX = 1.8f - 18.75f;
			else
				Debug.Log ("Errore numero carte ricevute!");

			GameObject[] instantiatedCards = new GameObject[receivedCards.Count - 1];
			for (int i = 0; i < receivedCards.Count - 1; i++) {
				GameObject instCard;
				Vector3 startPos = new Vector3(startX + (7.5f*i),40,-23);
				instCard = Instantiate (realCard, startPos,Quaternion.Euler (0, 180, 0));
				Component[] components = instCard.GetComponentsInChildren<Component> ();
				foreach (Component g in components) {
					if (g.name == "New Text") {
						g.GetComponent<TextMesh> ().text = receivedCards [i].ToString ();
					}
				}
				Button[] button = GameObject.Find (receivedCards[i].ToString ()).GetComponentsInChildren<Button>();
				ButtonInventary bi = button[0].GetComponent<ButtonInventary>();
//				Debug.Log ("CARTA RICEVUTA :" + receivedCards [i].ToString () +" Button ID : " + bi.id + " Indice : " + i);
				bi.lockSelection(bi.id);
				instantiatedCards [i] = instCard;
			}
			for(int ii = 0; ii<instantiatedCards.Length ; ii++){
				seq.Append (instantiatedCards[ii].transform.DOMove (new Vector3(instantiatedCards[ii].transform.position.x,45,0),0.7f,false));
			}
			createdCards = true;
		}
	}

	//metodo per la distribuzione fittizia delle carte in base al numero dei giocatori
	void FalseDealCards(){
		//seq.OnComplete(InizioPartita);
		int j = 0;
		float dealTime = 0.8f;
		for(j=0;j<cards.Length;j++) {
			if (numPlayers == 3) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 52), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 52), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 4) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 0), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 52), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 0), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 5) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 0), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 52), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 52), dealTime, false));
				}
				if (dirCardDecider == 4) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 0), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else if (numPlayers == 6) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, -30), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 31), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 52), dealTime, false));
				}
				if (dirCardDecider == 4) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 31), dealTime, false));
				}
				if (dirCardDecider == 5) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, -30), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else {
				Debug.Log ("Errore in numPlayers");
			}
		}
	}

    private void InizioPartita()
    {
        GameObject.Find("GameManager").GetComponent<Communication>().CambioTurno();
    }

	//metodo effettivo per la distribuzione delle carte da parte dell'host verso i client tramite Messaggi
	void DealerCards(){
		//mazzo di carte riempito di 21 carte (6 pers, 6 armi, 9 stanze)
		string[] cards = {"Dolphin Rouge","Vincent Count","Mark Johnson","Freddie Carneval","Anne Marie","Emma Stacy",
			"Corda","Pistola","Chiave inglese","Pugnale","Candeliere","Tubo di piombo",
			"Ingresso","Salotto","Sala da pranzo","Cucina","Sala da ballo","Serra","Sala da biliardo",
			"Biblioteca","Studio"};

		//scelta random carte della soluzione
		string[] hiddenCards = new string[3];
		hiddenCards [0] = cards [UnityEngine.Random.Range (0, 5)];
		hiddenCards [1] = cards [UnityEngine.Random.Range (6, 11)];
		hiddenCards [2] = cards [UnityEngine.Random.Range (12, 20)];

		Debug.Log ("Le carte della soluzione sono: ");
		for(int l=0;l<hiddenCards.Length;l++){
			Debug.Log (l+""+hiddenCards[l]);
		}

		//restanti carte da mischiare
		string[] cardsToDeal = new string[18];
		int h = 0; 
		for(int k=0;k<cardsToDeal.Length;k++){
			cardsToDeal [k] = cards [h];
			if(cards[h]==hiddenCards[0] | cards[h]==hiddenCards[1] | cards[h]==hiddenCards[2]){
				k--;
			}
			h++;
		}

		//carte riordinate randomicamente (mischiate) da distribuire ai gioctori
		string[] randomlyDealtCards = new string[18];
		int w = 0;
		for(int z=cardsToDeal.Length-1;z>=0;z--){
			int r = UnityEngine.Random.Range (0, z);
			randomlyDealtCards [w] = cardsToDeal [r];
			w++;
			cardsToDeal [r] = cardsToDeal [z];
		}

		//Debug.Log ("Le carte distribuite radomicamente sono: ");
		for(int x=0;x<randomlyDealtCards.Length;x++){
			//Debug.Log (randomlyDealtCards[x]+"");
			players [x%numPlayers].Send (msgNum, new StringMessage (randomlyDealtCards [x]));
		}
		for(int s=0;s<players.Length;s++){
			players[s].Send (msgNum,new StringMessage("end"));
		}
	}

	public static void MsgInvioCarteHandler(NetworkMessage netMsg){
		StringMessage strMsg = netMsg.ReadMessage<StringMessage> ();
		Debug.Log (strMsg.value+"");
		receivedCards.Add (strMsg.value);
	}
}
