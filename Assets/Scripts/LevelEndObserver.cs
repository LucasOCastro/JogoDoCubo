using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndObserver : MonoBehaviour
{
    [SerializeField] private Screen victoryScreen;
    [SerializeField] private Screen defeatScreen;
    private HashSet<Enemy> _enemies = new HashSet<Enemy>();
    private void Start()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            _enemies.Add(enemy);
            //enemy.Health.OnDeath += () => OnEnemyDeath(enemy);
        }
        
        //var player = Player.Instance;
        //player.Health.OnDeath += Defeat;
    }

#if UNITY_EDITOR
    private void Update()
    {
        //TODO DEBUG
        if (Input.GetKeyDown(KeyCode.V)) Victory();
        else  if (Input.GetKeyDown(KeyCode.D)) Defeat();
    }
#endif

    private void OnEnemyDeath(Enemy enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            Victory();
        }
    }

    private void Victory()
    {
        victoryScreen.SetShown(true);
    }

    private void Defeat()
    {
        defeatScreen.SetShown(true);
    }
}
