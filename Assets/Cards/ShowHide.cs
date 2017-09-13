using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowHide : MonoBehaviour {
	Vector3 initPos;
	bool positionChosed = false;
	public bool isHidden = false;
	Component[] components;
	Component text;
	GameObject[] cards;
	// Use this for initialization
	void Start () {
		cards = GameObject.FindGameObjectsWithTag ("card");
		initPos = transform.position;
		components = GetComponentsInChildren<Component> ();
		foreach (Component c in components) {
			if (c.name == "New Text") {
				text = c;
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
			transform.DOMove (new Vector3 (1.8f, 45, -15), 0.7f, false);
			isHidden = true;
			text.GetComponent<MeshRenderer> ().enabled = false;
			Debug.Log ("Nascondi");
		}else{
			transform.DOMove (initPos, 0.7f, false);
			isHidden = false;
			text.GetComponent<MeshRenderer> ().enabled = true;
			Debug.Log ("Mostra");
		}
	}
}
