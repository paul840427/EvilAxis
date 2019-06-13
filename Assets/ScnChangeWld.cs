using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScnChangeWld : MonoBehaviour {
    Vector3 mouse_p;
    float x1 = 0;
    float y1 = 0;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Cube")
                {
                    mouse_p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    mouse_p.z = 1.0f;
                    print(mouse_p);
                    transform.position = mouse_p;

                    //print("hit");
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Cube01")
                {
                    mouse_p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    if (mouse_p.x > 1)
                        x1 = 1;
                    else if (mouse_p.x < 1)
                        x1 = -1;
                    else
                        x1 = 0;

                    if (mouse_p.y > 1)
                        y1 = 1;
                    else if (mouse_p.y < 1)
                        y1 = -1;
                    else
                        y1 = 0;
                    transform.Rotate(new Vector3(y1, x1, 0f));
                    
                }
            }
        }

        //OnTriggerStay(aaa);
    }
}
