using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameInfo
{
    public static EVersion Version = EVersion.Local;

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

    // 數據儲存路徑(或許存本機端才會使用到，目前沒做版本間的切換)
    public static string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "EvilAxis");
}
