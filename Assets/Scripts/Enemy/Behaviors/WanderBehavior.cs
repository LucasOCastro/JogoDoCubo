using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderBehavior : MovementBehavior
{
    [Header("Wander")]
    [SerializeField] private float maxDistanceFromOrigin;
    [SerializeField] private float changeSecondsMin, changeSecondsMax;
    [Tooltip("O range de ângulo que o inimigo pode seguir depois de ultrapassar a distância máxima da origem")]
    [Range(0, 90)]
    [SerializeField] private float returnAngle;
    [Range(0,100)]
    [SerializeField] private int chanceToStopWhenChanging;

    private Vector3 _startPosition;
    private Vector3 _currentTargetDir;
    private float _changeTimer, _secondsToChange;
    private void Start()
    {
        _startPosition = transform.position;
    }

    private bool ExceededDistanceLimit()
    {
        Vector3 toStart = _startPosition - transform.position;
        
        float adjustedMaxDistance = maxDistanceFromOrigin;
        if (Acceleration > 0)
        {
            //Se o movimento é acelerado, até conseguir mudar de direção talvez já ultrapassou o limite.
            //Sendo assim, também considero o deslocamento do MUV na comparação de distância.
            //dV = a*t <-> t = dV/a <-> t = Vo/a  ==>  dS = Vot - at²/2 <-> dS = Vo²/a - Vo²/2a <-> [dS = Vo²/2a]
            float vel = Velocity;
            float deltaPos = (vel * vel) / (2 * Acceleration); 
            adjustedMaxDistance -= deltaPos;
        }
        
        if (toStart.sqrMagnitude < adjustedMaxDistance * adjustedMaxDistance)
        {
            return false;
        }

        //Se o ângulo já está propício para voltar à origem, então já está encaminhado e não precisa mudar.
        float angle = Vector3.Angle(toStart, _currentTargetDir);
        return angle > returnAngle * .5f; 
    } 
    public override void Tick()
    {
        base.Tick();
        
        //O inimigo muda de direção de tempo em tempo, ou quando ultrapassa o limite de distância do ponto original.
        bool exceededLimit = ExceededDistanceLimit();
        _changeTimer += Time.deltaTime;
        if (_changeTimer >= _secondsToChange || exceededLimit)
        {
            GetNewDirection(exceededLimit);
        }
    }

    private bool _justStopped;
    private void GetNewDirection(bool exceededLimit)
    {
        _secondsToChange = Random.Range(changeSecondsMin, changeSecondsMax);
        _changeTimer = 0;

        //Aleatoriamente faz uma pausa ao mudar de direção.
        if (!exceededLimit && !_justStopped && Random.Range(0,100) < chanceToStopWhenChanging)
        {
            _currentTargetDir = Vector3.zero;
            _justStopped = true;
            return;
        }

        float angleMin = 0, angleMax = 360;
        Vector3 forward = Vector3.forward;
        //Se ultrapassou o limite de distância, limito as possíveis direções pra garantir que volte pra dentro da área permitida
        if (exceededLimit)
        {
            forward = (_startPosition - transform.position).normalized;
            angleMin = -returnAngle * .5f;
            angleMax = returnAngle * .5f;;
        }

        float angle = Random.Range(angleMin, angleMax);
        _currentTargetDir = Quaternion.AngleAxis(angle, Vector3.up) * forward;
        _justStopped = false;
    }

    protected override Vector3 GetLookDirection() => _currentTargetDir;
    protected override Vector3 GetMoveDirection() => _currentTargetDir;
}