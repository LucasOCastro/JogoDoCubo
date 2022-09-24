using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum AnimState : Byte {motionless, idle, running};

public class PlayerControlBehavior : MovementBehavior
{
    [Tooltip("O animatior do player, cujo estado de animação será modificado durante o jogo.")]
    [SerializeField] private Animator animator;
    [Tooltip("Quanto tem o bob fica parado sem fazer nada.")]
    [SerializeField] private float MotionlessDuration;
    [Tooltip("Quanto tem o bob fica parado fazendo uma animação idle.")]
    [SerializeField] private float IdleDuration;

    private float MotionlessTimeCounter = 0;
    private float IdleTimeCounter = 0;

    private void FixedUpdate() {
        //Debug.Log(getPlayerMovIntention ());
        if (0 < GetPlayerMovIntention().magnitude)
        {
            //Debug.Log("a");
            animator.SetBool("Run", true);
            animator.SetBool("Motionless", false);
            animator.SetBool("Idle", false);
        } else if (MotionlessTimeCounter <= MotionlessDuration) {
            animator.SetBool("Run", false);
            animator.SetBool("Motionless", true);
            animator.SetBool("Idle", false);

            MotionlessTimeCounter += Time.deltaTime;
        } else if (IdleTimeCounter <= IdleDuration) {
            animator.SetBool("Run", false);
            animator.SetBool("Motionless", false);
            animator.SetBool("Idle", true);

            IdleTimeCounter += Time.deltaTime;
        } else {
            MotionlessTimeCounter = 0;
            IdleTimeCounter = 0;
        }
    }
    
    private Vector3 GetPlayerMovIntention()
    {
        Vector3 dir = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
        return CameraUtility.RotateFlatVector(dir);
    }

    protected override Vector3 GetMoveDirection() => GetPlayerMovIntention();
    protected override Vector3 GetLookDirection() => CameraUtility.DirectionToMouse(transform.position);
}