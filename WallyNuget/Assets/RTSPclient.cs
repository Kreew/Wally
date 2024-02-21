using UnityEngine;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RtspClientSharp;
using RtspClientSharp.Rtsp;
using RtspClientSharp.RawFrames;

public class RTSPclient : MonoBehaviour
{
    public string url;
    public string userName;
    public string password;
    private RtspClient rtspClient;
    private Task connectTask;
    private MeshRenderer renderTarget;
    public EventHandler<RawFrame> FrameReceived { get; set; }
    public EventHandler<string> ConnectionStatusChanged { get; set; }

    void Start()
    {
        var serverUri = new Uri(url);
        var credentials = new NetworkCredential(userName, password);

        renderTarget = this.GetComponent<MeshRenderer>();

        var connectionParameters = new ConnectionParameters(serverUri, credentials)
        {
            RtpTransport = RtpTransportProtocol.UDP
        };

        var cancellationTokenSource = new CancellationTokenSource();

        connectTask = ConnectAsync(connectionParameters, cancellationTokenSource.Token);

    }

    // Update is called once per frame
    private void Update()
    {
    }

    private  async Task ConnectAsync(ConnectionParameters connectionParameters, CancellationToken token)
    {
        try
        {
            TimeSpan delay = TimeSpan.FromSeconds(5);

            using (var rtspClient = new RtspClient(connectionParameters))
            {
                rtspClient.FrameReceived += (sender, frame) => Debug.Log($"New frame {frame.Timestamp}: {frame.GetType().Name} : {frame.FrameSegment}"); ; 

                while (true)
                {
                    Debug.Log("Connecting...");

                    try
                    {
                        await rtspClient.ConnectAsync(token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                        Debug.Log("failed");
                    }
                    catch (RtspClientException e)
                    {
                        Debug.Log(e.ToString());
                        await Task.Delay(delay, token);
                        continue;
                    }

                    Debug.Log("Connected.");

                    try
                    {
                        await rtspClient.ReceiveAsync(token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (RtspClientException e)
                    {
                        Debug.Log(e.ToString());
                        await Task.Delay(delay, token);
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }


}



