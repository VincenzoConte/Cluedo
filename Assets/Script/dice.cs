using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public class dice : MonoBehaviour {
 
    public List<Vector3> directions;
    public List<int> sideValues;
    public float forceAmount = 20.0f;
    public float torqueAmount = 50.0f;
    public ForceMode forceMode;
    public int value;
    SwitchCamera changeView;
    Rigidbody rb;
    bool flag;
    void Start ()
	{
        value = 0;
        changeView = GameObject.Find("Gestione camera").GetComponent<SwitchCamera>();
        if (directions.Count == 0) {
			// Object space directions
			directions.Add (Vector3.up);
			sideValues.Add (5); // up
			directions.Add (Vector3.down);
			sideValues.Add (2); // down
 
			directions.Add (Vector3.left);
			sideValues.Add (3); // left
			directions.Add (Vector3.right);
			sideValues.Add (4); // right
 
			directions.Add (Vector3.forward);
			sideValues.Add (1); // fw
			directions.Add (Vector3.back);
			sideValues.Add (6); // back
			rb = this.GetComponent<Rigidbody> ();
			flag = true;
		}
    }

     void Update ()
	{
     }



     public void RollTheDice(){
        //lancia
		changeView.ActiveTopView ();
			rb.AddForce (Random.onUnitSphere * forceAmount, forceMode);
			rb.AddTorque (Random.onUnitSphere * torqueAmount, forceMode);
			flag = false;
 
			if (rb.IsSleeping ()) {
				if (flag == false) {
					value = GetNumber (Vector3.up, 30f);
					Debug.Log (value);
					flag = true;
				}
			}
     }

     public int GetNumber(Vector3 referenceVectorUp, float epsilonDeg = 5f) {
         Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);
 
         float min = float.MaxValue;
         int mostSimilarDirectionIndex = -1;
         for (int i=0; i < directions.Count; ++i) {
             float a = Vector3.Angle(referenceObjectSpace, directions[i]);
             if (a <= epsilonDeg && a < min) {
                 min = a;
                 mostSimilarDirectionIndex = i;
             }
         }
         return (mostSimilarDirectionIndex >= 0) ? sideValues[mostSimilarDirectionIndex] : -1; 
     }
 }