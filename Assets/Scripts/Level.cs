using UnityEngine;

[System.Serializable]
public class Level
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;

    [SerializedScene] [SerializeField] private int scene;
    public int SceneBuildIndex => scene;
}