using UnityEngine.UIElements;

public class VictoryScreen : Screen
{
    protected override void Setup()
    {
        root.Q<Button>("NextLevelButton").clicked += LevelManager.LoadNextLevel;
        root.Q<Button>("RetryButton").clicked += LevelManager.ResetLevel;
        root.Q<Button>("MenuButton").clicked += LevelManager.LoadMenu;
    }
}