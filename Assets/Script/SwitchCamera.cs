using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SwitchCamera : MonoBehaviour {

    public GameObject topView, playerView, dadiButton3, accusaButton3, ipotesiButton3, botolaButton3, endTurn3;
    


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	//Switch da prima a terza persona e viceversa
		if((Input.GetKey (KeyCode.Keypad1))||Input.GetKey (KeyCode.Alpha1)){
			ActivePlayerView ();
		}
		if((Input.GetKey (KeyCode.Keypad3))||Input.GetKey (KeyCode.Alpha3)){
			ActiveTopView ();
		}
	}

    public void ActiveTopView()
    {
        topView.SetActive(true);
        playerView.SetActive(false);

		//Sposto gli elementi della UI

		ipotesiButton3.transform.localPosition = new Vector3(-309f,81.932f,0);
		ipotesiButton3.transform.localScale = new Vector3 (0.9839f, 1.0782f, 1.0782f);

		botolaButton3.transform.localPosition = new Vector3(-309f,35.566f,0);
		botolaButton3.transform.localScale = new Vector3 (0.9839f, 1.0782f, 1.0782f);

		dadiButton3.transform.localPosition = new Vector3(-309f,-9.7223f,0);
		dadiButton3.transform.localScale = new Vector3 (0.9839f, 1.0782f, 1.0782f);

		accusaButton3.transform.localPosition = new Vector3(-309f,-53.932f,0);
		accusaButton3.transform.localScale = new Vector3 (0.9839f, 1.0782f, 1.0782f);

		endTurn3.transform.localPosition = new Vector3(-309f,-96.7f,0);
		endTurn3.transform.localScale = new Vector3 (0.9839f, 1.0782f, 1.0782f);

      
    }

    public void ActivePlayerView()
    {
        topView.SetActive(false);
        playerView.SetActive(true);

         //Sposto gli elementi della UI

		ipotesiButton3.transform.localPosition = new Vector3(-366f,-26.4f,0);
		ipotesiButton3.transform.localScale = new Vector3 (0.5492f, 0.5492f, 0.5492f);

		botolaButton3.transform.localPosition = new Vector3(-366f,-50.01699f,0);
		botolaButton3.transform.localScale = new Vector3 (0.5492f, 0.5492f, 0.5492f);

		dadiButton3.transform.localPosition = new Vector3(-366f,-71.6811f,0);
		dadiButton3.transform.localScale = new Vector3 (0.5492f, 0.5492f, 0.5492f);

		accusaButton3.transform.localPosition = new Vector3(-366f,-94.2f,0);
		accusaButton3.transform.localScale = new Vector3 (0.5492f, 0.5492f, 0.5492f);

		endTurn3.transform.localPosition = new Vector3(-366f,-116.496f,0);
		endTurn3.transform.localScale = new Vector3 (0.5492f, 0.5492f, 0.5492f);
    }

    public void SetPlayerCamera(GameObject cam)
    {
        playerView = cam;
        ActiveTopView();
    }
}
