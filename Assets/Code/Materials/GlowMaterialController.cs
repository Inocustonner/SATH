using System;
using Code.Data.Enums;

namespace Code.Materials
{
    [Serializable]
    public class GlowMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._Glow;
    }
}