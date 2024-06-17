using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Data.DynamicData;
using Code.Data.Interfaces;
using Code.Data.StaticData;
using Code.Infrastructure.DI;
using Code.Infrastructure.Save;
using Code.Infrastructure.Services;
using Code.Utils;
using UnityEngine;

namespace Code.Infrastructure.ScreenResolution
{
    public class ResolutionService : IService, IGameInitListener, IProgressWriter
    {
        public const bool FixedAspectRatio = true;
        public const float TargetAspectRatio = 16 / 9f;
        private const float WindowedAspectRatio = 16f / 9f;

        // List of horizontal resolutions to include
        private readonly int[] _resolutions = { 854, 1280, 1400, 1600, 1920, 2560,3840, 7680 };

        public List<Vector2> WindowedResolutions;
        public List<Vector2> FullscreenResolutions;

        private int _currWindowedRes, _currFullscreenRes;

        private CoroutineRunner _coroutineRunner;
        private Coroutine _coroutine;
    
        public event Action OnInitResolutions;

        public GraphicData GraphicData { get; private set; } = new GraphicData();

        public void GameInit()
        {
            _coroutineRunner = Container.Instance.FindService<CoroutineRunner>();
            _coroutine = _coroutineRunner.StartRoutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            this.Log("Start routine",Color.yellow);
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                GraphicData.DisplayResolution = Screen.currentResolution;
            }
            else
            {
                if (Screen.fullScreen)
                {
                    Resolution r = Screen.currentResolution;
                    Screen.fullScreen = false;

                    yield return null;
                    yield return null;

                    GraphicData.DisplayResolution = Screen.currentResolution;

                    Screen.SetResolution(r.width, r.height, true);

                    yield return null;
                }
                else
                {
                    GraphicData.DisplayResolution = Screen.currentResolution;
                }
            }

            InitResolutions();
        }

        private void InitResolutions()
        {
            this.Log("Init resolutions",Color.yellow);

            GraphicData.DisplayResolution = Screen.currentResolution;
            var screenAspect = (float)GraphicData.DisplayResolution.width / GraphicData.DisplayResolution.height;

            WindowedResolutions = new List<Vector2>();
            FullscreenResolutions = new List<Vector2>();

            foreach (var width in _resolutions)
            {
                if (width <= GraphicData.DisplayResolution.width)
                {
                    Vector2 windowedResolution = new Vector2(width, Mathf.Round(width / (FixedAspectRatio ? TargetAspectRatio : WindowedAspectRatio)));
                    WindowedResolutions.Add(windowedResolution);
                    FullscreenResolutions.Add(new Vector2(width, Mathf.Round(width / screenAspect)));
                }
            }

            // Adding fullscreen native resolution
            FullscreenResolutions.Add(new Vector2(GraphicData.DisplayResolution.width, GraphicData.DisplayResolution.height));

            // Adding half fullscreen native resolution
            Vector2 halfNative = new Vector2(GraphicData.DisplayResolution.width * 0.5f, GraphicData.DisplayResolution.height * 0.5f);
            if (halfNative.x > _resolutions[0] && FullscreenResolutions.IndexOf(halfNative) == -1)
                FullscreenResolutions.Add(halfNative);

            FullscreenResolutions = FullscreenResolutions.OrderBy(resolution => resolution.x).ToList();

            bool found = false;

            GraphicData.IsFullScreen = Screen.fullScreen; 
            if (GraphicData.IsFullScreen)
            {
                _currWindowedRes = WindowedResolutions.Count - 1;

                for (int i = 0; i < FullscreenResolutions.Count; i++)
                {
                    if (FullscreenResolutions[i].x == Screen.width && FullscreenResolutions[i].y == Screen.height)
                    {
                        _currFullscreenRes = i;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    SetResolution(FullscreenResolutions.Count - 1, true);
                }
            }
            else
            {
                _currFullscreenRes = FullscreenResolutions.Count - 1;

                for (int i = 0; i < WindowedResolutions.Count; i++)
                {
                    if (WindowedResolutions[i].x == Screen.width && WindowedResolutions[i].y == Screen.height)
                    {
                        found = true;
                        _currWindowedRes = i;
                        break;
                    }
                }

                if (!found)
                {
                    SetResolution(WindowedResolutions.Count - 1, false);
                }
            }
            OnInitResolutions?.Invoke();
            this.Log("Init resoluttions");
        }

        public void SetResolution(int index, bool fullscreen)
        {
            this.Log($"Set rosolution {index} {fullscreen}",Color.yellow);

            if (GraphicData.DisplayResolution.width != Screen.currentResolution.width)
            {
                this.Log($"New init resolutions",Color.yellow);
                InitResolutions();
                return;
            }
        
            GraphicData.IsFullScreen = fullscreen;
        
            Vector2 resolution = new Vector2();
            if (fullscreen)
            {
                _currFullscreenRes = index;
                resolution = FullscreenResolutions[^1];
            }
            else
            {
                GraphicData.Index = index;
                _currWindowedRes = index;
                resolution = WindowedResolutions[_currWindowedRes];
            }

            bool fullscreen2windowed = Screen.fullScreen & !fullscreen;

            Debug.Log("Setting resolution to " + (int)resolution.x + "x" + (int)resolution.y);
   
            Screen.SetResolution((int)resolution.x, (int)resolution.y, fullscreen);
        
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                _coroutineRunner.StopRoutine(_coroutine);

                if (fullscreen2windowed)
                {
                    _coroutine = _coroutineRunner.StartRoutine(SetResolutionAfterResize(resolution));
                }
            }
        }

        public void SetFullscreen(bool isFullScreen)
        {
            if (GraphicData.IsFullScreen == isFullScreen)
            {
                return;
            }

            GraphicData.IsFullScreen = isFullScreen;
            SetResolution(Screen.fullScreen ? _currWindowedRes : _currFullscreenRes, isFullScreen);
        }

        private IEnumerator SetResolutionAfterResize(Vector2 r)
        {
            int maxTime = 5; // Max wait for the end of the resize transition
            float time = Time.time;

            // Skipping a couple of frames during which the screen size will change
            yield return null;
            yield return null;

            int lastW = Screen.width;
            int lastH = Screen.height;

            // Waiting for another screen size change at the end of the transition animation
            while (Time.time - time < maxTime)
            {
                if (lastW != Screen.width || lastH != Screen.height)
                {
                    Debug.Log("Resize! " + Screen.width + "x" + Screen.height);

                    Screen.SetResolution((int)r.x, (int)r.y, Screen.fullScreen);
                    yield break;
                }

                yield return null;
            }

            Debug.Log("End waiting");
        }

        public void LoadProgress(SavedData playerProgress)
        {
            if (playerProgress.GraphicData != null)
            {
                GraphicData = playerProgress.GraphicData;
            }
        }

        public void UpdateProgress(SavedData playerProgress)
        {
            playerProgress.GraphicData = GraphicData;
        }
    }
}