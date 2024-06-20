using System;
using System.Collections;
using Code.Data.Configs;
using Code.Data.Interfaces;
using Code.Game.Components;
using Code.Game.Components.Factory;
using Code.Infrastructure.Cameras;
using Code.Infrastructure.DI;
using UnityEngine;

namespace Code.Game.CustomActions.Actions.Single
{
    public class LocalCurtain: MonoBehaviour, IGameInitListener, IPartStartListener, IPartTickListener, IPartExitListener,IRestarable
    {
        [Header("Services")] 
        private CameraService _cameraService;

        [Header("Components")] 
        [SerializeField] private CollisionObserver _whiteZone;
        [SerializeField] private PushListener _pushListener;
        [SerializeField] private Rigidbody2D _player;
        [SerializeField] private SpriteRenderer _curtain;
        [Header("Static data")]
        [SerializeField] private Color _darkColor = new(0,0,0,0);
        [SerializeField] private Color _lightColor = new(1,1,1,0);
        [SerializeField] private float  _showLightDuration = 0.05f, _hideDuration;
        private float _showDarkDuration;
        [Header("Dynamic data")]
        [SerializeField] private bool _isDark;
        [SerializeField] private bool _isMax;
        private Coroutine _whiteCoroutine;
        
        
        public event Action OnSetMaxDark;
        public event Action OnSetMaxLight;

        public void GameInit()
        {
            _cameraService = Container.Instance.FindService<CameraService>();
            _showDarkDuration = Container.Instance.FindConfig<FactoryConfig>().DarkCurtainSpeed;
        }

        public void PartStart()
        {
            _curtain.color = new Color(0, 0, 0, 0);
            SubscribeToEvents(true);
        }

        public void PartTick()
        {
            MoveCurtain();
           
            if (_isMax)
            {
                return;
            }
           
            RefreshCurtainState();
        }

        public void PartExit()
        {
            SubscribeToEvents(false);
        }

        private void SubscribeToEvents(bool flag)
        {
            if (flag)
            {
                _whiteZone.OnEnter += OnEnterWhiteZone;
            }
            else
            {
                
                _whiteZone.OnEnter -= OnEnterWhiteZone;
            }
        }

        private void OnEnterWhiteZone(GameObject obj)
        {
            if (_whiteCoroutine != null)
            {
                StopCoroutine(_whiteCoroutine);
            }

            _whiteCoroutine = StartCoroutine(RoutineShowWhiteCurtain());
        }

        private IEnumerator RoutineShowWhiteCurtain()
        {
            var color = _curtain.color;
            var period = new WaitForEndOfFrame();
            while (color.a < 1)
            {
                color.a += (_isDark ? _showDarkDuration : _showLightDuration) * Time.deltaTime;
                _curtain.color = color;

                if (color.a >= 1)
                {
                    _isMax = true;
                    OnSetMaxLight?.Invoke();
                }
                color = _curtain.color;
                yield return period;
            }
        }

        private void RefreshCurtainState()
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
            else if (_player.velocity.x < 0)
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
            color.a += (_isDark ? _showDarkDuration : _showLightDuration) * Time.deltaTime;
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