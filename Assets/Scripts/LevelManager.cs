using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static Level CurrentLevel { get; private set; }

    [SerializedScene] [SerializeField] private string menuScene;
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
        if (CurrentLevel != null && CurrentLevel.SceneBuildIndex == activeScene.buildIndex)
        {
            return;
        }

        foreach (var level in levels)
        {
            if (level.SceneBuildIndex == activeScene.buildIndex)
            {
                CurrentLevel = level;
                break;
            }
        }
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(_instance.menuScene);
    }

    public static void LoadLevel(int lvl)
    {
        if (lvl < 0 || lvl >= _instance.levels.Length)
        {
            Debug.LogError($"{lvl} é um índice de level inválido.");
            return;
        }
        LoadLevel(_instance.levels[lvl]);
    }
    private static void LoadLevel(Level lvl)
    {
        SceneManager.LoadScene(lvl.SceneBuildIndex);
        CurrentLevel = lvl;
    }

    public static void ResetLevel() => LoadLevel(CurrentLevel);
    
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

        Debug.Log("Não tem carregar o próximo level");
    }
}