using UnityEngine;

public class Enemy : BehaviorRunner
{
    [SerializeField] private float viewHeightOffset;
    [SerializeField] private float activationRange;
    [SerializeField] private LayerMask viewBlockMask;
    
    private bool CanSee(Vector3 pos)
    {
        // Sem angulo de visão
        float sqrDistanceToPlayer = (transform.position - pos).sqrMagnitude;
        if (sqrDistanceToPlayer > activationRange * activationRange)
        {
            return false;
        }

        Vector3 castStart = transform.position + Vector3.up * viewHeightOffset;
        return !Physics.Linecast(castStart, pos, viewBlockMask);
    }

    public bool Alerted { get; private set; }

    //Idealmente, quando lidamos com Behaviors, temos uma estrutura de decisão como uma Behavior Tree.
    //O jogo é bem simples, e os inimigos mal aparecerão na câmera, então não implementei esse sistema.
    //Ao invés disso, optei por ter esse script central escolhendo qual Behavior executar, de forma hardcoded.
    private EnemyAttacker _attacker;
    private WanderBehavior _wander;
    private PursuePlayerBehavior _pursue;
    private void Awake()
    {
        _attacker = GetComponent<EnemyAttacker>();
        _wander = GetComponent<WanderBehavior>();
        _pursue = GetComponent<PursuePlayerBehavior>();

        GetComponent<HealthManager>().OnDamaged += () => Alerted = true;
    }

    protected override Behavior GetBehavior()
    {
        if (_attacker.Attacking)
        {
            return null;
        }

        var player = Player.Instance;
        if (player == null)
        {
            return _wander;
        }
        
        Transform playerTransform = player.transform;
        if (!Alerted && CanSee(playerTransform.position))
        {
            Alerted = true;
        }
        
        if (Alerted && _attacker.CanAttack(playerTransform))
        {
            _attacker.StartAttack(player.GetComponent<HealthManager>());
            return null;
        }
            
        if (Alerted)
        {
            return _pursue;
        }

        return _wander;
    }
}