using Code.Data.Interfaces;
using UnityEngine;

namespace Code.Game.Components.Destruction
{
    public class BossBullet : Bullet
    {
        [Header("Components")] 
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private CollisionObserver _collisionObserver;

        [Header("Static data")] 
        [SerializeField] private float laserDuration = 1f;
        [SerializeField] private float laserSpeed = 10f;
        [SerializeField] private float angleRange = 60f; // Диапазон угла в градусах

        private float laserTime;
        private Vector2 direction;
        private Vector3 startPosition;
        public int Damage = 1;
        private Vector3 rotatedDirection;

        public override void InitEntity(params object[] parameters)
        {
            Damage = (int)parameters[0];
            laserSpeed = (float)parameters[1];
            startPosition = (Vector3)parameters[2]; // Начальная позиция лазера
            direction = (Vector3)parameters[3]; // Направление лазера
        }

        public override void EnableEntity()
        {
            Debug.Log("BossBullet enabled");
            _collisionObserver.OnEnter += OnCollision;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = true;
            laserTime = laserDuration;
            var angle = Random.Range(-angleRange / 2, angleRange / 2);
            rotatedDirection = Quaternion.Euler(0, 0, angle) * direction;
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, startPosition + (Vector3)(rotatedDirection * 100f));
        }

        public override void DisableEntity()
        {
            Debug.Log("BossBullet disabled");
            _collisionObserver.OnEnter -= OnCollision;
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }

        public override void Move()
        {
            if (laserTime > 0)
            {
                // Определяем направление выстрела в пределах заданного диапазона

                // Создаем луч от начальной позиции
                RaycastHit2D hit = Physics2D.Raycast(startPosition, rotatedDirection);

                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(0, startPosition);
                    lineRenderer.SetPosition(1, hit.point);

                    laserTime -= Time.deltaTime * laserSpeed;
                    Vector3 endPoint = Vector3.Lerp(hit.point, startPosition, 1 - (laserTime / laserDuration));
                    lineRenderer.SetPosition(1, endPoint);
                }
                else
                {
                    lineRenderer.SetPosition(0, startPosition);
                    lineRenderer.SetPosition(1, startPosition + (Vector3)(rotatedDirection * 100f));
                }
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }

        private void OnCollision(GameObject obj)
        {
            Debug.Log("BossBullet collided with " + obj.name);
            // Логика при столкновении
        }
    }
}
