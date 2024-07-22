using Code.Data.StaticData;
using FMODUnity;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("Text params")]
        public float ReplicaDelaySeconds = 0.7f;
        public float DefaultTypingSpeed = 0.05f;
        public float ReplicaSkipDelaySeconds = 1;

        [Header("Text audio")] 
        public TextTypingAudioData[] TypingAudios;
        
        [Header("Game part transition curtain")] 
        public Color CurtainColor = Color.black;
        public float CurtainDuration = 1;

        [Header("Buttons")] 
        public EventReference ClickButtonAudio;
    }
}