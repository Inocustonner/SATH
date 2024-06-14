using Code.Data.Interfaces;
using Code.Game.CustomActions.Actions.Single;
using Code.Utils;
using UnityEngine;

namespace Code.Game.Components.Factory.Back
{
    public class FactoryPartToggle: MonoBehaviour, IPartStartListener, IPartExitListener
    {
        [SerializeField] private LocalCurtain _localCurtain;
        [SerializeField] private GameObject _startPart,_lightPart, _darkPart;

        public void PartStart()
        {
            SubscribeToEvents(true);
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _localCurtain.OnSetMaxDark += OnMaxDark;
                _localCurtain.OnSetMaxLight += OnMaxLight;
            }
            else
            {
                _localCurtain.OnSetMaxDark -= OnMaxDark;
                _localCurtain.OnSetMaxLight -= OnMaxLight;
            }
        }

        private void OnMaxLight()
        {
            this.Log("Set max light");
            _startPart.SetActive(false);
            _lightPart.SetActive(true);
        }

        private void OnMaxDark()
        {
            this.Log("Set max dark");
            _startPart.SetActive(false);
            _darkPart.SetActive(true);
        }
    }
}