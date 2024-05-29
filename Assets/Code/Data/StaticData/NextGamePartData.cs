using System;
using Code.Data.Enums;

namespace Code.Data.StaticData
{
    [Serializable]
    public struct NextGamePartData
    {
        public GameCondition Condition;
        public GamePartName GamePartName;
    }
}