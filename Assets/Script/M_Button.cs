using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class M_Button : MonoBehaviour {
	public Button b1;
	public Button b2;
	public Button b3;
	public Button b4;
	public Button b5;
	public Button b6;
	public Button b7;
	public Button b8;
	public Button b9;
	public Button b10;
	public Button b11;
	public Button b12;
	public Button b13;
	public Button b14;
	public Button b15;
	public Button b16;
	public Button b17;
	public Button b18;
	public Button b19;
	public Button b20;
	public Button b21;

	public Button[] buttons;
	// Use this for initialization
	void Start () {
		buttons = new Button[] {b1,b2,b3,b4,b5,b6,b7,b8,b9,b10,b11,b12,b13,b14,b15,b16,b17,b18,b19,b20,b21};
		Debug.Log (buttons.Length);
	}

	public void setIcon(int id, Sprite img)
	{
		buttons [id - 1].image.sprite = img;
 	}

	// Update is called once per frame
	void Update () {
		
	}
}
