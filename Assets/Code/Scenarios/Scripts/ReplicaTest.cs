using UnityEngine;

namespace Code.Scenarios.Scripts
{
    public sealed class ReplicaTest : MonoBehaviour
    {
        [SerializeField]
        private ReplicaConfig config;
        private Replica _replica;

        [SerializeField]
        private int choiceIndex;

        private void Start()
        {
            this._replica = new Replica(this.config);
            this.PrintDialog();
        }

        [ContextMenu("Select Choice")]
        public void SelectChoice()
        {
            if (this._replica.MoveNext(this.choiceIndex))
            {
                this.PrintDialog();
            }
            else
            {
                Debug.Log("Dialog finished!");
            }
        }
        
        private void PrintDialog()
        {
            Debug.Log("----");
            Debug.Log($"Message: {this._replica.CurrentMessage}");

            foreach (var choice in _replica.CurrentConditions)
            {
                Debug.Log($"Choice: {choice}");
            }
            
            Debug.Log("----");
        }
    }
}