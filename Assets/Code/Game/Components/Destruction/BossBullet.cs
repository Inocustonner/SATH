using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BossBullet: MonoBehaviour, IPoolEntity
    {
        public int Damage { get;private set; }
        
        public void InitEntity(params object[] parameters)
        {
            Damage = (int)parameters[0];
        }

        public void EnableEntity()
        {
            throw new System.NotImplementedException();
        }

        public void DisableEntity()
        {
            throw new System.NotImplementedException();
        }

    }
}