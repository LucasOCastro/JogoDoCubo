using System;
using UnityEngine;

public abstract class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    private float _cooldownTimer;

    public bool Attacking { get; private set; }
    
    //Aqui eu passo o player como parâmetro porque o alvo do inimigo sempre vai ser o player,
    //entao evito fazer um cast físico e salvo um pouco de performance.
    //Uma outra opção seria transformar um player num singleton, ao invés de passar por parametro.
    //De qualquer forma, seria interessante passar uma classe relevante ao invés do Transform.
    public virtual bool CanAttack(Transform player) => _cooldownTimer >= cooldownTime;
    
    private void Update()
    {
        _cooldownTimer += Time.deltaTime;
        if (Attacking)
        {
            AttackUpdate();
        }
    }
    
    protected Transform Target { get; private set; }
    public void StartAttack(Transform player)
    {
        Attacking = true;
        Target = player;
        OnAttackStart();
    }

    protected void EndAttack()
    {
        Attacking = false;
        Target = null;
        _cooldownTimer = 0;
    }

    protected abstract void OnAttackStart();
    protected abstract void AttackUpdate();
}