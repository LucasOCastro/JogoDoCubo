using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Level
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;

    [SerializedScene] [SerializeField] private string scene;
    public Scene Scene => SceneManager.GetSceneByPath(scene);
}