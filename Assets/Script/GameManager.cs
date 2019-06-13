using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject sphere;
    public GameObject prefab_cube;
    GameObject preview_cube;
    public GameObject cube;
    
    //------------------------------------------------------


    public DimensionManager dimension;
    Vector3 cube_pos;
    Vector3 cube_rot;
    Vector3 Value_rot = new Vector3(0,0,0);
    // Use this for initialization
    void Start () {
        preview_cube = Instantiate(prefab_cube, Vector3.zero, Quaternion.identity);

    }
	
	// Update is called once per frame
	void Update () {
        cube_pos = dimension.cubePos();
               
        //print("cube_x: " + cube_x + "cube_y: " + cube_y);
        //print("x_axis: " + dimension.x_axis + "y_axis: " + dimension.y_axis + "z_axis" + dimension.z_axis);
        preview_cube.transform.position = cube_pos;
        //print(cube_pos);

        if (Input.GetMouseButtonDown(0))
        {
            mouseDown();
        }
        
    }

    public void mouseDown()
    {
        Instantiate(cube, cube_pos, transform.rotation, transform);
    }

    

    private void OnMouseDrag()
    {
        print("00000000000000000000000000000000000000000000");
    }

    public void OnSliderValueXChanged(float valueX)
    {
        float x = valueX - Value_rot.x;
        sphere.transform.Rotate(new Vector3(x, 0, 0));
        preview_cube.transform.Rotate(new Vector3(x, 0, 0));
        Value_rot.x = valueX;

        Debug.Log("OnSliderValueXChanged :" + x);
    }
    public void OnSliderValueYChanged(float valueY)
    {
        float y = valueY - Value_rot.y;
        sphere.transform.Rotate(new Vector3(0, y, 0));
        preview_cube.transform.Rotate(new Vector3(0, y, 0));
        Value_rot.y = valueY;
    }
    public void OnSliderValueZChanged(float valueZ)
    {
        float z = valueZ - Value_rot.z;
        sphere.transform.Rotate(new Vector3(0, 0, z));
        preview_cube.transform.Rotate(new Vector3(0, 0, z));
        Value_rot.z = valueZ;
    }

}
