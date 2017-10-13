using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAccusaCard : MonoBehaviour {

	public string categoria, nomeCarta;
	public GameObject go;

	public void selectCard()
	{

		go.GetComponent<AccusaScript> ().selectCardAccusa (categoria, nomeCarta);
	}

	public void ShowCorrectPanel()
	{
		PanelDealer pd = go.GetComponent<PanelDealer> ();
		if(categoria.Equals("Sospetto"))
		{
			pd.showPanelSospetto ();
		}else if(categoria.Equals("Arma"))
		{
			pd.showPanelArma ();
		}else if(categoria.Equals("Stanza"))
		{
			pd.showStanzaPanel ();
		}
	}
}
