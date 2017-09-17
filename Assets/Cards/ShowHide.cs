using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowHide : MonoBehaviour {
	Vector3 initPos;
	bool positionChosed = false;
	public bool isHidden = false;
	Component[] components;
	Component texto;
	GameObject[] cards;
	// Use this for initialization
	void Start () {
		cards = GameObject.FindGameObjectsWithTag ("card");
		initPos = transform.position;
		components = GetComponentsInChildren<Component> ();
		foreach (Component c in components) {
			if (c.name == "New Text") {
				texto = c;
			}
		}
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
			transform.DOMove (new Vector3 (1.8f, 45, -15), 0.6f, false);
			isHidden = true;
			texto.GetComponent<MeshRenderer> ().enabled = false;
			Debug.Log ("Nascondi");
		}else{
			transform.DOMove (initPos, 0.6f, false);
			isHidden = false;
			texto.GetComponent<MeshRenderer> ().enabled = true;
			Debug.Log ("Mostra");
		}
	}
}
