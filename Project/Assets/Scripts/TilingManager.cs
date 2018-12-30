using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TilingManager : MonoBehaviour {
    private Vector3 Current_Size;

	// Use this for initialization
	void Start () {
        Current_Size = transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.localScale != Current_Size)
        {
            Current_Size = transform.localScale;
            gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2 (Current_Size.x, Current_Size.z);
        }



		
	}
}

