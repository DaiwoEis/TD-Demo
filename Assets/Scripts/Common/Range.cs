public class Range
{
    private readonly float _min;
    private readonly float _max;

    public Range(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public bool WithInRange(float value)
    {
        return value >= _min && value <= _max;
    }
}