using System;
using System.Linq;
using Code.Replicas.Scripts;
using Core.Infrastructure.Utils;
using UnityEngine;

namespace Code.CustomActions.Actions
{
    public class ReplicaNumerableAction : ReplicaAction
    {
        [SerializeField] private int _currentID;
        [SerializeField] private Data[] _datas;

        [Serializable]
        private class Data
        {
            public int ID;
            public ReplicaConfig Replica;
        }

        public void SetID(int id)
        {
            _currentID = id;
            var data = _datas.FirstOrDefault(d => d.ID == _currentID);
            if (data != null)
            {
                _replicaConfig = data.Replica;
            }
            else
            {
                this.LogError($"{gameObject.name} has not data by {_currentID} ID");
            }
        }
    }
}