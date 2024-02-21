using System;
using RawFramesDecoding.DecodedFrames;

namespace GUI
{
    interface IAudioSource
    {
        event EventHandler<IDecodedAudioFrame> FrameReceived;
    }
}
