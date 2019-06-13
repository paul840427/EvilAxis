using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderTest : MonoBehaviour {
    // Use this for initialization
    public GameObject cube;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnSliderValueChanged(float value)
    {
        cube.transform.localScale = new Vector3(value, value, value);
    }
}
