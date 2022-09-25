using UnityEngine.UIElements;

public class DefeatScreen : Screen
{
    protected override void Setup()
    {
        root.Q<Button>("RetryButton").clicked += LevelManager.ResetLevel;
        root.Q<Button>("MenuButton").clicked += LevelManager.LoadMenu;
    }
}