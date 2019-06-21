using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class SaveData : MonoBehaviour
{
    private string server;
    private string url;
    string guid;
    string get_request;

    WWWForm form;

    private void Start()
    {
        server = "http://140.122.91.200";
        guid = Guid.NewGuid().ToString();
        get_request = "";
    }

    void Update()
    {
        if(GameInfo.Version == EVersion.Test)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(screenShot());
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                StartCoroutine(uploadScreenShot());
            }
        }
    }

    #region local version
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
        string time = DateTime.Now.ToString("yyyy-MM-dd@HH-mm-ss-ffff");
        string data = string.Format("{0}, {1:F2}, {2:F2}, {3:F2}, {4}", time, pos.x, pos.y, pos.z, type);
        writer.WriteLine(data);
        writer.Close();
        writer.Dispose();
    }
    #endregion

    #region webGL version
    public IEnumerator uploadScreenShot()
    {
        print("start uploadScreenShot");
        yield return new WaitForEndOfFrame();
        // Capture Screenshot
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        // texture to bytes
        byte[] bytes = texture.EncodeToPNG();

        // upload image
        yield return StartCoroutine(uploadImage(bytes));
        print("end screenShot");
    }

    public IEnumerator screenShot()
    {
        print("start screenShot");
        yield return new WaitForEndOfFrame();
        // Capture Screenshot
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        // texture to bytes
        byte[] bytes = texture.EncodeToPNG();

        // upload image
        yield return StartCoroutine(uploadImage(bytes));

        // wait for get_request
        yield return StartCoroutine(getFilesName());

        // download image
        yield return StartCoroutine(downloadImage());

        // delete dir in server
        yield return StartCoroutine(deleteDir());

        print("end screenShot");
    }

    IEnumerator uploadImage(byte[] bytes)
    {
        url = string.Format("{0}/{1}", server, "ScreenShot.php");
        form = new WWWForm();
        form.AddField("guid", guid);
        string time = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
        form.AddField("time", time);
        form.AddBinaryData("file", bytes);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
            }
            else
            {
                get_request = www.downloadHandler.text;
                print("Form upload complete!");
                print(get_request);
            }
        }
    }

    IEnumerator getFilesName()
    {
        print("start GetFilesName");
        url = string.Format("{0}/{1}", server, "GetFilesName.php"); ;
        form = new WWWForm();
        form.AddField("guid", guid);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
            }
            else
            {
                get_request = www.downloadHandler.text;
                print("getFilesName complete!");
                print(get_request);
            }
        }

        print("end GetFilesName");
    }

    //public IEnumerator DownloadFile()
    //{
    //    UnityWebRequest uwr;
    //    string dir = string.Format("{0}/image/{1}", server, guid);
    //    string[] files_name = get_request.Split('#');
    //    int i, len = files_name.Length - 1;
    //    string file_path;

    //    for (i = 0; i < len; i++)
    //    {
    //        print("file_name: " + files_name[i]);
    //        url = string.Format("{0}/{1}", dir, files_name[i]);
    //        uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);


    //        file_path = Path.Combine(GameInfo.DataPath, files_name[i]);
    //        uwr.downloadHandler = new DownloadHandlerFile(file_path);
    //        yield return uwr.SendWebRequest();

    //        if (uwr.isNetworkError || uwr.isHttpError)
    //        {
    //            print(uwr.error);
    //        }
    //        else
    //        {
    //            print("File successfully downloaded and saved to " + file_path);
    //        }
                
    //    }
    //}

    IEnumerator downloadImage()
    {
        print("start downloadImage");
        url = string.Format("{0}/{1}", server, "DownloadImage2.php"); ;

        string[] files_name = get_request.Split('#');
        int i, len = files_name.Length - 1;
        string file_name;

        for (i = 0; i < len; i++) {
            file_name = files_name[i];

            form = new WWWForm();
            form.AddField("guid", guid);
            form.AddField("file_name", file_name);

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    print(www.error);
                }
                else
                {
                    print(string.Format("download {0} success.", file_name));
                }
            }
        }

        print("end downloadImage");
    }

    IEnumerator deleteDir()
    {
        url = string.Format("{0}/{1}", server, "DeleteDir.php");
        form = new WWWForm();
        form.AddField("guid", guid);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                print("isNetworkError: " + www.error);
            }else if (www.isHttpError)
            {
                print("isHttpError: " + www.error);
            }
            else
            {
                get_request = www.downloadHandler.text;
                print(get_request);
            }
        }
    }
    #endregion
}
