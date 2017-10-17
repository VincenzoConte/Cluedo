using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowHide : MonoBehaviour {
	Vector3 initPos;
	bool positionChosed = false;
	public bool isHidden = false;
	Component texto;
	GameObject[] cards;
	// Use this for initialization
	void Start () {
		cards = GameObject.FindGameObjectsWithTag ("card");
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!positionChosed && transform.position.y == 45){
			initPos = transform.position;
			positionChosed = true;
		}
	}
	void OnMouseDown(){
		foreach (GameObject c in cards) {
			c.GetComponent<ShowHide> ().MoveFunction ();
		}
	}

	public void MoveFunction(){
		if(!isHidden){
			transform.DOMove (new Vector3 (1.8f, 80, -40), 0.6f, false);
			isHidden = true;
		}else{
			transform.DOMove (initPos, 0.6f, false);
			isHidden = false;
			Debug.Log ("Mostra");
		}
	}
}
