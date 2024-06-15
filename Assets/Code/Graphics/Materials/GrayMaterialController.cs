using System;
using Code.Data.Enums;

namespace Code.Materials
{
    [Serializable]
    public class GrayMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._GreyscaleBlend;
    }
}