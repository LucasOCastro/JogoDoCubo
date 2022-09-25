using System;
using UnityEngine;

public abstract class BehaviorRunner : MonoBehaviour
{
    public Behavior CurrentBehavior { get; private set; }

    protected abstract Behavior GetBehavior();

    private void FixedUpdate()
    {
        if (CurrentBehavior != null)
        {
            CurrentBehavior.Tick();
        }
    }

    private void Update()
    {
        Behavior newBehavior = GetBehavior();
        if (newBehavior != CurrentBehavior)
        {
            CurrentBehavior = newBehavior;
        }
    }
}