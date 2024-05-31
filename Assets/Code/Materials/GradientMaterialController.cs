using System;
using Code.Entities;

namespace Code.Infrastructure.Services.Materials
{
    [Serializable]
    public class GradientMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._GradBlend;
    }
}