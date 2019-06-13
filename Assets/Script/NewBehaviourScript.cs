using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(string.Format("rotation:{0}", transform.rotation));
            print(string.Format("right:{0}", transform.right));
            print(string.Format("up:{0}", transform.up));
            print(string.Format("forward:{0}", transform.forward));
        }
    }
}
