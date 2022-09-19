using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static Level CurrentLevel { get; private set; }


    [SerializeField] private Level[] levels = Array.Empty<Level>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(gameObject);
        EnsureCorrectLevelIsLoaded();
    }

    private void EnsureCorrectLevelIsLoaded()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (CurrentLevel != null && CurrentLevel.Scene == activeScene)
        {
            return;
        }

        foreach (var level in levels)
        {
            if (level.Scene == activeScene)
            {
                CurrentLevel = level;
                break;
            }
        }
    }

    public static void LoadNextLevel()
    {
        if (CurrentLevel == null)
        {
            LoadLevel(0);
            return;
        }
        
        var levels = _instance.levels;
        for (int i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i] == CurrentLevel)
            {
                LoadLevel(i+1);
                return;
            }
        }

        Debug.Log("Não consegui carregar o próximo level");
    }

    public static void LoadLevel(int lvl) => LoadLevel(_instance.levels[lvl]);
    private static  void LoadLevel(Level lvl)
    {
        SceneManager.LoadScene(lvl.Scene.buildIndex);
        CurrentLevel = lvl;
    }
}