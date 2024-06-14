using System;
using Code.Data.Enums;

namespace Code.Materials
{
    [Serializable]
    public class GradientMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._GradBlend;
    }
}