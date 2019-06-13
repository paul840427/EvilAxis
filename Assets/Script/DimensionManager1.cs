using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager1 : MonoBehaviour {
    readonly float SCALE = 10f;

    readonly float WIDTH_LEFT = 0.4f;
    readonly float WIDTH_RIGHT = 0.9f;
    readonly float HEIGHT_UP = 0.85f;
    readonly float HEIGHT_DOWN = 0.05f;

    readonly float MIN_X = -5f;
    readonly float MAX_X = 5f;
    readonly float MIN_Y = -5f;
    readonly float MAX_Y = 5f;

    readonly float VALID_ANGLE = 45f; // 有效角度30度

    //--------------------------------------------
    float width_scale;
    float height_scale;
    float pre_x2;
    float pre_y2;
    float x2;
    float y2;
    float valid_x2;
    float valid_y2;

    //---------------------------------------------
    Vector2 mouse_pos;
    Vector3 cube_pos;
    public Vector2 x_axis;
    public Vector2 y_axis;
    public Vector2 z_axis;

    float projection_x;
    float projection_y;
    float projection_z;

    AxisMode axis_mode;
    Direction axis_direction;
    // 三軸的二維向量
    Dictionary<Direction, Vector2> orientation_map;
    // 三軸的三維向量
    Dictionary<Direction, Vector3> sphere_map;

    public Vector2 mouse_vector2;

    // Use this for initialization
    void Start() {
        width_scale = SCALE / (WIDTH_RIGHT - WIDTH_LEFT);
        height_scale = SCALE / (HEIGHT_UP - HEIGHT_DOWN);
        pre_x2 = 0f;
        pre_y2 = 0f;
        cube_pos = Vector3.zero;

       
        
        x2 = 0;
        y2 = 0;
    }

    // Update is called once per frame
    void Update() {
        // 坐標軸的模式
        axis_mode = (AxisMode)fitAxisMode();
        // 往哪個軸移動
        axis_direction = (Direction)direction();

        valid_x2 = Input.mousePosition.x / Screen.width;
        if (valid_x2 < WIDTH_LEFT)
        {
            x2 = MIN_X;
        }
        else if (valid_x2 > WIDTH_RIGHT)
        {
            x2 = MAX_X;
        }
        else
        {
            x2 = (valid_x2 - WIDTH_LEFT) * width_scale - SCALE / 2;
        }
        //---------------------------------------------------------
        valid_y2 = Input.mousePosition.y / Screen.height;
        if (valid_y2 < HEIGHT_DOWN)
        {
            y2 = MIN_Y;
        }
        else if (valid_y2 > HEIGHT_UP)
        {
            y2 = MAX_Y;
        }
        else
        {
            y2 = (valid_y2 - HEIGHT_DOWN) * height_scale - SCALE / 2;
        }



        mouse_vector2 = mouseVector2();
        //print(direction());
        x_axis = axisVector2(transform.right);
        y_axis = axisVector2(transform.up);
        z_axis = axisVector2(transform.forward);

        pre_x2 = x2;
        pre_y2 = y2;

        if (Input.GetKeyDown(KeyCode.M))
        {
            print(fitAxisMode());
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            print(string.Format("{0}", axis_direction));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Vector3 vector3 = new Vector3(projection_x, projection_y, projection_z);
            print(string.Format("projection: {0}", vector3));
        }
    }

    private void FixedUpdate()
    {
        // 三軸在螢幕上的二維向量
        orientation_map = new Dictionary<Direction, Vector2>()
        {
            // key軸的方向    value軸的向量
            {Direction.Pos_X, x_axis},
            {Direction.Pos_Y, y_axis},
            {Direction.Pos_Z, z_axis},
            {Direction.Neg_X, -x_axis},
            {Direction.Neg_Y, -y_axis},
            {Direction.Neg_Z, -z_axis}

            // orientation_map[Direction.Pos_X] = x_axis
        };

        sphere_map = new Dictionary<Direction, Vector3>()
        {
            // key軸的方向    value軸的向量
            {Direction.Pos_X, transform.right},
            {Direction.Pos_Y, transform.up},
            {Direction.Pos_Z, transform.forward},
            {Direction.Neg_X, -transform.right},
            {Direction.Neg_Y, -transform.up},
            {Direction.Neg_Z, -transform.forward}

            // orientation_map[Direction.Pos_X] = x_axis
        };
    }

    Vector2 mouseVector2()
    {
        return new Vector2(x2 - pre_x2, y2 - pre_y2);
    }

    // 三維向量轉螢幕上的二維向量
    Vector2 axisVector2(Vector3 axis_vector)
    {
        return new Vector2(axis_vector.x, axis_vector.y);
    }
    
    public Vector3 cubePos()
    {
        Vector2 mouse_pos = new Vector2(x2, y2);
        Vector3 axis = sphere_map[axis_direction];
        float pos = unit(axisVector2(axis), mouse_pos);
        // 回傳 cube_pos,初始值為 Vector3.zero
        // 根據 axis_direction 依序修改XYZ
        switch (axis_direction)
        {
            case Direction.Pos_X:
            case Direction.Neg_X:
                cube_pos.x = pos * axis.x;
                break;
            case Direction.Pos_Y:
            case Direction.Neg_Y:
                cube_pos.y = pos * axis.y;
                break;
            case Direction.Pos_Z:
            case Direction.Neg_Z:
                cube_pos.z = pos * axis.z;
                break;
        }

        return cube_pos;
    }
    //軸的向量跟滑鼠向量的內積
    int unit(Vector2 axis, Vector2 mouse) {
        float son = Vector2.Dot(axis, mouse);
        float mom = axis.magnitude;
        return Mathf.FloorToInt(son / mom);
    }

    // 決定坐標軸的模式,回傳給 axis_mode
    Enum fitAxisMode()
    {
        // 三軸在螢幕上的二維向量
        Vector2 x_axis = axisVector2(transform.right);
        Vector2 y_axis = axisVector2(transform.up);
        Vector2 z_axis = axisVector2(transform.forward);

        // 兩軸之間的夾角
        float xy = Vector2.Angle(x_axis, y_axis);
        float xz = Vector2.Angle(x_axis, z_axis);
        float yz = Vector2.Angle(y_axis, z_axis);


        // 這是xy軸太接近的情況
        if (xy < VALID_ANGLE || xy > 180 - VALID_ANGLE)
        {
            // x軸 比 y軸 長
            if (x_axis.sqrMagnitude >= y_axis.sqrMagnitude)
            {
                return AxisMode.XZ;
            }
            else
            {
                return AxisMode.YZ;
            }
        }

        // 這是xz軸太接近的情況
        if (xz < VALID_ANGLE || xz > 180 - VALID_ANGLE)
        {
            // x軸 比 z軸 長
            if (x_axis.sqrMagnitude >= z_axis.sqrMagnitude)
            {
                return AxisMode.XY;
            }
            else
            {
                return AxisMode.YZ;
            }
        }

        // 這是yz軸太接近的情況
        if (yz < VALID_ANGLE || yz > 180 - VALID_ANGLE)
        {
            // Y軸 比 Z軸 長
            if (y_axis.sqrMagnitude >= z_axis.sqrMagnitude)
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

    Enum orientation(params Direction[] directions)
    {
        // ex: directions = {Direction.Pos_X, Direction.Pos_Y, Direction.Neg_X, Direction.Neg_Y}
        // directions[2] = Direction.Neg_X
        int i, len = directions.Length;
        float[] angles = new float[len];
        // 滑鼠的向量跟某個軸的角度
        // orientation_map (KEY(directions[0])value())
        float min_angle = Vector2.Angle(mouse_vector2, orientation_map[directions[0]]);
        int min_index = 0;

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
