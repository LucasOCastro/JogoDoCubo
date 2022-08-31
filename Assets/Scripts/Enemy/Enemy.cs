using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] private float activationRange;
    
    public bool Alerted { get; protected set; }
    protected  EnemyAttacker Attacker { get; private set; }
    private void Awake()
    {
        Attacker = GetComponent<EnemyAttacker>();
        OnSpawn();
    }

    private void Update()
    {
        if (Alerted)
        {
            PursuePlayer();
            return;
        }
        
        float sqrDistanceToPlayer = (transform.position - player.position).sqrMagnitude;
        if (sqrDistanceToPlayer < activationRange * activationRange)
        {
            Alerted = true;
        }
    }

    protected abstract void OnSpawn();
    protected abstract void PursuePlayer();
}