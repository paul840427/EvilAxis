using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour {
    #region parameter
    float width_scale;
    float height_scale;
    #endregion

    #region point
    float pre_x2;
    float pre_y2;
    float x2;
    float y2;
    float valid_x2;
    float valid_y2;
    #endregion

    #region Vector2
    public Vector2 mouse_pos;
    public Vector2 mouse_vector2;
    public Vector2 x_axis;
    [SerializeField] float x_axis2_length;
    public Vector2 y_axis;
    [SerializeField] float y_axis2_length;
    public Vector2 z_axis;
    [SerializeField] float z_axis2_length;
    // 三軸的二維向量
    public Dictionary<Direction, Vector2> orientation_map;
    #endregion

    #region Vector3
    Vector3 cube_pos;
    // 三軸的三維向量
    public Dictionary<Direction, Vector3> sphere_map;
    public Vector3 x_axis3;
    [SerializeField] float x_axis3_length;
    public Vector3 y_axis3;
    [SerializeField] float y_axis3_length;
    public Vector3 z_axis3;
    [SerializeField] float z_axis3_length;
    #endregion

    #region Coordinate System
    public AxisMode axis_mode;
    public Direction axis_direction;
    public GameObject XY_plane;
    public GameObject XZ_plane;
    public GameObject YZ_plane;
    [SerializeField] bool XY;
    [SerializeField] bool XZ;
    [SerializeField] bool YZ;
    #endregion

    public Transform test_cube;

    // Use this for initialization
    void Start() {
        #region parameter
        width_scale = GameInfo.SCALE / (GameInfo.RIGHT - GameInfo.LEFT);
        height_scale = GameInfo.SCALE / (GameInfo.UP - GameInfo.DOWN);
        #endregion

        #region point
        pre_x2 = 0f;
        pre_y2 = 0f;
        x2 = 0f;
        y2 = 0f;
        #endregion

        #region Vector3
        cube_pos = Vector3.zero;
        #endregion
    }

    // Update is called once per frame
    void Update() {
        // 坐標軸的模式
        axis_mode = (AxisMode)fitAxisMode();
        

        valid_x2 = Input.mousePosition.x / Screen.width;
        valid_y2 = Input.mousePosition.y / Screen.height;

        // 判斷是否超出"可操作區域"
        if(valid_x2 < GameInfo.LEFT || GameInfo.RIGHT < valid_x2 || valid_y2 < GameInfo.DOWN || GameInfo.UP < valid_y2)
        {
            // 超出"可操作區域"：維持上一時刻的位置
            x2 = pre_x2;
            y2 = pre_y2;
            GameInfo.InVaildArea = false;
        }
        else
        {
            // "可操作區域"內：將 x2, y2 轉換為 -5 ~ 5 的數值
            x2 = (valid_x2 - GameInfo.LEFT) * width_scale - GameInfo.SCALE / 2;
            y2 = (valid_y2 - GameInfo.DOWN) * height_scale - GameInfo.SCALE / 2;
            GameInfo.InVaildArea = true;
        }

        // 計算滑鼠位置
        mouse_pos = new Vector2(x2, y2);
        // 計算滑鼠移動的向量(新 減 舊)
        mouse_vector2 = mouseVector2();

        // 當滑鼠有移動，才更新 軸移動的方向
        if (mouse_vector2 != Vector2.zero)
        {
            // 往哪個軸移動
            axis_direction = (Direction)direction();
        }

        // 三軸的三維向量
        x_axis3 = transform.right;
        y_axis3 = transform.up;
        z_axis3 = transform.forward;

        x_axis3_length = x_axis3.magnitude;
        y_axis3_length = y_axis3.magnitude;
        z_axis3_length = x_axis3.magnitude;

        // 三軸投影到平面的二維向量
        x_axis = axisVector2(transform.right);
        y_axis = axisVector2(transform.up);
        z_axis = axisVector2(transform.forward);

        x_axis2_length = x_axis.magnitude;
        y_axis2_length = y_axis.magnitude;
        z_axis2_length = z_axis.magnitude;

        // 更新上一時刻的位置
        pre_x2 = x2;
        pre_y2 = y2;

        switch (GameInfo.FuntionMode)
        {
            case "Test":
                if (Input.GetMouseButtonDown(0))
                {
                    print(string.Format("x2:{0:F4}, y2:{1:F4}", Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
                }
                break;
            case "Add":
                print(string.Format("Instantiate cube: ({0:F4}, {1:F4}, {2:F4})", test_cube.position.x, test_cube.position.y, test_cube.position.z));
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {

        #region Vector2
        // 三軸在螢幕上的二維向量
        orientation_map = new Dictionary<Direction, Vector2>() {
            {Direction.Pos_X, x_axis},
            {Direction.Pos_Y, y_axis},
            {Direction.Pos_Z, z_axis},
            {Direction.Neg_X, -x_axis},
            {Direction.Neg_Y, -y_axis},
            {Direction.Neg_Z, -z_axis},
        };
        #endregion
        
        #region Vector3
        // 三軸的三維向量
        sphere_map = new Dictionary<Direction, Vector3>() {
            {Direction.Pos_X, transform.right},
            {Direction.Pos_Y, transform.up},
            {Direction.Pos_Z, transform.forward},
            {Direction.Neg_X, -transform.right},
            {Direction.Neg_Y, -transform.up},
            {Direction.Neg_Z, -transform.forward},
        };
        #endregion

        XY_plane.SetActive(XY);
        XZ_plane.SetActive(XZ);
        YZ_plane.SetActive(YZ);
    }

    public Vector2 mouseVector2()
    {
        return new Vector2(x2 - pre_x2, y2 - pre_y2);
    }

    // 三維向量轉螢幕上的二維向量
    public Vector2 axisVector2(Vector3 axis_vector)
    {
        return new Vector2(axis_vector.x, axis_vector.y);
    }

    public Vector3 cubePos()
    {        
        Vector3 axis = sphere_map[axis_direction];

        // 正射影(向量) = 正射影長 * axis 的單位向量
        Vector3 positive_projection = positiveProjection(axis, mouse_pos);

        // 根據 axis_direction 依序修改XYZ


        switch (axis_mode)
        {
            case AxisMode.XY:
                cube_pos.x = Mathf.FloorToInt(positive_projection.x);
                cube_pos.y = Mathf.FloorToInt(positive_projection.y);
                break;

            case AxisMode.XZ:
                cube_pos.x = Mathf.FloorToInt(positive_projection.x);
                cube_pos.z = Mathf.FloorToInt(positive_projection.z);
                break;

            case AxisMode.YZ:
                cube_pos.y = Mathf.FloorToInt(positive_projection.y);
                cube_pos.z = Mathf.FloorToInt(positive_projection.z);
                break;

            default:
                cube_pos.x = Mathf.FloorToInt(positive_projection.x);
                cube_pos.y = Mathf.FloorToInt(positive_projection.y);
                cube_pos.z = Mathf.FloorToInt(positive_projection.z);
                break;
        }

        return cube_pos;
    }

    // 正射影長(純量)
    public float positiveProjectionLength(Vector2 axis, Vector2 mouse_pos) {
        return Vector2.Dot(axis, mouse_pos) / axis.magnitude;
    }

    // 正射影(向量) = 正射影長 * axis 的單位向量
    public Vector3 positiveProjection(Vector3 axis, Vector2 mouse_pos)
    {
        // 正射影長(純量)
        float positive_projection_length = positiveProjectionLength(axisVector2(axis), mouse_pos);
        // axis 的單位向量
        Vector3 unit_vector3 = axis / axis.magnitude;

        return positive_projection_length * unit_vector3;
    }

    // 決定坐標軸的模式,回傳給 axis_mode
    public Enum fitAxisMode()
    {
        // 三軸在螢幕上的二維向量
        Vector2 x_axis = axisVector2(transform.right);
        Vector2 y_axis = axisVector2(transform.up);
        Vector2 z_axis = axisVector2(transform.forward);

        // 兩軸之間的夾角
        float xy = Vector2.Angle(x_axis, y_axis);
        float xz = Vector2.Angle(x_axis, z_axis);
        float yz = Vector2.Angle(y_axis, z_axis);


        // 這是 xy 軸太接近的情況
        if (xy < GameInfo.VALID_ANGLE || (180 - GameInfo.VALID_ANGLE) < xy)
        {
            // x軸 比 y軸 長
            if (y_axis.sqrMagnitude <= x_axis.sqrMagnitude)
            {
                return AxisMode.XZ;
            }
            else
            {
                return AxisMode.YZ;
            }
        }

        // 這是 xz 軸太接近的情況
        if (xz < GameInfo.VALID_ANGLE || (180 - GameInfo.VALID_ANGLE) < xz)
        {
            // x軸 比 z軸 長
            if (z_axis.sqrMagnitude <= x_axis.sqrMagnitude)
            {
                return AxisMode.XY;
            }
            else
            {
                return AxisMode.YZ;
            }
        }

        // 這是yz軸太接近的情況
        if (yz < GameInfo.VALID_ANGLE || (180 - GameInfo.VALID_ANGLE) < yz)
        {
            // Y軸 比 Z軸 長
            if (z_axis.sqrMagnitude <= y_axis.sqrMagnitude)
            {
                return AxisMode.XY;
            }
            else
            {
                return AxisMode.XZ;
            }
        }

        return AxisMode.XYZ;
    }

    // 回傳在哪個軸移動
    public Enum direction()
    {  
        switch (axis_mode)
        {
            case AxisMode.XY:
                return orientation(Direction.Pos_X, Direction.Pos_Y, Direction.Neg_X, Direction.Neg_Y);

            case AxisMode.XZ:
                return orientation(Direction.Pos_X, Direction.Pos_Z, Direction.Neg_X, Direction.Neg_Z);

            case AxisMode.YZ:
                return orientation(Direction.Pos_Y, Direction.Pos_Z, Direction.Neg_Y, Direction.Neg_Z);

            default:
                return orientation(Direction.Pos_X, Direction.Pos_Y, Direction.Pos_Z, Direction.Neg_X, Direction.Neg_Y, Direction.Neg_Z);
        }
        
    }

    public Enum orientation(params Direction[] directions)
    {
        int i, len = directions.Length, min_index = 0;

        // 滑鼠的向量 mouse_vector2 跟某個軸的角度
        float min_angle = Vector2.Angle(mouse_vector2, orientation_map[directions[0]]);

        for (i = 1; i < len; i++)
        {
            if (Vector2.Angle(mouse_vector2, orientation_map[directions[i]]) < min_angle)
            {
                min_angle = Vector2.Angle(mouse_vector2, orientation_map[directions[i]]);
                min_index = i;
            }
        }

        return directions[min_index];
    }
    
}
