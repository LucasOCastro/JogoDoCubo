using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum AnimState : Byte {motionless, idle, running};

public class PlayerControl : MovementBehavior
{
    [Tooltip("O animatior do player, cujo estado de animação será modificado durante o jogo.")]
    [SerializeField] private UnityEngine.Animator animator;
    [Tooltip("Quanto tem o bob fica parado sem fazer nada.")]
    [SerializeField] private float MotionlessDuration;
    [Tooltip("Quanto tem o bob fica parado fazendo uma animação idle.")]
    [SerializeField] private float IdleDuration;

    private float MotionlessTimeCounter;
    private float IdleTimeCounter;

    private void Start() {
        MotionlessTimeCounter = 0;
        IdleTimeCounter = 0;
    }

    private void FixedUpdate() {
        //Debug.Log(getPlayerMovIntention ());
        if (0 < getPlayerMovIntention().magnitude)
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

        Tick();
    }
    
    private Vector3 getPlayerMovIntention ()
    { 
        Vector3 v = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
        //Debug.Log(v * 1);
        return v;
    }

    protected override Vector3 GetMoveDirection() => getPlayerMovIntention ();
    protected override Vector3 GetLookDirection() => getPlayerMovIntention ();
}