using System;
using Code.Data.Enums;
using UnityEngine;

namespace Code.Data.Configs
{
    [CreateAssetMenu(fileName = "LocalizationElementsConfig", menuName = "Config/LocalizationElementsConfig")]
    public class LocalizationElementsConfig : ScriptableObject
    {
        public LocalizationElementData[] LocalizationElements;

        private void OnValidate()
        {
            /*var elements = Enum.GetValues(typeof(LocalizationElementType)).Cast<LocalizationElementType>().ToList();
            List<LocalizationElementData> localizationElementDatas = LocalizationElements.ToList();
            for (int i = 1; i < elements.Count; i++)
            {
                if (localizationElementDatas.Count < i)
                {
                    localizationElementDatas.Add(new LocalizationElementData());
                }

                localizationElementDatas[i].ElementType = elements[i];
            }*/
        }
    }

    [Serializable]
    public struct LocalizationElementData
    {
        public LocalizationElementType ElementType;
        public LanguageText[] Localization;
    }

    [Serializable]
    public struct LanguageText
    {
        public Lan Language;
        public string Text;
    }
}