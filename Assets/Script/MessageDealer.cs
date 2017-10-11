using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class MessageDealer : MonoBehaviour {
 
	public Image mainImage, image1, image2, image3, image4, image5;
	public Text  mainText, text1, text2, text3, text4, text5;
	public static int counter;
	private GameObject [] uiMessages = new GameObject[7] ;
	

	// Use this for initialization
	void OnEnable () {
		}
	
	public void setMessageOnScreen (string imagePlayer, string message)
	{
		Sprite tmpSprite = Resources.Load<Sprite>("Immagini/PG/"+imagePlayer);
		GameObject.Find ("GameManager").GetComponent<OperativaInterfaccia> ().ShowMessaggiPanel ();

			switch (counter) {
			case 1:
				uiMessages [1] = transform.Find ("Messaggi1").gameObject;
				uiMessages [1].SetActive (true);
				image1.sprite = tmpSprite;
				text1.text = message;
				break;
			case 2:
				uiMessages [2] = transform.Find ("Messaggi2").gameObject;
				uiMessages [2].SetActive (true);
				image2.sprite = tmpSprite;
				text2.text = message;
				break;
			case 3:
				uiMessages [3] = transform.Find ("Messaggi3").gameObject;
				uiMessages [3].SetActive (true);
				image3.sprite = tmpSprite;
				text3.text = message;
				break;
			case 4:
				uiMessages [4] = transform.Find ("Messaggi4").gameObject;
				uiMessages [4].SetActive (true);
				image4.sprite = tmpSprite;
				text4.text = message;
				break;
			case 5:
				uiMessages [5] = transform.Find ("Messaggi5").gameObject;
				uiMessages [5].SetActive (true);
				image5.sprite = tmpSprite;
				text5.text = message;
				break;
			}
			counter++;
	}

	public void decrementaCounter()
	{
		counter--;
	}

	public void resetMessagePanel()
	{
		for(int i = 1; i<counter; i++)
		{
			uiMessages [i].gameObject.SetActive (false);
		}
		counter = 1;
	}
}
