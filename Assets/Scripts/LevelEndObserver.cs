using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelEndObserver : MonoBehaviour
{
    [SerializeField] private Screen victoryScreen;
    [SerializeField] private Screen defeatScreen;
    [SerializeField] private float secondsBeforeEndScreen;
    private HashSet<Enemy> _enemies = new HashSet<Enemy>();
    private void Start()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            _enemies.Add(enemy);
            var enemyHealth = enemy.GetComponent<HealthManager>();
            enemyHealth.OnDeath += () => OnEnemyDeath(enemy);
        }
        
        var player = Player.Instance;
        var playerHealth = player.GetComponent<HealthManager>();
        playerHealth.OnDeath += Defeat;

        GuaranteeIsActive(victoryScreen);
        GuaranteeIsActive(defeatScreen);
    }

    void GuaranteeIsActive(Screen screen)
    {
        if (!screen.gameObject.activeSelf)
        {
            screen.gameObject.SetActive(true);
            screen.SetShown(false);
        }
    }

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
        EndWithScreen(victoryScreen);
    }
    private void Defeat()
    {
        EndWithScreen(defeatScreen);
    }
    private async void EndWithScreen(Screen screen)
    {
        await Task.Delay((int)(secondsBeforeEndScreen * 1000));
        screen.SetShown(true);
    }
}
