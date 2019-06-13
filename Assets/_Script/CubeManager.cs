using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject prefab_cube;
    public GameObject unity_cube;
    GameObject preview_cube;
    Vector3 cube_pos;
    DimensionManager dm;

    GameObject[] children;

    // 三軸方向正射影
    Vector3 projection_x;
    Vector3 projection_y;
    Vector3 projection_z;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<DimensionManager>();
        cube_pos = Vector3.zero;

        switch (GameInfo.FuntionMode)
        {
            case "test":
                preview_cube = Instantiate(prefab_cube, Vector3.zero, transform.rotation, transform);
                break;
            case "Add":
                preview_cube = Instantiate(prefab_cube, Vector3.zero, transform.rotation, transform);
                break;
            default:
                // 預覽方塊：旋轉角度應與父物件相同
                preview_cube = Instantiate(prefab_cube, Vector3.zero, transform.rotation, transform);
                break;
        }

        children = GameObject.FindGameObjectsWithTag("Child");
        for (int i = 0; i < children.Length; i++)
        {
            children[i].transform.rotation = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameInfo.FuntionMode)
        {
            case "Test":
                updateCubeStatus();
                break;
            case "Add":
                updateCubeStatus();

                if (Input.GetMouseButtonDown(0) && GameInfo.InVaildArea)
                {
                    print(string.Format("Instantiate cube: ({0:F4}, {1:F4}, {2:F4})", cube_pos.x, cube_pos.y, cube_pos.z));
                    Instantiate(unity_cube, preview_cube.transform.position, transform.rotation, transform);
                }

                break;
            default:
                updateCubeStatus();
                break;
        }
    }

    void updateCubeStatus()
    {
        cube_pos = preview_cube.transform.position;

        switch (dm.axis_mode)
        {
            case AxisMode.XY:
                projection_x = positiveProjection(dm.x_axis3, dm.mouse_pos);
                projection_y = positiveProjection(dm.y_axis3, dm.mouse_pos);
                cube_pos = projection_x + projection_y;
                break;

            case AxisMode.XZ:
                projection_x = positiveProjection(dm.x_axis3, dm.mouse_pos);
                projection_z = positiveProjection(dm.z_axis3, dm.mouse_pos);
                cube_pos = projection_x + projection_z;
                break;

            case AxisMode.YZ:
                projection_y = positiveProjection(dm.y_axis3, dm.mouse_pos);
                projection_z = positiveProjection(dm.z_axis3, dm.mouse_pos);
                cube_pos = projection_y + projection_z;
                break;

            default:
                projection_x = positiveProjection(dm.x_axis3, dm.mouse_pos);
                projection_y = positiveProjection(dm.y_axis3, dm.mouse_pos);
                projection_z = positiveProjection(dm.z_axis3, dm.mouse_pos);
                cube_pos = projection_x + projection_y + projection_z;
                break;
        }

        //cube_pos.x = Mathf.FloorToInt(cube_pos.x);
        //cube_pos.y = Mathf.FloorToInt(cube_pos.y);
        //cube_pos.z = Mathf.FloorToInt(cube_pos.z);

        if (float.IsNaN(cube_pos.x) || float.IsNaN(cube_pos.y) || float.IsNaN(cube_pos.z))
        {
            print(string.Format("cube_pos:{0}", cube_pos));
            cube_pos = Vector3.zero;

        }

        preview_cube.transform.position = cube_pos;
        preview_cube.transform.rotation = transform.rotation;
    }

    Vector3 positiveProjection(Vector3 axis, Vector2 mouse_pos)
    {
        // 正射影長(純量)
        float positive_projection_length = dm.positiveProjectionLength(dm.axisVector2(axis), mouse_pos);
        // axis 的單位向量
        Vector3 unit_vector3 = axis / axis.magnitude;

        return Mathf.FloorToInt(positive_projection_length) * unit_vector3;
    }
}
