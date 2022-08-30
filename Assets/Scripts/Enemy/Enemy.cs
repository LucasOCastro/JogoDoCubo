using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected Transform player;
    [SerializeField] private float activationRange;
    
    public bool Alerted { get; protected set; }
    private void Awake()
    {
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