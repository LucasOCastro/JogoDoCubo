using UnityEngine;

public class AreaEnemyAttacker : EnemyAttacker
{
    [SerializeField] private float distance;
    [Range(0, 360)] [SerializeField] private float angle;
    [SerializeField] private string animationName;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private bool IsInDistance(Vector3 point) => (point - transform.position).sqrMagnitude <= distance * distance;
    private bool IsInAngle(Vector3 point) => Vector3.Angle(transform.forward, point - transform.position) <= angle * .5f;
    private bool IsInArea(Vector3 point) => IsInDistance(point) && IsInAngle(point);

    public override bool CanAttack(Transform player) => base.CanAttack(player) && IsInArea(player.position);

    private BehaviorAnimator _behaviorAnimator;
    private int _animationHash;
    private bool _attackWindow;
    private void Awake()
    {
        _behaviorAnimator = GetComponent<BehaviorAnimator>();
        _animationHash = Animator.StringToHash(animationName);
    }

    protected override void OnAttackStart()
    {
        _behaviorAnimator.PlayAnimation(_animationHash);
    }

    protected override void AttackUpdate()
    {
        if (Target == null)
        {
            return;
        }
        
        if (_attackWindow && IsInArea(Target.transform.position))
        {
            source.PlayOneShot(clip);
            DamageTarget();
            _attackWindow = false;
            
        }
    }

    //Esses callbacks são chamados em animator events na animação de ataque.
    //Quando a animação permite, _attackWindow é true e o ataque pode causar dano.
    private void AttackWindowStart() => _attackWindow = true;
    private void AttackWindowEnd() => _attackWindow = false;
}