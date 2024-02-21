using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RtspClientSharp;
using RawFramesReceiving;
using System;
using GUI;
using RtspClientSharp.RawFrames;
using RawFramesDecoding.DecodedFrames;
using System.Buffers;
using RtspClientSharp.RawFrames.Video;
using System.Threading;
using System.Threading.Tasks;

public class GetVideoFrames : MonoBehaviour
{


    private CancellationTokenSource _cancellationTokenSource;
    private Task _workTask = Task.CompletedTask;


    public GameObject objectToRenderize;
    public string URL;
    


    private RawFramesSource _rawFramesSource;
    private RealtimeVideoSource _realtimeVideoSource;

    private IntPtr intPtr;
    private int pointer;

    private Texture2D text;

    void Start()
    {


        ConnectionParameters connectionParameters = new ConnectionParameters(new Uri(URL))
        {
            RtpTransport = RtpTransportProtocol.UDP
        };

        if (_rawFramesSource != null)
            return;

        _rawFramesSource = new RawFramesSource(connectionParameters);
        _realtimeVideoSource = new RealtimeVideoSource(_rawFramesSource);


        //_rawFramesSource.ConnectionStatusChanged += ConnectionStatusChanged;


        //_realtimeAudioSource.SetRawFramesSource(_rawFramesSource);



       

        _rawFramesSource.Start();
        StartDecoding();

        


    }

    // Update is called once per frame
    void Update() {

        //
        //_realtimeVideoSource.SetRawFramesSource(_rawFramesSource);
        //_realtimeVideoSource.FrameReceived += onDecode;

        
        //text.LoadRawTextureData(intPtr, pointer);
        //text.Apply();

        if (text != null)
        {
           objectToRenderize.gameObject.GetComponent<Renderer>().material.mainTexture = text;
        }


    }



    private void OnApplicationQuit()
    {
        _rawFramesSource.Stop();
        _cancellationTokenSource.Cancel();
    }




    private async Task Receive(CancellationToken token)
    {
        //_realtimeVideoSource.FrameReceived += OnStamp;
        _realtimeVideoSource.FrameReceived += onDecode;

    }

    public void StartDecoding()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        CancellationToken token = _cancellationTokenSource.Token;
        _workTask = _workTask.ContinueWith(async p =>
        {
            await Receive(token);
        }, token);
    }

   



    public void onDecode(object sender, IDecodedVideoFrame decoded)
    {

        //Debug.Log("ere");

        
        DecodedVideoFrame decodedFrame = (DecodedVideoFrame)decoded;
        Debug.Log(decoded);
    }



    // IntPtr buffer, int bufferStride, TransformParameters transformParameters
    /*
    public void onDecode(object sender, IDecodedVideoFrame transformAction)
    {
        IntPtr buffer = ;
        int bufferStride;
        TransformParameters transform;

        transformAction.TransformTo(buffer, bufferStride, transform);
    }
    */
    /*
    public void OnDecode(object sender, RawFrame rawVideoFrame)
    {
        Debug.Log(rawVideoFrame is RawH264Frame rawH264Frame);

        if (rawVideoFrame is RawH264IFrame)
        {
            byte[] bytes = rawVideoFrame.FrameSegment.Array;

            Texture2D text = new Texture2D(1920, 1080, TextureFormat.RGBA32, false, false);




            text.LoadRawTextureData(bytes);
            text.Apply();
            Debug.LogFormat("Loaded image [{0},{1}]", text.width, text.height);

            if (text.width == 2)
            {
                return;
            }

            objectToRenderize.gameObject.GetComponent<Renderer>().material.mainTexture = text;
        }
    }*/

    /*
    public void loadRawFrame(RawFrame frame)
    {

        if (frame is RawH264Frame)
        {
            byte[] bytes = frame.FrameSegment.Array;
            if(bytes.Length <= 0)
            {
                return;
            }



            Debug.Log(bytes.Length);
            Texture2D text = new Texture2D(640, 480, TextureFormat.RGB24, false, false);
            text.LoadRawTextureData(bytes);
            text.Apply();

            if (text.width == 2)
            {
                return;
            }

            objectToRenderize.gameObject.GetComponent<Renderer>().material.mainTexture = text;


        }
    }
    */
}

