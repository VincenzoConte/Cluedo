using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectIpotesiCard : MonoBehaviour {

	public string categoria, nomeCarta;
	public GameObject go;
	public Button button;


	public void selectCard()
	{
		
		go.GetComponent<IpotesiCarte> ().selectedCard (categoria, nomeCarta, button );
	}

}
