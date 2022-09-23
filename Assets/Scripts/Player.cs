using UnityEngine;

public class Player : BehaviorRunner
{
    public static Player Instance {get; private set;}

    private PlayerControlBehavior _playerBehavior;

    protected override Behavior GetBehavior()
    {
        return _playerBehavior;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerBehavior = GetComponent<PlayerControlBehavior>();
    }
}