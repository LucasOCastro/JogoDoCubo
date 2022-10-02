using System;
using System.Collections;
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
            RegisterEnemy(enemy);
        }
        
        var player = Player.Instance;
        var playerHealth = player.GetComponent<HealthManager>();
        playerHealth.OnDeath += Defeat;

        GuaranteeIsActive(victoryScreen);
        GuaranteeIsActive(defeatScreen);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
        var enemyHealth = enemy.GetComponent<HealthManager>();
        enemyHealth.OnDeath += () => OnEnemyDeath(enemy);
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
    
    private void EndWithScreen(Screen screen) => StartCoroutine(EndWithScreenCoroutine(screen));
    private IEnumerator EndWithScreenCoroutine(Screen screen)
    {
        yield return new WaitForSeconds(secondsBeforeEndScreen);
        screen.SetShown(true);
    }
}
