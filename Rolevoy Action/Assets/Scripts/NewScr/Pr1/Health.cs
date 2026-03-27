using UnityEngine;

public class Health : IHealth
{
    public float Current { get; private set; }
    public float Max { get; private set; }

    public Health(float max)
    {
        Max = max;
        Current = max;
    }

    public void Take(float value)
    {
        Current -= value;
        if (Current < 0) Current = 0;
    }

    public void Heal(float value)
    {
        Current = Mathf.Min(Current + value, Max);
    }
}