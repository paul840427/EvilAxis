using System;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class ExcelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveData(Vector3 pos, EFunction type)
    {
        //string path = Path.Combine(GameInfo.ApplicationPath, "StreamingAssets/data.csv");
        string path = Path.Combine(GameInfo.ApplicationPath, "StreamingAssets/data.csv");

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
        // JsonConvert.SerializeObject 將 record_data 轉換成json格式的字串
        writer.WriteLine(data);
        writer.Close();
        writer.Dispose();
    }
}
