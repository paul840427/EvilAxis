using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintScreen : MonoBehaviour {
    public int image_nub = 0;
    public void OnMouseDown()
    {
        image_nub ++; 
        ScreenCapture.CaptureScreenshot("Assets/Image/image" + image_nub +".jpg");
        Debug.Log("已儲存圖檔");
    }
}
