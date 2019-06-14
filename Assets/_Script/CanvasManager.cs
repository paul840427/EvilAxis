using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Button add;
    public Button del;
    public Button save;
    public Transform sphere;
    Vector3 rotation_value;

    string path1;
    string path2;

    // Start is called before the first frame update
    void Start()
    {
        rotation_value = Vector3.zero;

        add.onClick.AddListener(() =>
        {
            GameInfo.FunctionMode = EFunction.Add;
            print(string.Format("FunctionMode: {0}", GameInfo.FunctionMode));            
        });

        del.onClick.AddListener(() =>
        {
            GameInfo.FunctionMode = EFunction.Del;
            print(string.Format("FunctionMode: {0}", GameInfo.FunctionMode));            
        });

        save.onClick.AddListener(() =>
        {
            print(string.Format("FunctionMode: {0}", "Save"));
            string file_name = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
            path1 = Path.Combine(GameInfo.ScreenCapturePath, string.Format("{0}.png", file_name)); //存本機
            path2 = Path.Combine(GameInfo.ScreenCapturePath2, string.Format("{0}.png", file_name));

            try
            {
                ScreenCapture.CaptureScreenshot(path1);
            }
            catch (Exception e)
            {
                print(e.Message);
            }

            try
            {
                ScreenCapture.CaptureScreenshot(path2);
            }
            catch (Exception e)
            {
                print(e.Message);
            }

        });
    }
    private void OnGUI()
    {
        GUI.color = Color.red;
        GUI.skin.label.fontSize = 50;
        GUILayout.Label(path1 + "\n" + path2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onSliderXValueChanged(float value)
    {
        float x = value - rotation_value.x;
        sphere.Rotate(new Vector3(x, 0f, 0f));
        rotation_value.x = value;
    }

    public void onSliderYValueChanged(float value)
    {
        float y = value - rotation_value.y;
        sphere.Rotate(new Vector3(0f, y, 0f));
        rotation_value.y = value;
    }

    public void onSliderZValueChanged(float value)
    {
        float z = value - rotation_value.z;
        sphere.Rotate(new Vector3(0f, 0f, z));
        rotation_value.z = value;
    }

}
