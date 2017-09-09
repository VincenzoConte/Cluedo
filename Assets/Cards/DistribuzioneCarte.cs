using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;

public class DistribuzioneCarte : NetworkBehaviour {
	public GameObject card;
	GameObject[] cards;
	bool a = true;

	NetworkConnection[] players;
	public static short msgNum = MsgType.Highest + 11;
	static bool cardArrived = false;
	bool oneStamp = true;

	[SyncVar]
		public int numPlayers = 4;

	int dirCardDecider = 0;
	// Use this for initialization
	void Start () {
		int i = 20;
		float y = 43;
		for (i = 20; i < 38; i++) {
			Instantiate (card, new Vector3 (1.8f, y, 0), Quaternion.Euler(0,0,180));
			y = y + 0.2f;
		}

		cards = GameObject.FindGameObjectsWithTag ("card");
		if (NetworkServer.active) {
			numPlayers = NetworkServer.connections.Count;
			players = new NetworkConnection[NetworkServer.connections.Count];
			NetworkServer.connections.CopyTo(players, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (a) {
			Debug.Log ("NumCarte " + cards.Length);
			DealCards ();
			a = false;
		}
		if(oneStamp && NetworkServer.active){
			TestDealerCards ();
			oneStamp = false;
		}

		if(cardArrived){
			Instantiate (card, new Vector3 (1.8f, 45, 0), Quaternion.Euler (0, 180, 0));
			cardArrived = false;
			Debug.Log ("creata carta!");
		}
	}

	void DealCards(){
		int j = 0;
		float dealTime = 0.8f;
		Sequence seq = DOTween.Sequence ();
        seq.OnComplete(InizioPartita);
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

	void TestDealerCards(){
		//mazzo di carte riempito di 21 carte (6 pers, 6 armi, 9 stanze)
		string[] cards = {"Dolphin Rouge","Emma Stacy","Vincent Count","Mark Johnson","Freddie Carneval","Anne Marie",
			"Coltello","Tubo di piombo","Corda","Pistola","Candeliere","Chiave inglese",
			"Cucina","Salotto","Studio","Ingresso","Biblioteca","Sala da biliardo","Sala da ballo",
			"Serra","Sala da pranzo"};

		//scelta random carte della soluzione
		string[] hiddenCards = new string[3];
		hiddenCards [0] = cards [UnityEngine.Random.Range (0, 5)];
		hiddenCards [1] = cards [UnityEngine.Random.Range (6, 11)];
		hiddenCards [2] = cards [UnityEngine.Random.Range (12, 20)];

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

		/*Debug.Log ("Le carte sono: "+cards.Length+"");
		Debug.Log ("Le carte nascoste sono: ");
		for(int j=0;j<hiddenCards.Length;j++){
			Debug.Log (hiddenCards[j]+"");
		}
		Debug.Log ("Le carte da distribuire sono: ");
		for(int y=0;y<cardsToDeal.Length;y++){
			Debug.Log (cardsToDeal[y]+"");
		}*/

		//carte riordinate randomicamente (mischiate) da distribuire ai gioctori
		string[] randomlyDealtCards = new string[18];
		int w = 0;
		for(int z=cardsToDeal.Length-1;z>=0;z--){
			int r = UnityEngine.Random.Range (0, z);
			randomlyDealtCards [w] = cardsToDeal [r];
			w++;
			cardsToDeal [r] = cardsToDeal [z];
		}

		Debug.Log ("Le carte distribuite radomicamente sono: ");
		for(int x=0;x<randomlyDealtCards.Length;x++){
			//Debug.Log (randomlyDealtCards[x]+"");
			players [x%numPlayers].Send (msgNum, new StringMessage (randomlyDealtCards [x]));
		}
	}

	public static void MsgInvioCarteHandler(NetworkMessage netMsg){
		StringMessage strMsg = netMsg.ReadMessage<StringMessage> ();
		cardArrived = true;
		Debug.Log (strMsg.value+" c: "+cardArrived.ToString ());
	}
}
