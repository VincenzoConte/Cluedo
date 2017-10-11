using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OperativaInterfaccia : MonoBehaviour {

	public GameObject dadi;
	public Button accusa, botola, ipotesi, endTurn;
	public GameObject ipotesiPanel, mostraCartaPanel, messaggiPanel, bottoni;
	public Image avatar1;
	public Text messaggioUI;
	private dice dado1,dado2;
	private	bool [] saveState;
	GameObject colliderDadi;
    bool isMyTurn = false, lanciatoDadi, isInRoom, usatoBotola, miSonoSpostato, fattoIpotesi;
	public Grid grid;
	SwitchCamera sc;
	public Pathfinding aStar;
	Room room;

	// Use this for initialization
	void Start () {
		sc = GameObject.Find ("Gestione camera").GetComponent <SwitchCamera> ();
		isMyTurn = false;
		isInRoom = false;
		usatoBotola = false;
		lanciatoDadi = false;
		miSonoSpostato = false;
		fattoIpotesi = false;
		dado1 = GameObject.Find("dado").GetComponent<dice>();
        dado2 = GameObject.Find("dado 2").GetComponent<dice>();
        colliderDadi = GameObject.Find ("ColliderDadi");
		saveState = new bool[5];
	}
	
	// Update is called once per frame
	void Update ()
	{
        //Debug.Log(isMyTurn);				//E' IL MIO TURNO?
        if (isMyTurn) 
		{                   

			if (lanciatoDadi == true) {        //Se ho lanciato i dadi, non posso rilanciarli e non posso più utilizzare la botola (SE disponibile)
				//Rendo invisibile il bottone dadi
				dadi.gameObject.SetActive (false);
				botola.gameObject.SetActive (false);
			}

			isInRoom = findPosition ();
		//	Debug.Log (myRoom() +"");						//IN CHE STANZA MI TROVO?
			if (((miSonoSpostato  && isInRoom) || usatoBotola ) && !fattoIpotesi) {    //Se ho cambiato stanza (Lanciando i dadi o usando la botola) e sono in una stanza e non ho fatto ipotesi // posso avanzare un'ipotesi
				ipotesi.gameObject.SetActive (true);
			}
			if(fattoIpotesi){
                bottoni.SetActive(true);
				ipotesi.gameObject.SetActive (false);
				accusa.gameObject.SetActive (true);
				endTurn.gameObject.SetActive (true);
			}
			if ((!lanciatoDadi) && isInRoom) 
			{
				string myStanza = myRoom ();
				if (myStanza.Equals ("Cucina") || myStanza.Equals ("Serra") || myStanza.Equals ("Studio") || myStanza.Equals ("Salotto")) {   //Se mi trovo in una stanza dove c'è la botola e non ho ancora tirato i dadi
					botola.gameObject.SetActive (true);                 //posso utilizzarla
			}
			if(usatoBotola)
			{
				botola.gameObject.SetActive (false);
		        dadi.gameObject.SetActive (false);
			}

			}
		}

		else{
			//nothing
		}

		//Debug.Log ("mi sono spostato: "+miSonoSpostato);
	}



	public void LanciaDadi()
	{
		accusa.gameObject.SetActive (false);
		endTurn.gameObject.SetActive (false);
        aStar.seeker.GetComponent<GamePlayer>().CmdLanciaDadi(dado1.gameObject, dado2.gameObject);
		lanciatoDadi = true;
		accusa.gameObject.SetActive (true);
		endTurn.gameObject.SetActive (true);
	}

	public bool findPosition()
	{
		Node position = grid.NodeFromWorldPoint(aStar.seeker.transform.position);
	    Room stanza= grid.FindRoom(position);
		if (stanza != null)
			return true;
		return false;
	}

	public string myRoom()
	{
		Node position = grid.NodeFromWorldPoint(aStar.seeker.transform.position);
	    Room stanza= grid.FindRoom(position);
	    if(stanza!= null)
		 return stanza.getNomeStanza ();
		 else
		 return "nullo";
	}

	public void fineTurno()
	{
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("target"))
            GameObject.Destroy(t, 0f);
        isMyTurn = false;
		usatoBotola = false;
		miSonoSpostato = false;
		fattoIpotesi = false;
		sc.ActiveTopView ();
		dado1.gameObject.SetActive (true);
		dado2.gameObject.SetActive (true);
        colliderDadi.gameObject.SetActive(true);
		HideButtons ();
        aStar.seeker.GetComponent<GamePlayer>().CmdFineTurno(gameObject);
	}

	public void usaBotola()
	{
		string myStanza = myRoom ();
		if(myStanza.Equals("Cucina"))
		{
				aStar.MoveInRoom(new Room("Studio", GameObject.Find("area studio").transform));
			}
		else if(myStanza.Equals("Studio"))
				{
					aStar.MoveInRoom(new Room("Cucina", GameObject.Find("area cucina").transform));
				}
		else if(myStanza.Equals("Serra"))
					{
						aStar.MoveInRoom(new Room("Salotto", GameObject.Find("area salotto").transform));
					}
		else if(myStanza.Equals("Salotto"))
						{
							aStar.MoveInRoom(new Room("Serra", GameObject.Find("area serra").transform));
						}
		usatoBotola = true;
	}

	public void setTurnoTrue()             //METODO DI TEST PER ACQUISIRE MANUALMENTE IL TURNO. DA CANCELLARE. 
	{
        GamePlayer player = aStar.seeker.GetComponent<GamePlayer>();
        if (player.turniDaSaltare > 0)
        {
            player.turniDaSaltare--;
            player.CmdFineTurno(gameObject);
            return;
        }
        player.CmdGiocatoreAttivo(player.gameObject);
        isMyTurn = true;
	    lanciatoDadi = false;

		//Se è il mio turno, sono in grado fin da subito a fare l'accusa o a terminare il turno
		accusa.gameObject.SetActive (true);
		endTurn.gameObject.SetActive (true);
		ipotesi.gameObject.SetActive (false);
		dadi.gameObject.SetActive (true);
	}

	public void setMiSonoSpostato()
	{
		miSonoSpostato = true; 
	}

	public void showIpotesiPanel()
	{
		saveState[0] = dadi.activeSelf;					//Mi salvo il valore dei pulsanti visibili e non sull'interfaccia
		saveState[1] = botola.gameObject.activeSelf;
		saveState[2] = ipotesi.gameObject.activeSelf;
		saveState[3] = accusa.gameObject.activeSelf;
		saveState[4] = endTurn.gameObject.activeSelf;

		ipotesiPanel.SetActive (true);
        //ipotesiPanel.GetComponent<IpotesiCarte> ().Start ();
        /*dadi.SetActive (false);
		botola.gameObject.SetActive (false);
		ipotesi.gameObject.SetActive (false);
		accusa.gameObject.SetActive (false);
		endTurn.gameObject.SetActive (false);*/
        bottoni.SetActive(false);

	}

	public void goBack()
	{
        bottoni.SetActive(true);
		dadi.SetActive (saveState [0]);
		botola.gameObject.SetActive (saveState [1]);
		ipotesi.gameObject.SetActive (saveState [2]);
		accusa.gameObject.SetActive (saveState [3]);
		endTurn.gameObject.SetActive (saveState [4]);	
        
		ipotesiPanel.SetActive (false);
	}

    public void AggiornaInterfaccia(GameObject player)
    {
        GamePlayer p = player.GetComponent<GamePlayer>();
		avatar1.sprite = p.playerImage;
		messaggioUI.text = "E' il turno di "+ p.character;
    }

	public void DoIpotesiEffettiva ()
	{
		fattoIpotesi = true;
		ipotesiPanel.gameObject.SetActive (false);
	}

    public bool IsMyTurn()
    {
        return isMyTurn;
    }

	public void HideButtons(){
		dadi.SetActive (false);
		botola.gameObject.SetActive (false);
		ipotesi.gameObject.SetActive (false);
		accusa.gameObject.SetActive (false);
		endTurn.gameObject.SetActive (false);
	}

	public void ShowMessaggiPanel()
	{
		messaggiPanel.gameObject.SetActive (true);
	}

	public void HideMessaggiPanel()
	{
		messaggiPanel.gameObject.SetActive(false);
	}


}
