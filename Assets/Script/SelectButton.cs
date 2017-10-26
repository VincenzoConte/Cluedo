using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour {

	public string nameButton;

	public void Clicked()
	{
		GameObject.Find("MostraCartaPanel").GetComponent<MostraCartaScript> ().ScegliCarta (nameButton);
	}
}
