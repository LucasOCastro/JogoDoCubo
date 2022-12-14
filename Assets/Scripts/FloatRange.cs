using System;

[Serializable]
public struct FloatRange : IEquatable<FloatRange>
{
    [UnityEngine.SerializeField]
    private float _min;
    public float Min
    {
        get => _min;
        set
        {
            if (value > _max)
            {
                _min = _max;
                _max = value;
                return;
            }
            _min = value;
        }
    }
    
    [UnityEngine.SerializeField]
    private float _max;
    public float Max
    {
        get => _max;
        set
        {
            if (value < _min)
            {
                _max = _min;
                _min = value;
                return;
            }
            _max = value;
        }
    }

    public float Medium => (Min + Max) * .5f;
    public float Random => UnityEngine.Random.Range(Min, Max);

    public FloatRange(float min, float max)
    {
        _min = (min > max) ? max : min;
        _max = (min > max) ? min : max;
    }

    public bool Equals(FloatRange other)
    {
        return _min.Equals(other._min) && _max.Equals(other._max);
    }
    public override bool Equals(object obj)
    {
        return obj is FloatRange other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(_min, _max);
    }
    
    public static explicit operator IntRange(FloatRange fr) => new IntRange((int)fr._min, (int)fr._max);
    public static implicit operator FloatRange((float min, float max) tuple) => new FloatRange(tuple.min, tuple.max);
}