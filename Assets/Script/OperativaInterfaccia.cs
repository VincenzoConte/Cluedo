using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OperativaInterfaccia : MonoBehaviour {

public GameObject dadi, pedina;
public Button accusa, botola, ipotesi, endTurn;
private dice dado1,dado2;
	    GameObject colliderDadi;
    bool isMyTurn = false, lanciatoDadi, isInRoom, usatoBotola, miSonoSpostato;
	    Grid grid;
	    SwitchCamera sc;
		
		Pathfinding aStar;
		Room room;



	// Use this for initialization
	void Start () {
	    dadi.gameObject.SetActive (true);
		ipotesi.gameObject.SetActive (false);
		botola.gameObject.SetActive (false);
		grid = GameObject.Find("A*").GetComponent<Grid>();
		sc = GameObject.Find ("Gestione camera").GetComponent <SwitchCamera> ();
		//isMyTurn = false;                           //Al MOMENTO IL PRIMO TURNO è SEMPRE DEL GIOCATORE LOCALE
		isInRoom = false;
		usatoBotola = false;
		lanciatoDadi = false;
		miSonoSpostato = false;
		dado1 = GameObject.Find("dado").GetComponent<dice>();
        dado2 = GameObject.Find("dado 2").GetComponent<dice>();
        colliderDadi = GameObject.Find ("ColliderDadi");

		aStar = GameObject.Find("A*").GetComponent<Pathfinding>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Debug.Log(isMyTurn);
        if (isMyTurn) {                   //Se è il mio turno, sono in grado fin da subito a fare l'accusa o a terminare il turno
			accusa.gameObject.SetActive (true);
			endTurn.gameObject.SetActive (true);
			ipotesi.gameObject.SetActive (false);
			dadi.gameObject.SetActive (true);

			if (lanciatoDadi == true) {        //Se ho lanciato i dadi, non posso rilanciarli e non posso più utilizzare la botola (SE disponibile)
				//Rendo invisibile il bottone dadi
				dadi.gameObject.SetActive (false);
				botola.gameObject.SetActive (false);
			}

			isInRoom = findPosition ();
			Debug.Log (myRoom() +"");
			if ((miSonoSpostato  && isInRoom) || usatoBotola) {    //Se ho cambiato stanza (Lanciando i dadi o usando la botola) e sono in una stanza// posso avanzare un'ipotesi
				ipotesi.gameObject.SetActive (true);
			}

			if ((!lanciatoDadi) && isInRoom) {
			string myStanza = myRoom ();
			if (myStanza.Equals ("cucina") || myStanza.Equals ("serra") || myStanza.Equals ("studio") || myStanza.Equals ("salotto")) {   //Se mi trovo in una stanza dove c'è la botola e non ho ancora tirato i dadi
				botola.gameObject.SetActive (true);                 //posso utilizzarla
			}
			if(usatoBotola)
			{
				botola.gameObject.SetActive (false);
		        dadi.gameObject.SetActive (false);
			}

		}
		}

		else                                                               //Se non è il mio turno, non posso fare nulla 
		{                                                                  //Soluzione approssimativa. DA RIVEDERE!  
			accusa.gameObject.SetActive (false);
			endTurn.gameObject.SetActive (false);
			dadi.gameObject.SetActive (false);
			botola.gameObject.SetActive (false);
			ipotesi.gameObject.SetActive (false);
		}

		Debug.Log ("mi sono spostato: "+miSonoSpostato);
	}



	public void LanciaDadi(){

	dado1.RollTheDice ();
	dado2.RollTheDice ();
	lanciatoDadi = true;
	}

	public bool findPosition()
	{
		Node position = grid.NodeFromWorldPoint(pedina.transform.position);
	    Room stanza= grid.FindRoom(position);
		if (stanza != null)
			return true;
		return false;
	}

	public string myRoom()
	{
		Node position = grid.NodeFromWorldPoint(pedina.transform.position);
	    Room stanza= grid.FindRoom(position);
	    if(stanza!= null)
		 return stanza.getNomeStanza ();
		 else
		 return "nullo";
	}

	public void fineTurno()
	{
		isMyTurn = false;
		usatoBotola = false;
		miSonoSpostato = false;
		sc.ActiveTopView ();
		dado1.gameObject.SetActive (true);
		dado2.gameObject.SetActive (true);
        colliderDadi.gameObject.SetActive(true);
        aStar.seeker.GetComponent<GamePlayer>().CmdFineTurno(gameObject);

	}

	public void usaBotola()
	{
		string myStanza = myRoom ();
		if(myStanza.Equals("cucina"))
		{
				aStar.MoveInRoom(new Room("studio", GameObject.Find("area studio").transform));
			}
		else if(myStanza.Equals("studio"))
				{
					aStar.MoveInRoom(new Room("cucina", GameObject.Find("area cucina").transform));
				}
		else if(myStanza.Equals("serra"))
					{
						aStar.MoveInRoom(new Room("salotto", GameObject.Find("area salotto").transform));
					}
		else if(myStanza.Equals("salotto"))
						{
							aStar.MoveInRoom(new Room("serra", GameObject.Find("area serra").transform));
						}
		usatoBotola = true;
	}



	public void setTurnoTrue()             //METODO DI TEST PER ACQUISIRE MANUALMENTE IL TURNO. DA CANCELLARE. 
	{
	isMyTurn= true;
	lanciatoDadi = false;
	}

	public void setMiSonoSpostato()
	{
		miSonoSpostato = true; 
	}


}
