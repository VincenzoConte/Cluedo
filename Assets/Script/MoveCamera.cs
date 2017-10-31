using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public const int max = 30, min = 5;
    public const float moveSpeed = 45f;
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
        float y = cam.gameObject.transform.localPosition.y - 1;
        if (y < min)
            y = min;
        cam.gameObject.transform.localPosition = new Vector3(cam.gameObject.transform.localPosition.x, y, cam.gameObject.transform.localPosition.z);

    }

    public void ZoomOut()
    {
        float y = cam.gameObject.transform.localPosition.y + 1;
        if (y > max)
            y = max;
        cam.gameObject.transform.localPosition = new Vector3(cam.gameObject.transform.localPosition.x, y, cam.gameObject.transform.localPosition.z);
    }
}
