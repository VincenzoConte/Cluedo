using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DistribuzioneCarte : MonoBehaviour {
	public GameObject card;
	GameObject[] cards;
	bool a = true;
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
	}
	
	// Update is called once per frame
	void Update () {
		if (a) {
			Debug.Log ("NumCarte " + cards.Length);
			DealCards ();
			a = false;
		}
	}

	void DealCards(){
		int j = 0;
		int dealTime = 5;
		for(j=0;j<cards.Length;j++) {
			if (numPlayers == 3) {
				if (dirCardDecider == 0) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false);
				}
				if (dirCardDecider == 1) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 2) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 52), dealTime, false);
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 4) {
				if (dirCardDecider == 0) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false);
				}
				if (dirCardDecider == 1) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 0), dealTime, false);
				}
				if (dirCardDecider == 2) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 3) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 0), dealTime, false);
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;

			} else if (numPlayers == 5) {
				if (dirCardDecider == 0) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false);
				}
				if (dirCardDecider == 1) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 0), dealTime, false);
				}
				if (dirCardDecider == 2) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 3) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 4) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 0), dealTime, false);
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else if (numPlayers == 6) {
				if (dirCardDecider == 0) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, -52), dealTime, false);
				}
				if (dirCardDecider == 1) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, -52), dealTime, false);
				}
				if (dirCardDecider == 2) {
					cards[j].transform.DOMove (new Vector3 (-47, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 3) {
					cards[j].transform.DOMove (new Vector3 (1.8f, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 4) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, 52), dealTime, false);
				}
				if (dirCardDecider == 5) {
					cards[j].transform.DOMove (new Vector3 (50, cards[j].transform.position.y, -52), dealTime, false);
				}

				dirCardDecider = (dirCardDecider + 1) % numPlayers;


			} else {
				Debug.Log ("Errore in numPlayers");
			}
		}
	}
}
