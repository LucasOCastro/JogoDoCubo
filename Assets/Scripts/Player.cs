using UnityEngine;

public class Player : BehaviorRunner
{
    public static Player Instance {get; private set;}

    [SerializeField] private Behavior playerBehavior;

    protected override Behavior GetBehavior()
    {
        return playerBehavior;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}