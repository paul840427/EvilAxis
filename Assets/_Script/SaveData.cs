using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
   
    public void saveClickProcess(Vector3 pos, EFunction type)
    {
        // 檢查資料夾是否存在，不存在則建立
        if (!Directory.Exists(GameInfo.DataPath))
        {
            //新增資料夾
            Directory.CreateDirectory(GameInfo.DataPath);
        }

        string path = Path.Combine(GameInfo.DataPath, "data.csv");

        // 檢查檔案是否存在，不存在則建立
        StreamWriter writer;
        if (!File.Exists(path))
        {
            writer = new FileInfo(path).CreateText();
            writer.WriteLine("Time, X, Y, Z}, Type");
        }
        else
        {
            writer = new FileInfo(path).AppendText();
        }

        // 時間格式化
        string time = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
        string data = string.Format("{0}, {1:F2}, {2:F2}, {3:F2}, {4}", time, pos.x, pos.y, pos.z, type);
        writer.WriteLine(data);
        writer.Close();
        writer.Dispose();
    }
}
