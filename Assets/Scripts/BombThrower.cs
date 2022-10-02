using UnityEngine;
using Random = UnityEngine.Random;

//Faria muito sentido transformar isso em uma child class de Shooter e transformar o ArcProjectile em um tipo de IProjectile, porém
public class BombThrower : MonoBehaviour
{
    [SerializeField] private ArcProjectile bombPrefab;
    [SerializeField] private Transform bombOrigin;
    [SerializeField] private RandomTimer timer;
    [RadiusDraw(1, 0, 0)]
    [SerializeField] private FloatRange distanceRange;

    private void Update()
    {
        if (!timer.Tick(Time.deltaTime))
        {
            return;
        }

        float randAngle = Random.Range(0, 360);
        Vector3 dir = Quaternion.AngleAxis(randAngle, Vector3.up) * Vector3.forward;
        Vector3 finalPos = transform.position + (dir * distanceRange.Random);
        Throw(finalPos);
    }

    private void Throw(Vector3 target)
    {
        Vector3 origin = bombOrigin ? bombOrigin.position : transform.position;
        bombPrefab.MakeInstance(origin, target);
    }
}