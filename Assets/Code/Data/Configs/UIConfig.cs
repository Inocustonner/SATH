using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        public float ReplicaDisableDelay = 0.7f;
    }
}