using System;
using Code.Data.Enums;

namespace Code.Materials
{
    [Serializable]
    public class ContrastMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._Contrast;
    }
}