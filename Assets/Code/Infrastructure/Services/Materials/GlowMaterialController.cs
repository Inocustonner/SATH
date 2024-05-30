using Code.Data.Configs;
using Code.Entities;
using Code.Infrastructure.DI;
using Code.Infrastructure.GameLoop;

namespace Code.Infrastructure.Services.Materials
{
    public class GlowMaterialController: BaseMaterialController, IGameInitListener, IGameExitListener
    {
        public void GameInit()
        {
            _material = Container.Instance.FindConfig<EffectsConfig>().GlowMaterial;
            SetFloatValue(MaterialFloatValueType._Glow,0);    
        }

        public void GameExit()
        {
            SetFloatValue(MaterialFloatValueType._Glow,0);    
        }
    }
}