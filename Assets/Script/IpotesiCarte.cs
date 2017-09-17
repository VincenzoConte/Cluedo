using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class IpotesiCarte : MonoBehaviour {

	public string categoria, nome;
	private static string[] ipotesi;
	public Text txt;
	private ColorBlock cb;
	private static ColorBlock def;
	private Button button;
	private static Button prevSbutton, prevAbutton;
	public GameObject gameManagerr;
	public Button ipotesiEffettivaButton;

	// Use this for initialization
	void Start () {
		ipotesi = new string[3];
		ipotesiEffettivaButton.gameObject.SetActive (false);
		button = this.GetComponent<Button> ();
		prevSbutton = this.GetComponent<Button> ();
		prevAbutton = this.GetComponent<Button> ();
		def = button.colors;
		txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in ---";
	}
	
	public void selectCard()
	{
		if (this.categoria == "Sospetto")
		{
			if (this.nome == ipotesi [0])			//Sto deselezionando la carta Sospetto
			{
				button.colors = def;
				ipotesi [0] = null; 
				if (ipotesi [1]!= null)
				{
					txt.text= "Secondo me è stato (scegli sospetto) con "+ ipotesi[1] + " in ---";
				}
				else
				{
					txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in ---";
				}
			}else								//Sto selezionando la carta Sospetto
			{
				ipotesi [0] = this.nome;
				prevSbutton.colors = def;
				cb = button.colors;
				cb.normalColor = new Color (0.118f, 1f, 0f, 1.0f);
				button.colors = cb;
				prevSbutton.colors = def;
				ipotesi [0] = this.nome;
				if (ipotesi [1]!= null)	
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in ---";
				}
				else
				{
					txt.text = "Secondo me è stato "+ ipotesi[0] +" con (scegli arma) in ---";
				}
				prevSbutton = button;
			}
		}
		else if(this.categoria == "Arma")
		{
			if (this.nome == ipotesi [1])			//Sto deselezionando la carta Arma
			{
				button.colors = def;
				ipotesi [1] = null; 
				if (ipotesi [0]!= null)
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con (scegli arma) in ---";
				}
				else
				{
					txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in ---";
				}
			}else							//Sto selezionando la carta Arma
			{
				prevAbutton.colors = def;
				cb = button.colors;
				cb.normalColor = new Color (0f, 0.545f, 1.0f, 1.0f);
				button.colors = cb;
				prevAbutton.colors = def;
				ipotesi [1] = this.nome;
				if (ipotesi [0]!= null)
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in ---";
				}
				else
				{
					txt.text= "Secondo me è stato (scegli sospetto) con "+ ipotesi[1] + " in ---";
				}
				prevAbutton = button;
			}
		}
		if((ipotesi[0]!=null) && (ipotesi[1]!= null))
		{
			ipotesiEffettivaButton.gameObject.SetActive (true);
		}
		else
		{
			ipotesiEffettivaButton.gameObject.SetActive (false);	
		}
	}

	public void goBack()
	{
		prevAbutton.colors = def;
		prevSbutton.colors = def;
		for(int i = 0; i<ipotesi.Length; i++)
		{
			ipotesi[i] = null;
		}
		txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in ---";
		button.colors = def;
		ipotesiEffettivaButton.gameObject.SetActive (false);
		gameManagerr.GetComponent<OperativaInterfaccia> ().goBack ();
	}
}
