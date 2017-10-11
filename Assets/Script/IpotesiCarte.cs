using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class IpotesiCarte : MonoBehaviour {

	private static string[] ipotesi;
	public Text txt;
	private ColorBlock cb;
	private static ColorBlock defS, defA;
	public Button buttonS, buttonA;
	private static Button prevSbutton, prevAbutton;
	public GameObject gameManagerr;
	public Button ipotesiEffettivaButton;
	public Image avatar;
	private GamePlayer player;

	// Use this for initialization
	public void OnEnable () {
		ipotesi = new string[3];
		ipotesiEffettivaButton.gameObject.SetActive (false);
		if(buttonS==null)
			buttonS = this.GetComponent<Button> ();		
		if(buttonA==null)
			buttonA = this.GetComponent<Button> ();	
		prevSbutton = buttonS; //this.GetComponent<Button> ();
		prevAbutton = buttonA; //this.GetComponent<Button> ();
		defS = buttonS.colors;
		defA = buttonA.colors;
		ipotesi [2] = gameManagerr.GetComponent<OperativaInterfaccia> ().myRoom ();
		avatar.sprite = gameManagerr.GetComponent<OperativaInterfaccia> ().avatar1.sprite;
		txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in "+ ipotesi[2];
	}
	


	public void goBack()
	{
		ripristina ();
		gameManagerr.GetComponent<OperativaInterfaccia> ().goBack ();

	}

	public void doIpotesi()
	{
		GameObject.Find ("A*").GetComponent<Pathfinding> ().seeker.GetComponent<GamePlayer> ().CmdIpotesi (ipotesi);
		ripristina ();
		gameManagerr.GetComponent<OperativaInterfaccia> ().DoIpotesiEffettiva ();
	}


	public void selectedCard(string category, string nameCard, Button b)
	{
		ipotesi [2] = gameManagerr.GetComponent<OperativaInterfaccia> ().myRoom ();
		if (category == "Sospetto")
		{
			if (nameCard == ipotesi [0])			//Sto deselezionando la carta Sospetto
			{
				b.colors = defS;
				ipotesi [0] = null; 
				if (ipotesi [1]!= null)
				{
					txt.text= "Secondo me è stato (scegli sospetto) con "+ ipotesi[1] + " in "+ ipotesi[2];
				}
				else
				{
					txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in "+ ipotesi[2];
				}
			}else								//Sto selezionando la carta Sospetto
			{
				ipotesi [0] = nameCard;
				prevSbutton.colors = defS;
				cb = b.colors;
				cb.normalColor = new Color (0.118f, 1f, 0f, 1.0f);
				b.colors = cb;
				ipotesi [0] = nameCard;
				if (ipotesi [1]!= null)	
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in "+ ipotesi[2];
				}
				else
				{
					txt.text = "Secondo me è stato "+ ipotesi[0] +" con (scegli arma) in "+ ipotesi[2];
				}
				prevSbutton = b;
			}
		}
		else if(category == "Arma")
		{
			if (nameCard == ipotesi [1])			//Sto deselezionando la carta Arma
			{
				b.colors = defA;
				ipotesi [1] = null; 
				if (ipotesi [0]!= null)
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con (scegli arma) in "+ ipotesi[2];
				}
				else
				{
					txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in "+ ipotesi[2];
				}
			}else							//Sto selezionando la carta Arma
			{
				prevAbutton.colors = defA;
				cb = b.colors;
				cb.normalColor = new Color (0f, 0.545f, 1.0f, 1.0f);
				b.colors = cb;
				prevAbutton.colors = defA;
				ipotesi [1] = nameCard;
				if (ipotesi [0]!= null)
				{
					txt.text= "Secondo me è stato "+ ipotesi[0] + " con "+ ipotesi[1] + " in "+ ipotesi[2];
				}
				else
				{
					txt.text= "Secondo me è stato (scegli sospetto) con "+ ipotesi[1] + " in "+ ipotesi[2];
				}
				prevAbutton = b;
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

	public void ripristina()
	{
		prevAbutton.colors = defA;
		prevSbutton.colors = defS;
		for(int i = 0; i<ipotesi.Length; i++)
		{
			ipotesi[i] = null;
		}
		txt.text = "Secondo me è stato (scegli sospetto) con (scegli arma) in "+ ipotesi[2];
		buttonS.colors = defS;
		buttonA.colors = defA;
		ipotesiEffettivaButton.gameObject.SetActive (false);
	}
}
