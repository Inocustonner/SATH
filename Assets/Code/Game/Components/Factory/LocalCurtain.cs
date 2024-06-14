using Code.Data.Interfaces;
using Code.Game.Components.Factory;
using UnityEngine;

namespace Code.Game.CustomActions.Actions.Single
{
    public class LocalCurtain: MonoBehaviour,IPartStartListener, IPartTickListener, IPartExitListener
    {
        [Header("Services")] 
        [Header("Components")]
        [SerializeField] private PushListener _pushListener;
        [SerializeField] private Rigidbody2D _player;
        [SerializeField] private SpriteRenderer _curtain;
        [Header("Static data")]
        [SerializeField] private Color _darkColor = new Color(0,0,0,0);
        [SerializeField] private Color _lightColor = new Color(1,1,1,0);
        [SerializeField] private float _showDuration, _hideDuration;
        [Header("Dynamic data")]
        [SerializeField] private bool _isDark;

        

        public void PartStart()
        {
            SubscribeToEvents(true);
            _curtain.color = new Color(0, 0, 0, 0);
        }

        public void PartTick()
        {
            if (_pushListener.IsPushed)
            {
                if (!_isDark)
                {
                    if (_curtain.color.a > 0)
                    {
                        Hide();
                    }
                    else
                    {
                        _curtain.color = _darkColor;
                        _isDark = true;
                    }
                }
                else
                {
                    Show();
                }
            }
            else if(_player.velocity.x < 0)
            {
                if (_isDark)
                {
                    if (_curtain.color.a > 0)
                    {
                        Hide();
                    }
                    else
                    {
                        _curtain.color = _lightColor;
                        _isDark = false;
                    }
                }
                else
                {
                    Show();
                }
            }
            else if (_curtain.color.a > 0)
            {
                Hide();
            }
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _pushListener.OnStartPush += OnStartPushStone;
                _pushListener.OnStopPush += OnEndPushStone;
            }
            else
            {
                _pushListener.OnStartPush -= OnStartPushStone;
                _pushListener.OnStopPush -= OnEndPushStone;
            }
        }

        private void OnEndPushStone()
        {
            
        }

        private void OnStartPushStone()
        {
            
        }

        private void Hide()
        {
            var color = _curtain.color;
            color.a -= _hideDuration * Time.deltaTime;
            _curtain.color = color;
        }

        private void Show()
        {
            var color = _curtain.color;
            color.a += _showDuration * Time.deltaTime;
            _curtain.color = color;
        }
    }
}