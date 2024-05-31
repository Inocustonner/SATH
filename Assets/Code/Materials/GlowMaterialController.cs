using System;
using Code.Entities;

namespace Code.Infrastructure.Services.Materials
{
    [Serializable]
    public class GlowMaterialController: BaseMaterialController
    {
        public override MaterialFloatValueType FloatValueType => MaterialFloatValueType._Glow;
    }
}