using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SwitchCamera : MonoBehaviour {

    public GameObject topView, playerView, dadiButton3, accusaButton3, ipotesiButton3, botolaButton3, endTurn3;
    


	// Use this for initialization
	void Start () {
		
		ipotesiButton3.transform.localPosition = new Vector2(-573f,18.9f);
		ipotesiButton3.transform.localScale = new Vector3 (1.2319f, 1.3500f, 1.3500f);

		botolaButton3.transform.localPosition = new Vector2(-573f,-39.2f);
		botolaButton3.transform.localScale = new Vector3 (1.2319f, 1.3500f, 1.3500f);

		dadiButton3.transform.localPosition = new Vector2(-573f,-95.9f);
		dadiButton3.transform.localScale = new Vector3 (1.2319f, 1.3500f, 1.3500f);

		accusaButton3.transform.localPosition = new Vector2(-573f,-151.2f);
		accusaButton3.transform.localScale = new Vector3 (1.2319f, 1.3500f, 1.3500f);

		endTurn3.transform.localPosition = new Vector2(-573f,-203.2f);
		endTurn3.transform.localScale = new Vector3 (1.2319f, 1.3500f, 1.3500f);

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
    }

    public void ActivePlayerView()
    {
        topView.SetActive(false);
        playerView.SetActive(true);
    }

    public void SetPlayerCamera(GameObject cam)
    {
        playerView = cam;
        ActiveTopView();
    }
}
