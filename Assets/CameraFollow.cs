using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float height = 10;
    public float followSpeed = 1;
    public float zoomSpeed = 1;
    public float rotateSpeed = 1;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(GameObject.Find("topPlate"))
        {
            Transform topPlate = GameObject.Find("topPlate").transform;
            Quaternion dir = Quaternion.LookRotation(topPlate.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, Time.deltaTime * 5);
            Vector3 cameraPos = transform.position;
            cameraPos.y = topPlate.position.y + height;
            transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * followSpeed);

            if (Input.GetMouseButton(1))
            {
                float h = Input.GetAxis("mouse X");
                transform.RotateAround(topPlate.position, Vector3.up, h * Time.deltaTime * rotateSpeed);
            }

            float slider = Input.GetAxis("Mouse ScrollWheel");
            GetComponent<Camera>().fieldOfView -= slider * Time.deltaTime * zoomSpeed;
        }


	}
}
