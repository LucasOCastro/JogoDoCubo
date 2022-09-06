using UnityEngine;

public abstract class MovementBehavior : Behavior
{
    [Header("Movement")]
    [Tooltip("Tempo para o inimigo ir do repouso ao movimento pleno. Se for 0, a aceleração será instantânea.")]
    [SerializeField] private float accelerationTime;
    [Tooltip("A velocidade máxima do inimigo.")]
    [SerializeField] private float walkSpeed;
    [Tooltip("Velocidade com que o inimigo vira ao mudar de direção. Apenas cosmético. Se for 0, a rotação será instantânea.")]
    [SerializeField] private float rotationSpeed;
    
    protected abstract Vector3 GetMoveDirection();
    protected abstract Vector3 GetLookDirection();

    /// <summary>
    /// Se negativo, então o movimento é linear.
    /// </summary>
    protected float Acceleration => accelerationTime > 0 ? walkSpeed / accelerationTime : -1;
    protected float Velocity => _velocity.magnitude;

    private Vector3 _velocity;
    public override void Tick()
    {
        Vector3 moveDir = GetMoveDirection();
        MoveTowards(moveDir);

        Vector3 lookDir = GetLookDirection();
        if (lookDir != Vector3.zero)
        {
            RotateTowards(lookDir);
        }
    }
    
    private void RotateTowards(Vector3 dir)
    {
        //Rotação instantânea
        if (rotationSpeed == 0)
        {
            transform.forward = dir;
            return;
        }
            
        //Rotação suavizada
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveTowards(Vector3 dir)
    {
        //Aceleração instantânea
        if (accelerationTime == 0)
        {
            transform.position += dir * walkSpeed * Time.deltaTime;
            return;
        }
            
        //Aceleração suavizada
        Vector3 targetVelocity = dir * walkSpeed;
        _velocity = Vector3.MoveTowards(_velocity, targetVelocity, Acceleration * Time.deltaTime);
        transform.position += _velocity * Time.deltaTime;
    }
}