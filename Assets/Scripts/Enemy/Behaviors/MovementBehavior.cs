using UnityEngine;

public abstract class MovementBehavior : Behavior
{
    [Header("Movement")]
    [Tooltip("Tempo para o inimigo ir do repouso ao movimento pleno. Se for 0, a aceleração será instantânea.")]
    [SerializeField] private float accelerationTime;
    [Tooltip("A velocidade máxima do inimigo.")]
    [SerializeField] private float walkSpeed;
    [Tooltip("Velocidade com que o inimigo vira ao mudar de direção. Se for 0, a rotação será instantânea.")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Se true, só iniciará o movimento se estiver olhando pra direção certa.")]
    [SerializeField] private bool moveAfterRotating;

    protected abstract Vector3 GetMoveDirection();
    protected abstract Vector3 GetLookDirection();

    /// <summary>
    /// Se negativo, então o movimento é linear.
    /// </summary>
    public float Acceleration => accelerationTime > 0 ? walkSpeed / accelerationTime : -1;
    public float CurrentSpeed => Velocity.magnitude;
    public Vector3 Velocity { get; private set; }

    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Tick()
    {
        Vector3 lookDir = GetLookDirection();
        if (lookDir != Vector3.zero)
        {
            RotateTowards(lookDir);
        }
        
        Vector3 moveDir = GetMoveDirection();
        if (moveAfterRotating && transform.forward != lookDir) {
            moveDir = Vector3.zero;
        }
        MoveTowards(moveDir);
    }

    public override void Exit()
    {
        _rb.velocity = Vector3.zero;
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
        _rb.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveTowards(Vector3 dir)
    {
        //Aceleração instantânea
        if (accelerationTime == 0)
        {
            _rb.velocity = dir * walkSpeed;
            return;
        }

        //Aceleração suavizada
        Vector3 targetVelocity = dir * walkSpeed;
        Velocity = Vector3.MoveTowards(Velocity, targetVelocity, Acceleration * Time.deltaTime);
        _rb.velocity = Velocity;
    }
}