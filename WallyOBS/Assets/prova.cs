using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prova : MonoBehaviour
{
    public string deviceName;

    public Renderer webCamCanvas;
    WebCamTexture _wct;
   
    WebCamDevice[] _devices;


    // Use this for initialization
    void Start()
    {
        _devices = WebCamTexture.devices;

        //deviceName = _devices[0].name;
        _wct = new WebCamTexture("OBS Virtual Camera", 1920, 9600, 30);


        webCamCanvas.material.mainTexture = _wct;


        // CheckDeviceNames();
        _wct.Play();
        /*
        if (_devices.Length > 0)
        {
            Debug.Log("Number webcams is " + _devices.Length);
            foreach (WebCamDevice wcd in _devices)
            {
                Debug.Log(wcd.name);
            }
        }*/
    }



    // For photo varibles

    public Texture2D heightmap;
    public Vector3 size = new Vector3(100, 10, 100);


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            TakeSnapshot();

    }

    // For saving to the _savepath
    private string _SavePath = "C:/WebcamSnaps/"; //Change the path here!
    int _CaptureCounter = 0;
    void TakeSnapshot()
    {
        Texture2D snap = new Texture2D(_wct.width, _wct.height);
        snap.SetPixels(_wct.GetPixels());
        snap.Apply();
        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        ++_CaptureCounter;
    }
}
