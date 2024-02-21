using System;

//using UnityEngine;

namespace RawFramesDecoding.DecodedFrames
{
    class DecodedVideoFrame : IDecodedVideoFrame
    {
        private readonly Action<IntPtr, int, TransformParameters> _transformAction;


        public IntPtr intPtr;
        public int pointer = 11;
        public TransformParameters parameters;


        

        public DecodedVideoFrame(Action<IntPtr, int, TransformParameters> transformAction)
        {

            _transformAction = transformAction;

            
            transformAction = (data, point, transform) => { intPtr = data; pointer = point; parameters = transform;  };
            
        }

        public void TransformTo(IntPtr buffer, int bufferStride, TransformParameters transformParameters)
        {
            _transformAction(buffer, bufferStride, transformParameters);
        }

        
    }
}