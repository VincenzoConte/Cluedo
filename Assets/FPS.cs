﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = ((int)(1f / Time.deltaTime)).ToString();
    }
}
