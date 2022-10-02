using System;

[Serializable]
public struct IntRange : IEquatable<IntRange>
{
    [UnityEngine.SerializeField]
    private int _min;
    public int Min
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
    private int _max;
    public int Max
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
    public int Random => UnityEngine.Random.Range(Min, Max+1);
    public int RandomExclusive => UnityEngine.Random.Range(Min, Max);

    public IntRange(int min, int max)
    {
        _min = (min > max) ? max : min;
        _max = (min > max) ? min : max;
    }

    public bool Equals(IntRange other)
    {
        return _min.Equals(other._min) && _max.Equals(other._max);
    }
    public override bool Equals(object obj)
    {
        return obj is IntRange other && Equals(other);
    }
    public override int GetHashCode()
    {
        FloatRange fr = new FloatRange();
        IntRange ir = (IntRange)fr;
        return HashCode.Combine(_min, _max);
    }

    public static explicit operator FloatRange(IntRange ir) => new FloatRange(ir._min, ir._max);
    public static implicit operator IntRange((int min, int max) tuple) => new IntRange(tuple.min, tuple.max);
}