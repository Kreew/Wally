using System;
using RawFramesDecoding.DecodedFrames;

namespace GUI
{
    public interface IVideoSource
    {
        event EventHandler<IDecodedVideoFrame> FrameReceived;
    }
}