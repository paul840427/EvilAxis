using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotoTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

        }
    }

    Texture2D CaptureScreenshot(Rect rect)
    {
        Texture2D shot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        shot.ReadPixels(rect, 0, 0);
        shot.Apply();

        byte[] bytes = shot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        return shot;
    }
}
