using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

    public GameObject topView, playerView;
	// Use this for initialization
	void Start () {
        ActiveTopView();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActiveTopView()
    {
        topView.SetActive(true);
        playerView.SetActive(false);
    }

    public void ActivePlayerView()
    {
        topView.SetActive(false);
        playerView.SetActive(true);
    }
}
