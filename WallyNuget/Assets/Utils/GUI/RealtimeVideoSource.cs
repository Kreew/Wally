using System;
using System.Collections.Generic;
using RtspClientSharp.RawFrames;
using RtspClientSharp.RawFrames.Video;
using RawFramesDecoding.DecodedFrames;
using RawFramesDecoding.FFmpeg;
using RawFramesReceiving;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace GUI
{
    class RealtimeVideoSource : IVideoSource, IDisposable
    {
        private IRawFramesSource _rawFramesSource;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _workTask = Task.CompletedTask;

        private readonly Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder> _videoDecodersMap =
            new Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder>();

        public event EventHandler<IDecodedVideoFrame> FrameReceived;
        
       



        public RealtimeVideoSource(IRawFramesSource rawFramesSource)
        {

            _rawFramesSource = rawFramesSource;
            Start();
        }




        private async Task Receive(CancellationToken token)
        {

            /*
            if (_rawFramesSource != null)
            {
                Debug.Log("il frame è nullo");
                _rawFramesSource.FrameReceived -= OnFrameReceived;
                DropAllVideoDecoders();
            }
            */

            if (_rawFramesSource == null)
            {
                return;
            }

            _rawFramesSource.FrameReceived += OnFrameReceived;


        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            CancellationToken token = _cancellationTokenSource.Token;
            _workTask = _workTask.ContinueWith(async p =>
            {
                await Receive(token);
            }, token);
        }


        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }


        public void Dispose()
        {
            DropAllVideoDecoders();
        }

        private void DropAllVideoDecoders()
        {
            foreach (FFmpegVideoDecoder decoder in _videoDecodersMap.Values)
                decoder.Dispose();

            _videoDecodersMap.Clear();
        }

        private void OnFrameReceived(object sender, RawFrame rawFrame)
        {
            
            
            if (!(rawFrame is RawVideoFrame rawVideoFrame))
                return;

            
            
            FFmpegVideoDecoder decoder = GetDecoderForFrame(rawVideoFrame);

            IDecodedVideoFrame decodedFrame = decoder.TryDecode(rawVideoFrame);

            if (decodedFrame != null)
            {
                FrameReceived?.Invoke(this, decodedFrame);
            }
        }


        private FFmpegVideoDecoder GetDecoderForFrame(RawVideoFrame videoFrame)
        {
            FFmpegVideoCodecId codecId = DetectCodecId(videoFrame);
            if (!_videoDecodersMap.TryGetValue(codecId, out FFmpegVideoDecoder decoder))
            {
                decoder = FFmpegVideoDecoder.CreateDecoder(codecId);
                _videoDecodersMap.Add(codecId, decoder);
            }

            return decoder;
        }

        private FFmpegVideoCodecId DetectCodecId(RawVideoFrame videoFrame)
        {
            if (videoFrame is RawJpegFrame)
                return FFmpegVideoCodecId.MJPEG;
            if (videoFrame is RawH264Frame)
                return FFmpegVideoCodecId.H264;

            throw new ArgumentOutOfRangeException(nameof(videoFrame));
        }
    }
}