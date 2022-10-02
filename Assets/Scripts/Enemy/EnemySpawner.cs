
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    private struct EnemyChance
    {
        public Enemy prefab;
        public float chance;
    }
    
    [SerializeField] private EnemyChance[] prefabs;
    [SerializeField] private float minSeconds, maxSeconds;
    [SerializeField] private int maxEnemies;
    [SerializeField] private bool countForLevelEnd;
    
    private float _totalChance=-1;
    private float TotalChance
    {
        get
        {
            if (_totalChance < 0)
            {
                _totalChance = prefabs.Sum(p => p.chance);
            }
            return _totalChance;
        }
    }

    private LevelEndObserver _observer;
    private float _timer, _timeNeeded = -1;
    private int _enemiesSpawned;

    private void Awake()
    {
        _observer = FindObjectOfType<LevelEndObserver>();
        _observer.OnLevelEnd += _ => gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (_enemiesSpawned >= maxEnemies)
        {
            _timer = 0;
            return;
        }
        
        if (_timeNeeded < 0)
        {
            _timeNeeded = Random.Range(minSeconds, maxSeconds);
        }

        _timer += Time.deltaTime;

        if (_timer >= _timeNeeded)
        {
            Spawn(RandomPrefab());
            _timer = 0;
            _timeNeeded = -1;
        }
    }
    
    private Enemy RandomPrefab()
    {
        float rand = Random.Range(0, TotalChance);
        float sum = 0;
        foreach (var enemyChance in prefabs)
        {
            sum += enemyChance.chance;
            if (rand <= sum) return enemyChance.prefab;
        }

        return null;
    }

    private void Spawn(Enemy prefab)
    {
        Enemy instance = Instantiate(prefab, transform.position, Quaternion.identity);
        instance.Alerted = true;
        _enemiesSpawned++;

        HealthManager health = instance.GetComponent<HealthManager>();
        health.OnDeath += () => _enemiesSpawned--;
        _observer.OnLevelEnd += state => {
            if (state == LevelEndObserver.EndState.Victory && health != null) {
                health.Kill();
            }
        };
        //health.OnDeath += () => _observer.OnLevelEnd -= health.Kill;
        
        if (countForLevelEnd)
        {
            _observer.RegisterEnemy(instance);
        }
    }
}