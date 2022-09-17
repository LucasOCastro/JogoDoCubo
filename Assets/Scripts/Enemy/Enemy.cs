using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] private float activationRange;
    
    
    public bool Alerted { get; private set; }
    public Behavior CurrentBehavior { get; private set; }
    
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
    }

    private void Update()
    {
        if (_attacker.Attacking)
        {
            return;
        }
        
        if (_attacker.CanAttack(player))
        {
            _attacker.StartAttack(player);
            CurrentBehavior = null;
            return;
        }
        
        if (Alerted)
        {
            CurrentBehavior = _pursue;
            _pursue.Tick();
            return;
        }

        CurrentBehavior = _wander;
        _wander.Tick();
        
        float sqrDistanceToPlayer = (transform.position - player.position).sqrMagnitude;
        if (sqrDistanceToPlayer < activationRange * activationRange)
        {
            Alerted = true;
        }
    }
}