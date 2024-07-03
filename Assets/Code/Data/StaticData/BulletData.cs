using System;
using Code.Data.Enums;

namespace Code.Data.StaticData
{
    [Serializable]
    public struct BulletData
    {
        public BulletType Type;
        public float Speed;
        public int Damage;
    }
}