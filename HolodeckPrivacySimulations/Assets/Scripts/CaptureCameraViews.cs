using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CaptureCameraViews : MonoBehaviour
{
    public RenderTexture userView, sameView, unauthView;
    public string imageFilePrefix;
    private string imageDir = "/ImageOutputs/";

    // Start is called before the first frame update
    void Start()
    {
        // empty start
    }

    // Update is called once per frame
    void Update()
    {
        CaptureViews();
    }

    void CaptureViews()
    {
        CaptureView(userView, imageFilePrefix + "_user_view");
        CaptureView(sameView, imageFilePrefix + "_same_view");
        CaptureView(unauthView, imageFilePrefix + "_unauth_view");
    }

    void CaptureView(RenderTexture view, string filename)
    {
        Texture2D tex2d = new Texture2D(view.width, view.height, TextureFormat.RGB24, false);

        RenderTexture.active = view;
        tex2d.ReadPixels(new Rect(0, 0, view.width, view.height), 0, 0);
        tex2d.Apply();

        var Bytes = tex2d.EncodeToJPG();
        string filepath = Application.dataPath + imageDir + filename + ".jpg";
        File.WriteAllBytes(filepath, Bytes);
    }
}
