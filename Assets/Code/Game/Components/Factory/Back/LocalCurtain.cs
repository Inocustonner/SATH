using System;
using Code.Data.Interfaces;
using Code.Game.Components;
using Code.Game.Components.Factory;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.CustomActions.Actions.Single
{
    public class LocalCurtain: MonoBehaviour, IGameInitListener, IPartStartListener, IPartTickListener, IRestarable
    {
        [Header("Services")] 
        private CameraService _cameraService;
        [Header("Components")]
        [SerializeField] private PushListener _pushListener;
        [SerializeField] private Rigidbody2D _player;
        [SerializeField] private SpriteRenderer _curtain;
        [Header("Static data")]
        [SerializeField] private Color _darkColor = new(0,0,0,0);
        [SerializeField] private Color _lightColor = new(1,1,1,0);
        [SerializeField] private float _showDuration, _hideDuration;
        [Header("Dynamic data")]
        [SerializeField] private bool _isDark;
        [SerializeField] private bool _isMax;

        public event Action OnSetMaxDark;
        public event Action OnSetMaxLight;

        public void GameInit()
        {
            _cameraService = Container.Instance.FindService<CameraService>();
        }

        public void PartStart()
        {
            _curtain.color = new Color(0, 0, 0, 0);
        }

        public void PartTick()
        {
            MoveCurtain();
           
            if (_isMax)
            {
                return;
            }
           
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

        private void MoveCurtain()
        {
            Vector2 camPos = _cameraService.CurrentCamera.transform.position;
            _curtain.transform.position = camPos;
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

            if (color.a >= 1)
            {
                if (_isDark)
                {
                    OnSetMaxDark?.Invoke();
                }
                else
                {
                    OnSetMaxLight?.Invoke();
                }

                _isMax = true;
            }
        }

        public void Restart()
        {
            _isMax = false;
        }
    }
}