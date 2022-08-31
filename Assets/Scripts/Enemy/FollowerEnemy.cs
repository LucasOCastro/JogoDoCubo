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
            Vector3 vecToPlayer = player.position - transform.position;
            float playerDistance = vecToPlayer.magnitude;
            Vector3 playerDir = vecToPlayer / playerDistance;
            
            //Se já pode atacar, inicia o ataque e para o movimento.
            //Caso decidamos aumentar a abstração, talvez faça sentido fazer esse check em outra classe (como a própria classe de ataque).
            if (Attacker.CanAttack(player))
            {
                Attacker.StartAttack(player);
                _velocity = Vector3.zero;
                return;
            }
            
            //Se está atacando ou não tem como mover/girar, então não chamo as funções de movimento.
            if (playerDir == Vector3.zero || Attacker.Attacking)
            {
                return;
            }
            RotateTowards(playerDir);
            MoveTowards(playerDir);
        }

        private void RotateTowards(Vector3 dir)
        {
            //Rotação instantânea
            if (rotationSpeed == 0)
            {
                transform.forward = dir;
                return;
            }
            
            //Rotação suavizada
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void MoveTowards(Vector3 dir)
        {
            //Aceleração instantânea
            if (accelerationTime == 0)
            {
                transform.position += dir * walkSpeed * Time.deltaTime;
                return;
            }
            
            //Aceleração suavizada
            Vector3 targetVelocity = dir * walkSpeed;
            _velocity = Vector3.MoveTowards(_velocity, targetVelocity, _acceleration * Time.deltaTime);
            transform.position += _velocity * Time.deltaTime;
        }
    }
}