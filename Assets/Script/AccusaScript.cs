using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccusaScript : MonoBehaviour {

	private static string[] accusa = new string[3];
	public Text txt;
	//private ColorBlock cb;
	//private static ColorBlock defS, defA;
	//public Button buttonS, buttonA;
	//private static Button prevSbutton, prevAbutton;
	public Image imageSospetto, imageArma, imageStanza;
	public GameObject gameManagerr;
	public Button accusaEffettivaButton;
	public Image avatar;
	private GamePlayer player;
	private PanelDealer pd;

	void Start ()
	{
		pd = GameObject.Find ("AccusaPanel").GetComponent<PanelDealer> ();
		accusa [0] =  "(scegli sospetto)";
		accusa [1] =  "(scegli arma)";
		accusa [2] =  "(scegli stanza)";
	}

	public void OnEnable()
	{
		this.GetComponent<PanelDealer> ().hidePanelSospetto ();
		this.GetComponent<PanelDealer> ().hidePanelArma ();
		this.GetComponent<PanelDealer> ().hideStanzaPanel ();
	}

	public void selectCardAccusa(string category, string nameCard)
	{
		if (category == "Sospetto")
		{
			accusa [0] = nameCard;
			imageSospetto.sprite =  Resources.Load<Sprite>("Immagini/Carte/"+nameCard.Replace (" ", ""));
			pd.hidePanelSospetto();

		}
		else if(category == "Arma")
		{
			accusa [1] = nameCard;
			imageArma.sprite = Resources.Load<Sprite>("Immagini/Carte/"+nameCard.Replace (" ", ""));
			pd.hidePanelArma ();

		}else if(category == "Stanza")
		{
			accusa [2] = nameCard;
			imageStanza.sprite = Resources.Load<Sprite>("Immagini/Carte/"+nameCard.Replace (" ", ""));
			pd.hideStanzaPanel ();
		}

		if( !(accusa[0].Equals("(scegli sospetto)")) && !(accusa[1].Equals("(scegli arma)")) && !(accusa[2].Equals("(scegli stanza)")))
		{
			accusaEffettivaButton.gameObject.SetActive (true);
		} else {
			accusaEffettivaButton.gameObject.SetActive (false);										//Probabilmente else inutile
		}
		txt.text = "Accuso "+accusa[0]+" con "+accusa[1]+" in  "+accusa[2];
	}

	public void doAccusa()
	{
		GameObject.Find ("A*").GetComponent<Pathfinding> ().seeker.GetComponent<GamePlayer> ().CmdAccusa (accusa);
		resetta ();
		gameManagerr.GetComponent<OperativaInterfaccia> ().DoAccusaEffettiva ();
	}

	public void goBack()
	{
		gameManagerr.GetComponent<OperativaInterfaccia> ().goBackAccusa ();
	}

	public void goOneBack()
	{
		this.GetComponent<PanelDealer> ().showMainPanel ();
	}

	public void resetta()
	{
		accusa [0] =  "(scegli sospetto)";
		accusa [1] =  "(scegli arma)";
		accusa [2] =  "(scegli stanza)";

		txt.text = "Accuso "+accusa[0]+" con "+accusa[1]+" in  "+accusa[2];
		imageSospetto.sprite = Resources.Load<Sprite>("Immagini/Carte/Sospetto");
		imageArma.sprite = Resources.Load<Sprite>("Immagini/Carte/Arma");
		imageStanza.sprite = Resources.Load<Sprite>("Immagini/Carte/Stanza");
		accusaEffettivaButton.gameObject.SetActive (false);
	}


}
