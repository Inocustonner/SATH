using System;
using Code.Data.Enums;
using FMODUnity;

namespace Code.Data.DynamicData
{
    [Serializable]
    public struct AcceleratedTextData
    {
        public string Text;
        public float Speed;
        public TextTypingAudioType TextTypingAudioType;
    }
}