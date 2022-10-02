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
    [SerializeField] private IntRange bombCountRange = (1,1);

    private void Update()
    {
        if (timer.Tick(Time.deltaTime))
        {
            ThrowBombs(bombCountRange.Random);
        }
    }

    private void ThrowBombs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 finalPos = RandomPos();
            Throw(finalPos); 
        }
    }

    private Vector3 RandomPos()
    {
        float randAngle = Random.Range(0, 360);
        Vector3 dir = Quaternion.AngleAxis(randAngle, Vector3.up) * Vector3.forward;
        return transform.position + (dir * distanceRange.Random);
    }

    private void Throw(Vector3 target)
    {
        Vector3 origin = bombOrigin ? bombOrigin.position : transform.position;
        bombPrefab.MakeInstance(origin, target);
    }
}