using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTest : MonoBehaviour {
    DimensionManager aaa;
    // Use this for initialization
    void Start () {
        aaa = GetComponent<DimensionManager>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            print(aaa.mouse_vector2);
        }
    }

    public void OnMouseDown()
    {
        print("OnMouseDown");
    }

    
}
