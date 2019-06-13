using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))   
        {
            gameObject.transform.Rotate(0, 5, 0);
        }
	}
    private void OnMouseDrag()
    {
        print("00000000000000000000000000000000000000000000");
    }
}
