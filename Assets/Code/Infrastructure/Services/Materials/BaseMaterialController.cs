using System;
using Code.Entities;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Infrastructure.Services.Materials
{
    public abstract class BaseMaterialController: IService
    {
        
        protected Material _material;

        #region Base methods

        public bool GetStateValue(MaterialStateType materialStateType) => _material != null && _material.shader.isSupported &&
                                                          _material.IsKeywordEnabled(materialStateType.ToString());

        public void SetFloatValue(MaterialFloatValueType materialFloatValueType, float value)
        {
            if (_material != null && _material.HasProperty(materialFloatValueType.ToString()))
            {
                _material.SetFloat(materialFloatValueType.ToString(), value);
            }
            else
            {
                Debug.LogError("Материал не содержит свойство с именем " + materialFloatValueType);
            }
        }

        public float GetFloatValue(MaterialFloatValueType materialFloatValueType)
        {
            if (_material != null && _material.HasProperty(materialFloatValueType.ToString()))
            {
                return _material.GetFloat(materialFloatValueType.ToString());
            }
            else
            {
                Debug.LogError("Материал не содержит свойство с именем " + materialFloatValueType);
                return 0;
            }
        }

        public void SetState(MaterialStateType materialStateType, bool isActive)
        {
            if (_material != null && _material.shader.isSupported)
            {
                if (isActive && !_material.IsKeywordEnabled(materialStateType.ToString()))
                {
                    _material.EnableKeyword(materialStateType.ToString());
                }
                else
                {
                    _material.DisableKeyword(materialStateType.ToString());
                }
            }
            else
            {
                Debug.LogError("Материал не поддерживает директиву " + materialStateType);
            }
        }

        public void Clear()
        {
            foreach (var value in Enum.GetValues(typeof(MaterialStateType)))
            {
                SetState((MaterialStateType)value, false);
            }
        }

        #endregion
    }
}