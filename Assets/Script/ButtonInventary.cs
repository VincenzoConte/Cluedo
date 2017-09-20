using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInventary : MonoBehaviour {


public Sprite question;
public Sprite check;
public Sprite x;
public Sprite locked;
public Sprite empty;
private int count;
public int id;
public M_Button mb;
private string[] cards;

	// Use this for initialization
	void Start () {
		count = 0;

		cards = new string[] {"Dolphin Rouge","Vincent Count","Mark Johnson","Freddie Carneval","Anne Marie","Emma Stacy",
			"Corda","Pistola","Chiave inglese","Pugnale","Candeliere","Tubo di piombo",
			"Ingresso","Salotto","Sala da pranzo","Cucina","Sala da ballo","Serra","Sala da biliardo",
			"Biblioteca","Studio"};
	}

	public void click ()
	{
		GameObject go = GameObject.Find ("GameManager");
		mb = (M_Button)go.GetComponent ((typeof(M_Button)));
		if (count != 4) 
		{
			count = (count + 1) % 4;
			if (count == 0)
				mb.setIcon (id, empty);
			else if (count == 1)
				mb.setIcon (id, x);
			else if (count == 2)
				mb.setIcon (id, check);
			else if (count == 3)
				mb.setIcon (id, question);
		}
		else
			return;
	}

	public void lockSelection(int id)
	{
		GameObject go = GameObject.Find ("GameManager");
		mb = (M_Button)go.GetComponent ((typeof(M_Button)));
		count = 4;
		mb.setIcon (id, locked);
	}
}
