using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "EffectsConfig", menuName = "Config/EffectsConfig")]
    public class EffectsConfig : ScriptableObject
    {
        public Material GlowMaterial;
    }
}