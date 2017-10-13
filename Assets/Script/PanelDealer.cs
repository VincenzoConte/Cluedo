using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelDealer : MonoBehaviour {
	public GameObject mainPanelAccusa,panelSospettoAccusa, panelArmaAccusa, panelStanzaAccusa;
	public Button indietro;

	public void showPanelSospetto()
	{
		panelSospettoAccusa.SetActive (true);
		mainPanelAccusa.SetActive (false);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goOneBack ();}); 
	}

	public void hidePanelSospetto()
	{
		panelSospettoAccusa.SetActive (false);
		mainPanelAccusa.SetActive (true);
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goBack ();}); 

	}

	public void showPanelArma()
	{
		panelArmaAccusa.SetActive (true);
		mainPanelAccusa.SetActive (false);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goOneBack ();}); 
	}

	public void hidePanelArma()
	{
		panelArmaAccusa.SetActive (false);
		mainPanelAccusa.SetActive (true);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goBack ();}); 
	}

	public void showStanzaPanel()
	{
		panelStanzaAccusa.SetActive (true);
		mainPanelAccusa.SetActive (false);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goOneBack ();}); 
	}

	public void hideStanzaPanel()
	{
		panelStanzaAccusa.SetActive (false);
		mainPanelAccusa.SetActive (true);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goBack ();}); 
	}

	public void showMainPanel()
	{
		mainPanelAccusa.SetActive (true);
		panelSospettoAccusa.SetActive (false);
		panelArmaAccusa.SetActive (false);
		panelStanzaAccusa.SetActive (false);
		indietro.onClick.RemoveAllListeners ();
		indietro.onClick.AddListener (delegate { this.GetComponent<AccusaScript> ().goBack ();}); 
	}

	public void hideMainPanel()
	{
		mainPanelAccusa.SetActive (false);
	}
}
