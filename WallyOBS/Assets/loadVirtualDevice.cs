using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadVirtualDevice : MonoBehaviour
{
    public string deviceName;
    public Renderer webCamCanvas;
    WebCamTexture _wct;
    public int height;
    public int width;
    public int FPS;


    // Use this for initialization
    void Start()
    {
        _wct = new WebCamTexture(deviceName, width, height, FPS);
        
        webCamCanvas.material.mainTexture = _wct;
        
        _wct.Play();
    }

}
