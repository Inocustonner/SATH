using System;
using Code.Data.Enums;
using FMODUnity;

namespace Code.Data.StaticData
{
    [Serializable]
    public struct TextTypingAudioData
    {
        public TextTypingAudioType Type;
        public EventReference Audio;
    }
}