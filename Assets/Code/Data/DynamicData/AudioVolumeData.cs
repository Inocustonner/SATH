using System;

namespace Code.Infrastructure.Audio.AudioSystem
{
    [Serializable]
    public class AudioVolumeData
    {
        public float Music;
        public float Effects;
        public bool IsEnabled;
    }
}