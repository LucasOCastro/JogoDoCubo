using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FollowerEnemy : Enemy
    {
        [Header("Follower Enemy")]
        [Tooltip("Tempo para o inimigo ir do repouso ao movimento pleno. Se for 0, a aceleração será instantânea.")]
        [SerializeField]
        private float accelerationTime;
        [Tooltip("A velocidade máxima do inimigo.")]
        [SerializeField] private float walkSpeed;
        [Tooltip("Velocidade com que o inimigo vira ao mudar de direção. Apenas cosmético. Se for 0, a rotação será instantânea.")]
        [SerializeField] private float rotationSpeed;

        private Vector3 _velocity;
        private float _acceleration;
        protected override void OnSpawn()
        {
            _acceleration = (accelerationTime > 0) ? walkSpeed / accelerationTime : 0;
        }
        
        protected  override void PursuePlayer()
        {
            Vector3 vecToPlayer = player.transform.position - transform.position;
            float playerDistance = vecToPlayer.magnitude;
            Vector3 playerDir = vecToPlayer / playerDistance;
            
            if (playerDir == Vector3.zero)
            {
                return;
            }
            RotateTowards(playerDir);
            MoveTowards(playerDir);
        }

        private void RotateTowards(Vector3 dir)
        {
            if (rotationSpeed == 0)
            {
                transform.forward = dir;
                return;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void MoveTowards(Vector3 dir)
        {
            if (accelerationTime == 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, walkSpeed * Time.deltaTime);
                return;
            }
            
            Vector3 targetVelocity = dir * walkSpeed;
            _velocity = Vector3.MoveTowards(_velocity, targetVelocity, _acceleration * Time.deltaTime);
            transform.position += _velocity * Time.deltaTime;
        }
    }
}