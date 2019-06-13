using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class priview_cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(new Vector3(0, 0, 0), transform.forward * 3f, Color.blue);
        Debug.DrawLine(new Vector3(0, 0, 0), transform.up * 3f, Color.green);
        Debug.DrawLine(new Vector3(0, 0, 0), transform.right * 3f, Color.red);

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(0f, 0f, 1f, 1f);
    //    Gizmos.DrawLine(new Vector3(0, 0, 0), transform.forward * 3f);

    //    Gizmos.color = new Color(1f, 0f, 0f, 1f);
    //    Gizmos.DrawLine(new Vector3(0, 0, 0), transform.right * 3f);

    //    Gizmos.color = new Color(0f, 1f, 0f, 1f);
    //    Gizmos.DrawLine(new Vector3(0, 0, 0), transform.up * 3f);
    //}
}
