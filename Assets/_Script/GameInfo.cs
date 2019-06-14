using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    // 方塊移動邊界
    public static readonly float LEFT = 0.5102f;
    public static readonly float RIGHT = 0.8163f;
    public static readonly float UP = 0.8f;
    public static readonly float DOWN = 0.1f;

    // 方塊移動區域邊長
    public static readonly float SCALE = 10f;

    // 方塊移動區域座標
    public static readonly float MIN_X = -5f;
    public static readonly float MAX_X = 5f;
    public static readonly float MIN_Y = -5f;
    public static readonly float MAX_Y = 5f;

    // mouse in vaild area
    public static bool InVaildArea = false;

    // 有效角度30度
    public static readonly float VALID_ANGLE = 40f;

    // function mode
    public static EFunction FunctionMode = EFunction.Add;

    public static string ScreenCapturePath = "Assets/Image/";
}
