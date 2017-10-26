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
	private Pathfinding aStar;

	GameObject[] instantiatedCards;
    string[] hiddenCards;
    string[] randomlyDealtCards;

    public GameObject realCard;
    public Connections conn;
	public static short msgNum = MsgType.Highest + 11;
	static ArrayList receivedCards;
	bool oneStamp = true;
	bool createdCards = false;

	public GameObject[] cardPrefabs;  

	[SyncVar]
	int numPlayers = -1;

	int dirCardDecider = 0;
	// Use this for initialization
	void Start () {
		aStar = GameObject.Find ("A*").GetComponent<Pathfinding> ();
        receivedCards = new ArrayList();
		seq = DOTween.Sequence ();
        seq.Pause();
		int i = 20;
		float y = 43;
		for (i = 20; i < 38; i++) {
			Instantiate (card, new Vector3 (1.8f, y, 0), Quaternion.Euler(0,0,180));
			y = y + 0.2f;
		}
        if (NetworkServer.active)
            numPlayers = NetworkServer.connections.Count;

		cards = GameObject.FindGameObjectsWithTag ("falseCard");
	}
	
	// Update is called once per frame
	void Update () {
		if (oneTimeDeal && (numPlayers != -1) ) {
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
			int mod = receivedCards.Count - 1;
			if (mod == 3)
				startX = -24.5f;
			else if (mod == 4)
				startX = -33f;
			else if (mod == 5)
				startX = -41.5f;
			else if (mod == 6) {
				startX = -24.5f;
				mod = 3;
			}
			else
				Debug.Log ("Errore numero carte ricevute!");

			instantiatedCards = new GameObject[receivedCards.Count - 1];
			for (int i = 0; i < receivedCards.Count - 1; i++) {
				GameObject instCard = null;
				Vector3 startPos = new Vector3(startX + (17f*(i%mod)),80,-50);

				foreach(GameObject go in cardPrefabs){
					if(go.name.Equals (receivedCards[i])){
						instCard = Instantiate (go, startPos,Quaternion.Euler (0, 180, 0));
					}
				}

				Button[] button = GameObject.Find (receivedCards[i].ToString ()).GetComponentsInChildren<Button>();
				ButtonInventary bi = button[0].GetComponent<ButtonInventary>();
				bi.lockSelection(bi.id);
				instantiatedCards [i] = instCard;
			}
			if (receivedCards.Count - 1 == 6) {
				int initZ = 13;
				seq.Append (instantiatedCards [0].transform.DOMove (new Vector3 (instantiatedCards [0].transform.position.x, 80, -initZ), 0.7f, false));
				seq.Append (instantiatedCards [1].transform.DOMove (new Vector3 (instantiatedCards [1].transform.position.x, 80, -initZ), 0.7f, false));
				seq.Append (instantiatedCards [2].transform.DOMove (new Vector3 (instantiatedCards [2].transform.position.x, 80, -initZ), 0.7f, false));
				seq.Append (instantiatedCards [3].transform.DOMove (new Vector3 (instantiatedCards [3].transform.position.x, 80, initZ), 0.7f, false));
				seq.Append (instantiatedCards [4].transform.DOMove (new Vector3 (instantiatedCards [4].transform.position.x, 80, initZ), 0.7f, false));
				seq.Append (instantiatedCards [5].transform.DOMove (new Vector3 (instantiatedCards [5].transform.position.x, 80, initZ), 0.7f, false));
					
			} else {
				for (int ii = 0; ii < instantiatedCards.Length; ii++) {
					seq.Append (instantiatedCards [ii].transform.DOMove (new Vector3 (instantiatedCards [ii].transform.position.x, 80, 0), 0.7f, false));
				}
			}
			aStar.seeker.GetComponent<GamePlayer> ().carteInMano = receivedCards.ToArray(typeof(string)) as string [];
			createdCards = true;
		}
	}

	//metodo per la distribuzione fittizia delle carte in base al numero dei giocatori
	void FalseDealCards(){
		
		int j = 0;
		float dealTime = 0.5f;
        for (j=0;j<cards.Length;j++) {
            if (numPlayers == 3) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -50), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-42, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (45, cards[j].transform.position.y, 50), dealTime, false));
				}
				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 4) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -50), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-90, cards[j].transform.position.y, 0), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (70, cards[j].transform.position.y, 0), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 5) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -50), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-90, cards[j].transform.position.y, 0), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (-42, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (45, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 4) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (70, cards[j].transform.position.y, 0), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else if (numPlayers == 6) {
				if (dirCardDecider == 0) {
					seq.Append (cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -50), dealTime, false));
				}
				if (dirCardDecider == 1) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (-42, cards[j].transform.position.y, -50), dealTime, false));
				}
				if (dirCardDecider == 2) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (-42, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 3) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 4) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (45, cards[j].transform.position.y, 50), dealTime, false));
				}
				if (dirCardDecider == 5) {
					seq.Append (	cards[j].transform.DOMove (new Vector3 (45, cards[j].transform.position.y, -50), dealTime, false));
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else {
				Debug.Log ("Errore in numPlayers");
			}
		}
        seq.Play();
	}

    private void InizioPartita()
    {
        GameObject.Find("GameManager").GetComponent<Communication>().CambioTurno();

		for(int i = 0; i<instantiatedCards.Length ; i++){
			instantiatedCards [i].GetComponent<ShowHide> ().enabled = true;
		}

		GameObject[] cardsToDestroy = GameObject.FindGameObjectsWithTag ("falseCard");
		foreach(GameObject go in cardsToDestroy){
			Destroy (go);
		}
    }

	//metodo effettivo per la distribuzione delle carte da parte dell'host verso i client tramite Messaggi
	void DealerCards(){
		//mazzo di carte riempito di 21 carte (6 pers, 6 armi, 9 stanze)
		string[] cards = {"Dolphin Rogue","Vincent Count","Mark Johnson","Freddie Carnival","Anne Marie","Emma Stacy",
			"Corda","Pistola","Chiave inglese","Pugnale","Candeliere","Tubo di piombo",
			"Ingresso","Salotto","Sala da pranzo","Cucina","Sala da ballo","Serra","Sala del biliardo",
			"Biblioteca","Studio"};

		//scelta random carte della soluzione
		hiddenCards = new string[3];
		hiddenCards [0] = cards [UnityEngine.Random.Range (0, 5)];
		hiddenCards [1] = cards [UnityEngine.Random.Range (6, 11)];
		hiddenCards [2] = cards [UnityEngine.Random.Range (12, 20)];

		/*Debug.Log ("Le carte della soluzione sono: ");
		for(int l=0;l<hiddenCards.Length;l++){
			Debug.Log (l+""+hiddenCards[l]);
		}*/

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
		randomlyDealtCards = new string[18];
		int w = 0;
		for(int z=cardsToDeal.Length-1;z>=0;z--){
			int r = UnityEngine.Random.Range (0, z);
			randomlyDealtCards [w] = cardsToDeal [r];
			w++;
			cardsToDeal [r] = cardsToDeal [z];
		}

        //Debug.Log ("Le carte distribuite radomicamente sono: ");
        NetworkConnection[] players = conn.connections;
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
		//Debug.Log (strMsg.value+"");
		receivedCards.Add (strMsg.value);
	}

    public string[] GetSolution()
    {
        return hiddenCards;
    }

    public string GetCard(string[] cards, int player)
    {
        NetworkConnection[] players = conn.connections;
        if (Array.IndexOf(randomlyDealtCards, cards[0]) % players.Length == player)
            return cards[0];
        else if (Array.IndexOf(randomlyDealtCards, cards[1]) % players.Length == player)
            return cards[1];
        else if (Array.IndexOf(randomlyDealtCards, cards[2]) % players.Length == player)
            return cards[2];
        return null;
    }

}
