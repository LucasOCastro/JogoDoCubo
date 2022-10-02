using UnityEngine;

public class BehaviorAnimator : MonoBehaviour
{
    private static readonly int XVel = Animator.StringToHash("xVel");
    private static readonly int YVel = Animator.StringToHash("yVel");


    private Animator _animator;
    private BehaviorRunner _runner;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _runner = GetComponent<BehaviorRunner>();
    }

    public void PlayAnimation(int animHash)
    {
        _animator.Play(animHash);
    }

    private void Update()
    {
        Vector2 velocity = Vector2.zero;
        if (_runner != null && _runner.CurrentBehavior is MovementBehavior movement)
        {
            Vector3 orientedVel = transform.InverseTransformDirection(movement.Velocity.normalized);
            velocity = new Vector2(orientedVel.x, orientedVel.z);
        }
        
        SetVelocity(velocity);
    }

    private void SetVelocity(Vector2 velocity)
    {
        _animator.SetFloat(XVel, velocity.x);
        _animator.SetFloat(YVel, velocity.y);
    }
}