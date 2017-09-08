using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Carte : NetworkBehaviour {

	int numPlayers = 0;
	bool oneStamp = true;
	public static short msgNum = MsgType.Highest + 11;
	NetworkConnection[] players;

	// Use this for initialization
	void Start () {
		if(NetworkServer.active){
			numPlayers = NetworkServer.connections.Count;
			players = new NetworkConnection[NetworkServer.connections.Count];
			NetworkServer.connections.CopyTo(players, 0);
		}
	}
	// Update is called once per frame
	void Update () {
		if(oneStamp && NetworkServer.active){
			TestDealerCards ();
			oneStamp = false;
		}
	}

	void TestDealerCards(){
		//mazzo di carte riempito di 21 carte (6 pers, 6 armi, 9 stanze)
		string[] cards = {"Dolphin Rouge","Emma Stacy","Vincent Count","Mark Johnson","Freddie Carneval","Anne Marie",
			"Coltello","Tubo di piombo","Corda","Pistola","Candeliere","Chiave inglese",
			"Cucina","Salotto","Studio","Ingresso","Biblioteca","Sala da biliardo","Sala da ballo",
			"Serra","Sala da pranzo"};

		//scelta random carte della soluzione
		string[] hiddenCards = new string[3];
		hiddenCards [0] = cards [Random.Range (0, 5)];
		hiddenCards [1] = cards [Random.Range (6, 11)];
		hiddenCards [2] = cards [Random.Range (12, 20)];

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
			int r = Random.Range (0, z);
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
		Debug.Log (strMsg.value+"");
	}
}
