﻿using UnityEngine;
using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using RtspClientSharp;
using RtspClientSharp.RawFrames;
using RtspClientSharp.Rtsp;
using System.Collections.Generic;

namespace RawFramesReceiving
{
    class RawFramesSource : IRawFramesSource
    {
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(5);
        private readonly ConnectionParameters _connectionParameters;
        private Task _workTask = Task.CompletedTask;
        private CancellationTokenSource _cancellationTokenSource;

        public EventHandler<RawFrame> FrameReceived { get; set; }
        public EventHandler<string> ConnectionStatusChanged { get; set; }
        //public RawFrame actualFrame { get; set; }

        public RawFramesSource(ConnectionParameters connectionParameters)
        {
             _connectionParameters =
                 connectionParameters ?? throw new ArgumentNullException(nameof(connectionParameters));

            _connectionParameters = connectionParameters;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            CancellationToken token = _cancellationTokenSource.Token;
            _workTask = _workTask.ContinueWith(async p =>
            {
                await ReceiveAsync(token);
            }, token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private async Task ReceiveAsync(CancellationToken token)
        {
            try
            {
                using (var rtspClient = new RtspClient(_connectionParameters))
                {
                    rtspClient.FrameReceived += RtspClientOnFrameReceived; 

                    //rtspClient.FrameReceived += (sender, frame) => Debug.Log(frame);


                    while (true)
                    {
                        Debug.Log("Connecting...");

                        try
                        {
                            await rtspClient.ConnectAsync(token);
                        }
                        catch (InvalidCredentialException)
                        {
                            Debug.Log("Invalid login and/or password");
                            await Task.Delay(RetryDelay, token);
                            continue;
                        }
                        catch (RtspClientException e)
                        {
                            Debug.Log(e.ToString());
                            await Task.Delay(RetryDelay, token);
                            continue;
                        }

                        OnStatusChanged("Receiving frames...");
                        Debug.Log("Receiving frames..."); 

                        try
                        {
                            await rtspClient.ReceiveAsync(token);
                        }
                        catch (RtspClientException e)
                        {
                            OnStatusChanged(e.ToString());
                            Debug.Log(e.ToString());
                            await Task.Delay(RetryDelay, token);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void RtspClientOnFrameReceived(object sender, RawFrame rawFrame)
        {
            Debug.Log("frame ricevuto");
            FrameReceived?.Invoke(this, rawFrame);
        }

        private void OnStatusChanged(string status)
        {
            ConnectionStatusChanged?.Invoke(this, status);
        }

       
    }
}