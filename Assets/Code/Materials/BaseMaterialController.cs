using System;
using Code.Data.Enums;
using UnityEditor;
using UnityEngine;

namespace Code.Materials
{
    [Serializable, CanEditMultipleObjects]
    public abstract class BaseMaterialController
    {
        [SerializeField] protected Material _material;
        public abstract MaterialFloatValueType FloatValueType { get; }

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
                Debug.LogError($"Материал {_material?.name} не содержит свойство с именем {materialFloatValueType}");
            }
        }

        
        public void SetFloatValue( float value)
        {
            if (_material != null && _material.HasProperty(FloatValueType.ToString()))
            {
                _material.SetFloat(FloatValueType.ToString(), value);
            }
            else
            {
                Debug.LogError($"Материал {_material?.name} не содержит свойство с именем {FloatValueType}");
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
        
        public float GetFloatValue()
        {
            if (_material != null && _material.HasProperty(FloatValueType.ToString()))
            {
                return _material.GetFloat(FloatValueType.ToString());
            }
            else
            {
                Debug.LogError("Материал не содержит свойство с именем " + FloatValueType);
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