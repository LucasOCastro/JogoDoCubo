using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [Tooltip("Controla quantos graus a rotação vai ter a cada aperto da tecla de rotação. Se for 0, a rotação é contínua. Comportamento estranho em valores não divisores de 360.")]
    [Range(0, 180)]
    [SerializeField] private float incrementSize = 90;
    [SerializeField] private float rotateSpeed = 100;
    [SerializeField] private KeyCode leftRotateKey = KeyCode.Q;
    [SerializeField] private KeyCode rightRotateKey = KeyCode.E;
    
    private CinemachineOrbitalTransposer _orbit;
    private void Awake()
    {
        _orbit = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }
    
    private float CurrentRotation
    {
        get => _orbit.m_XAxis.Value;
        set => _orbit.m_XAxis.Value = value;
    }

    
    private int _rotationDir;
    private float _targetRotation;
    private void Update()
    {
        int newDir = -GetInputDirection(); //negativo porque para o Cinemachine direita é negativo :(
        if (incrementSize == 0) 
        {
            //Para rotação suave, modifico a rotação enquanto o player aperta o botão.
            CurrentRotation += newDir * rotateSpeed * Time.deltaTime;   
            return;
        }
        
        //Se a mudança parte do repouso ou a nova direção é contrária à anterior, iniciar o movimento.
        if (newDir != 0 && newDir != _rotationDir)
        {
            _rotationDir = newDir;
            _targetRotation = TargetSnapRotation();
        }

        //Se a rotação alvo já foi basicamente alcançada, reseto os valores das variáveis. Caso contrário, continuo a rotação.
        if (Mathf.Abs(Mathf.DeltaAngle(CurrentRotation, _targetRotation)) < 1)
        {
            CurrentRotation = _targetRotation;
            _rotationDir = 0;
            return;
        }
        CurrentRotation = Mathf.MoveTowardsAngle(CurrentRotation, _targetRotation, rotateSpeed * Time.deltaTime);
    }

    private float TargetSnapRotation()
    {
        //Calculo o valor bruto da próxima rotação se simplesmente rotacionasse do ponto atual
        float nextRotationRaw = CurrentRotation + incrementSize * _rotationDir;
        float pct = nextRotationRaw / incrementSize; 
        //Para evitar overshooting, volto a rotação para o valor anterior ao valor bruto calculado
        pct = (_rotationDir < 0) ? Mathf.Ceil(pct) : Mathf.Floor(pct); 
        return pct * incrementSize;
    }

    private int GetInputDirection()
    {
        if (Input.GetKey(leftRotateKey))
            return -1;
        if (Input.GetKey(rightRotateKey))
            return 1;
        return 0;
    }
}
