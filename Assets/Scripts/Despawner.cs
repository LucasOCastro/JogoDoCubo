using UnityEngine;

public class Despawner : MonoBehaviour
{
    [SerializeField] private float despawnSeconds;

    private float _timer;
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= despawnSeconds)
        {
            Destroy(gameObject);
        }
    }
}
