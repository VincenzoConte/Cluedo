using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostraCartaScript : MonoBehaviour {

	private string[] carteInMano, ipotesiFatta;
	public Button buttonPrefab, NonHoCarte;
	public GameObject panel;
	private float margin = 0;
	private Button myButton;
//	private bool clicked;

	// Use this for initialization
	void OnEnable ()
	{
		//clicked = false;
		margin = 0;
		NonHoCarte.gameObject.SetActive (false);
	}

	public void ShowCardsOnPanel(string[] ipotesi, string[] carteInMano )
	{ 
		int counter = 0;			//conta quante carte ho in mano
		ipotesiFatta = ipotesi;
		for (int i = 0; i < ipotesi.Length; i++) 
		{
			for (int j = 0; j < carteInMano.Length -1; j++) 
			{
				Debug.Log ("Confronto " + ipotesi [i] + " con " + carteInMano [j] + " ");
				if (ipotesi [i].Replace (" ", "").Equals(carteInMano [j].Replace (" ", ""))) 
				{
					Debug.Log ("ho instanziato la carta" + ipotesi [i] + " coinvolta nell'ipotesi");
					myButton = Instantiate (buttonPrefab, new Vector3 (200.2f + margin, 350.5f, 0), Quaternion.Euler (0, 0, 0)) as Button;
					myButton.image.sprite = Resources.Load<Sprite> ("Immagini/Carte/" + ipotesi [i].Replace (" ", ""));
					margin += 290f;
					myButton.name = ipotesi [i];
					myButton.onClick.AddListener (ScegliCarta);	
					myButton.transform.SetParent (panel.transform);
					counter++;
					break;
				}
			}
		}
		if (counter == 0)
		{
			myButton = null;
			NonHoCarte.gameObject.SetActive (true);
		}

		StartCoroutine (clickAfterTimer ());
	}
	public void ScegliCarta()
	{
		Debug.Log(myButton.name);
		GamePlayer localPlayer = GameObject.Find("A*").GetComponent<Pathfinding>().seeker.GetComponent<GamePlayer>();
		localPlayer.CmdMostraCarta(localPlayer.character, myButton.name, ipotesiFatta);
		this.gameObject.SetActive (false);
		StopCoroutine (clickAfterTimer ());
	}

	public void Skip()
	{
		GamePlayer localPlayer = GameObject.Find("A*").GetComponent<Pathfinding>().seeker.GetComponent<GamePlayer>();
		localPlayer.CmdMostraCarta(localPlayer.character, null, ipotesiFatta);
		this.gameObject.SetActive (false);
		StopCoroutine (clickAfterTimer ());
	}

	IEnumerator clickAfterTimer(){
		yield return new WaitForSeconds (20);
		if (myButton == null)
			NonHoCarte.onClick.Invoke ();
		else
			myButton.onClick.Invoke ();
	}
}
