using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Experimental.UIElements;
using System.Security.Cryptography;

public class ButtonInventary : MonoBehaviour {


public Sprite question;
public Sprite check;
public Sprite x;
public Sprite locked;
public Sprite empty;
private int count;
public int id;
public M_Button mb;

	// Use this for initialization
	void Start () {
		count = 0;
	}

	public void click ()
	{
		GameObject go = GameObject.Find ("GameManager");
		M_Button mb = (M_Button)go.GetComponent ((typeof(M_Button)));
	Debug.Log ("cliccato!"+ id);
		count = (count + 1) % 4;
		if (count == 0)
			mb.setIcon(id, empty);
			else
			if(count==1)
				mb.setIcon(id, x);
				else
				if(count==2)
					mb.setIcon(id, check);
					else
					if(count == 3)
						mb.setIcon(id, question);

	}

	// Update is called once per frame
	void Update () {
		
	}
}
