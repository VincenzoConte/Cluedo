﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovCapsula : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (Vector3.forward*Time.deltaTime*10);
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (-Vector3.forward*Time.deltaTime*10);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (-Vector3.up*Time.deltaTime*100);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up*Time.deltaTime*100);
		}
	}
}
