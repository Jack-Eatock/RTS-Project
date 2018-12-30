using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float FF = 15f; // Forward Force.
    public float SF = 15f; // Sideways Force.
    public float scrollspeed = 10f; // Scroll speed
    public float scroll;

    public GameObject Movement;
    

	// Update is called once per frame
	void Update () {
        Vector3 Rotation_ = transform.rotation.eulerAngles;
        Vector3 Pos = transform.position;

        // Rotation on Y axis using Q and E ,

        if (Input.GetKey(KeyCode.Q))
        {
            Rotation_.y -= 1;

        }

        if (Input.GetKey(KeyCode.E))
        {
            Rotation_.y += 1;
        }

        // Middle Click drag,
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") < 0)
            {
               
                Rotation_.y -= 2 * - Input.GetAxis("Mouse X");
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
               
                Rotation_.y += 2 * Input.GetAxis("Mouse X"); ;
            }
            if(Input.GetAxis("Mouse Y") > 0)
            {
               
                Rotation_.x -= 2 * Input.GetAxis("Mouse Y");
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
               
                Rotation_.x += 2 * - Input.GetAxis("Mouse Y");
            }

        }
        

        // Scroll wheel,
        scroll = Input.GetAxis("Mouse ScrollWheel");
        Pos.y -= scroll * scrollspeed * -50f * Time.deltaTime;


        // Moving The Camera,

        if (Input.GetKey(KeyCode.W))
        {
            Pos += Movement.transform.forward * FF * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Pos -= Movement.transform.forward * FF * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Pos -= Movement.transform.right * FF * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Pos += Movement.transform.right * FF * Time.deltaTime;
        }


        Movement.transform.eulerAngles = new Vector3 (0, Rotation_.y, 0);
        transform.eulerAngles = Rotation_;
        transform.position = Pos;
    }

    
}
