using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistribuzioneCarte : MonoBehaviour {
	public GameObject card;
	public GameObject[] cards;
	bool a = true;
	int numPlayers = 4;
	int dirCardDecider ;
	// Use this for initialization
	void Start () {
		int i = 20;
		float y = 48;
		for (i = 20; i < 38; i++) {
			Instantiate (card, new Vector3 (1.8f, y, 0), Quaternion.Euler(0,0,180));
			y = y + 0.2f;
		}

		cards = GameObject.FindGameObjectsWithTag ("card");
		dirCardDecider = numPlayers;
	}
	
	// Update is called once per frame
	void Update () {
		if (a) {
			Debug.Log ("NumCarte " + cards.Length);
			a = false;
		}
		foreach (GameObject c in cards) {
			if (numPlayers == 3) {
				
			} else if (numPlayers == 4) {

			} else if (numPlayers == 5) {

			} else if (numPlayers == 6) {

			} else {
				Debug.Log ("Errore in numPlayers");
			}
		}
	}
}
