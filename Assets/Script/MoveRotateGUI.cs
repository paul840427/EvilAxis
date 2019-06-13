using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotateGUI : MonoBehaviour {
    readonly float SCALE = 10f;

    float x2;
    float width_scale;
    float height_scale;

    public float Pos_X;
    public float Pos_Y;
    public float Dir_pos;
    public GameObject Target;
    
    // Use this for initialization
    void Start () {
        width_scale = Screen.width / SCALE;
        height_scale = Screen.height / SCALE;

        Pos_Y = Target.GetComponent<Transform>().position.y;
        Pos_X = Target.GetComponent<Transform>().position.x;
    }

    // Update is called once per frame
    void Update () {
        x2 = Input.mousePosition.x / width_scale - SCALE / 2;
        if (Input.GetMouseButton(0))
        {
            print("111111111111111111111111111");
            transform.position = new Vector2(Pos_X+x2*20, Pos_Y);
        }
    }
    
    private void OnMouseDrag()
    {
        print("0000000000000000000000000000000000");
        
    }
}
