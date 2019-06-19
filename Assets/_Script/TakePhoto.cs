using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TakePhoto : MonoBehaviour
{
    string url;
    string guid;
    string get_request;

    // Start is called before the first frame update
    void Start()
    {
        get_request = "";
        //guid = string.Format("{0}", Guid.NewGuid());
        guid = "9527";
        url = string.Format("http://140.122.91.200/GetFilesName.php?guid={0}", guid);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(screenShot());
        }
    }


    public IEnumerator screenShot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        // texture to bytes
        byte[] bytes = texture.EncodeToPNG();

        url = "http://140.122.91.200/ScreenShot.php";        

        WWWForm form = new WWWForm();
        form.AddField("guid", guid);
        string time = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
        form.AddField("time", time);
        form.AddBinaryData("file", bytes);

        StartCoroutine(Upload(url, form));
    }

    public void clickProcess(Vector3 pos, EFunction type)
    {
        string url = "140.122.91.200/ScreenShot.php";

        WWWForm form = new WWWForm();
        string time = DateTime.Now.ToString("yyyy-MM-dd@H-mm-ss-ffff");
        form.AddField("time", time);
        form.AddField("time", time);
        form.AddField("time", time);
        form.AddField("time", time);

        StartCoroutine(Upload(url, form));
    }

    IEnumerator Upload(string url, WWWForm form)
    {
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

    IEnumerator downloadImage(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError)
        {
            print(webRequest.error);
            yield break;
        }
        else
        {
            get_request = webRequest.downloadHandler.text;

            string[] files_name = get_request.Split('#');
            int i, len = files_name.Length - 1;
            for (i = 0; i < len; i++) {                
                url = string.Format("http://140.122.91.200/DownloadImage.php?guid={0}&image={1}", guid, files_name[i]);
                print(url);
                yield return StartCoroutine(GetRequest(url));
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                print(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                print(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
