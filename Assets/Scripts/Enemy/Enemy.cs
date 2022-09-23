using UnityEngine;

public class Enemy : BehaviorRunner
{
    [SerializeField] private float activationRange;

    //TODO isso e tudo sobre ragdoll deveria estar na classe de vida
    [SerializeField] private GameObject ragdollPrefab;
    public void SpawnRagdoll(Vector3 impactPoint, Vector3 force)
    {
        var instance = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        var rb = instance.GetComponentInChildren<Rigidbody>();
        rb.AddForceAtPosition(force, impactPoint, ForceMode.Impulse);
        Destroy(gameObject);
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
    }

    protected override Behavior GetBehavior()
    {
        if (_attacker.Attacking)
        {
            return null;
        }
        
        Transform player = Player.Instance.transform;
        if (_attacker.CanAttack(player))
        {
            _attacker.StartAttack(player);
            return null;
        }
        
        if (Alerted)
        {
            return _pursue;
        }
        
        float sqrDistanceToPlayer = (transform.position - player.position).sqrMagnitude;
        if (sqrDistanceToPlayer < activationRange * activationRange)
        {
            Alerted = true;
        }

        return _wander;
    }
}