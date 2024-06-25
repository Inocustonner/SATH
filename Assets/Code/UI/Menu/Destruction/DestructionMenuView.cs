using System;
using Code.UI.Base;
using TMPro;
using UnityEngine;

namespace Code.UI.Menu.Destruction
{
    public class DestructionMenuView: BaseMenuView
    {
        [SerializeField] private TextMeshProUGUI _playerHpText;
        
        public override void OpenMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public override void CloseMenu(Action onComplete = null)
        {
            windowTransform.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        public void SetPlayerHealth(string health)
        {
            _playerHpText.SetText(health);   
        }
    }
}