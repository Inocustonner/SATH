using System;
using Code.Data.Interfaces;
using Code.Materials;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Components.DisownChoice
{
    public class PlayerMoveObserver: MonoBehaviour, IGameInitListener, IPartTickListener
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Vector2 _sceneBoarder;
        [SerializeField] private GrayMaterialController[] _grayMaterialControllers;
        [SerializeField] private GlowMaterialController _glowDoorLightMaterial;
        [SerializeField] private ParticleSystem _particleSystem;
     
        public float MIN = 0f;
        public float MAX = 1f;
        private ParticleSystem.MainModule _mainPartcleModule;

        public void GameInit()
        {
            _mainPartcleModule = _particleSystem.main;
        }

        public void PartTick()
        {
            var normalize = NormalizePlayerPosition(_player.position.x, _sceneBoarder.x, _sceneBoarder.y, MIN, MAX);
            foreach (var grayMaterialController in _grayMaterialControllers)
            {
                grayMaterialController.SetFloatValue(normalize);
            }

            var reverseNormalize = NormalizePlayerPosition(_player.position.x, _sceneBoarder.y, _sceneBoarder.x, MIN, MAX);
            SetParticleStartColor(reverseNormalize);
            _glowDoorLightMaterial.SetFloatValue(reverseNormalize * 10);
        }

        private float NormalizePlayerPosition(float playerX, float sceneMinX, float sceneMaxX, float min, float max)
        {
            var normalizedValue = Mathf.InverseLerp(sceneMinX, sceneMaxX, playerX);
            return Mathf.Lerp(min, max, normalizedValue);
        }

        private void SetParticleStartColor(float reverseNormalize)
        {
            var particleColor = _mainPartcleModule.startColor.color;
            particleColor.a = reverseNormalize;
            var startColor = _mainPartcleModule.startColor;
            startColor.color = particleColor;
            _mainPartcleModule.startColor = startColor;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(new Vector3(_sceneBoarder.x,0,0), new Vector3(_sceneBoarder.y,0,0));
        }
    }
}