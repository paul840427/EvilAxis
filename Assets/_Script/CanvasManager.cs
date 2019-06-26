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
    public Button init;
    public Transform sphere;
    Vector3 rotation_value;
    public SaveData sd;

    public Slider x_slider;
    public Slider y_slider;
    public Slider z_slider;

    string path;

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
            switch (GameInfo.Version)
            {
                case EVersion.Local:
                    string file_name = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
                    path = Path.Combine(GameInfo.DataPath, string.Format("{0}.png", file_name));
                    ScreenCapture.CaptureScreenshot(path);
                    print(string.Format("Save file: {0}", path));
                    break;
                case EVersion.WebGL:
                    StartCoroutine(sd.uploadScreenShot());
                    break;
                case EVersion.Test:
                    StartCoroutine(sd.uploadScreenShot());
                    break;
            }
        });

        init.onClick.AddListener(() =>
        {
            print("Initialize rotation angle.");            
            x_slider.value = 0f;
            y_slider.value = 0f;
            z_slider.value = 0f;
            sphere.rotation = Quaternion.identity;
        });
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
