using System;

[Serializable]
public class RandomTimer
{
    [UnityEngine.SerializeField] private float minTime, maxTime;

    private float _neededTime = -1, _timer;
    public bool Tick(float delta)
    {
        if (_neededTime < 0)
        {
            _neededTime = UnityEngine.Random.Range(minTime, maxTime);
        }

        _timer += delta;
        if (_timer >= _neededTime)
        {
            _timer = 0;
            _neededTime = -1;
            return true;
        }
        return false;
    }
}