using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Header("Text params")]
        public float ReplicaDelaySeconds = 0.7f;
        public float DefaultTypingSpeed = 0.05f;
    }
}