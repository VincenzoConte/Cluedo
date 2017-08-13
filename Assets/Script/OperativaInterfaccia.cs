using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperativaInterfaccia : MonoBehaviour {

public GameObject dadi;
public Button accusa;
public Button botola;
public Button ipotesi;
public dice dado1;
private dice dado2;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//E' il mio turno

	public void LanciaDadi(){

	GameObject go = GameObject.Find ("dado");
	dado1 = (dice)go.GetComponent ((typeof(dice)));

	go = GameObject.Find ("dado 2");
    dado2 = (dice)go.GetComponent ((typeof(dice)));

	dado1.RollTheDice ();
	dado2.RollTheDice ();
	dadi.gameObject.SetActive (false);
	}


}
