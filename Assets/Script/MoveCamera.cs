using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public const int max = 120, min = 10;
    public const float zoomSpeed = 40f, moveSpeed=40f;
    private Vector3 pos;
    private Camera cam;

	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();
        pos = gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOut();
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomIn();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            float h = -moveSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            float v = -moveSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            transform.Translate(h, v, 0);
        }

        if (Input.GetKey(KeyCode.Space))
            InitialPosition();
    }

    public void InitialPosition()
    {
        gameObject.transform.localPosition = pos;
    }

    public void ZoomIn()
    {
        cam.fieldOfView -= zoomSpeed/8;
        if (cam.fieldOfView < min)
        {
            cam.fieldOfView = min;
        }
    }

    public void ZoomOut()
    {
        cam.fieldOfView += zoomSpeed / 8;
        if (cam.fieldOfView > max)
        {
            cam.fieldOfView = max;
        }
    }
}
