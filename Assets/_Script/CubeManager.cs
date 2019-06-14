using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public Camera ray_camera;
    public GameObject prefab_cube;
    public GameObject unity_cube;
    GameObject preview_cube;
    Vector3 cube_pos;
    DimensionManager dm;
    ExcelManager em;

    // 避免重複生成物件
    bool[,,] cube_exist;
    bool cube_not_exist;
    Vector3 coordinate;

    // 三軸方向正射影
    Vector3 projection_x;
    Vector3 projection_y;
    Vector3 projection_z;

    // Start is called before the first frame update
    void Start()
    {
        dm = GetComponent<DimensionManager>();
        em = GetComponent<ExcelManager>();
        cube_pos = Vector3.zero;
        cube_exist = new bool[10, 10, 10];

    }

    // Update is called once per frame
    void Update()
    {
        switch (GameInfo.FunctionMode)
        {
            case EFunction.Add:
                if(preview_cube == null)
                {
                    preview_cube = Instantiate(prefab_cube, Vector3.zero, transform.rotation, transform);
                }

                updateCubeStatus();

                if (Input.GetMouseButtonDown(0) && GameInfo.InVaildArea)
                {
                    coordinate = dm.positionToCoordinate(cube_pos);
                    cube_not_exist = !cube_exist[(int)coordinate.x, (int)coordinate.y, (int)coordinate.z];

                    // 若方塊不存在，則可生成新方塊
                    if (cube_not_exist)
                    {
                        Instantiate(unity_cube, preview_cube.transform.position, transform.rotation, transform);
                        print(string.Format("Instantiate cube: ({0:F4}, {1:F4}, {2:F4}) @ ({3:F4}, {4:F4}, {5:F4})",
                            coordinate.x, coordinate.y, coordinate.z,
                            cube_pos.x, cube_pos.y, cube_pos.z));

                        cube_exist[(int)coordinate.x, (int)coordinate.y, (int)coordinate.z] = true;

                        em.saveData(coordinate, EFunction.Add);
                    }
                }

                break;

            case EFunction.Del:
                if (preview_cube != null)
                {
                    Destroy(preview_cube);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = ray_camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    // layer 使用數值為 2進制， Cube 的 layer 在 12，故使用 1 << 12
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Cube")))
                    {
                        coordinate = dm.positionToCoordinate(hit.transform.position);
                        cube_exist[(int)coordinate.x, (int)coordinate.y, (int)coordinate.z] = false;
                        Destroy(hit.collider.gameObject);
                        print(string.Format("Destroy cube:({0:F4}, {1:F4}, {2:F4}) @ ({3:F4}, {4:F4}, {5:F4})",
                                coordinate.x, coordinate.y, coordinate.z,
                                hit.transform.position.x, hit.transform.position.y, hit.transform.position.z));

                        em.saveData(coordinate, EFunction.Del);
                    }
                }
               
                break;

            case EFunction.Test:
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
