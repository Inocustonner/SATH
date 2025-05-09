﻿using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components
{
    public class LayerZoneController : MonoBehaviour, IGameInitListener, IPartTickListener
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _zoneLayer;
        [SerializeField] private Bounds _bounds;

        private int _defaultLayer;

        public void GameInit()
        {
            _defaultLayer = _spriteRenderer.sortingOrder;
        }

        public void PartTick()
        {
            if (Inside())
            {
                _spriteRenderer.sortingOrder =  _zoneLayer;
            }
            else if(_spriteRenderer.sortingOrder != _defaultLayer)
            {
                _spriteRenderer.sortingOrder = _defaultLayer;
            }
        }

        private Vector3 ThisPosition()
        {
            return transform.position + _bounds.center;
        }
        
        private float HorizontalOffset()
        {
            return _bounds.extents.x / 2;
        }
        
        private float VerticalOffset()
        {
            return _bounds.extents.y / 2;
        }
        
        private Vector3 SpritePos()
        {
            return _spriteRenderer.transform.position;
        }

        private bool Inside()
        {
            return SpritePos().x > ThisPosition().x - HorizontalOffset()
                   && SpritePos().x < ThisPosition().x + HorizontalOffset()
                   && SpritePos().y > ThisPosition().y - VerticalOffset()
                   && SpritePos().y < ThisPosition().y + VerticalOffset();
        }
        
        private void OnDrawGizmosSelected()
        {
            var color = Color.cyan;
            color.a = 0.1f;
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position + _bounds.center, _bounds.extents);
        }
    }
}