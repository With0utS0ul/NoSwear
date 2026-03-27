using UnityEngine;

public interface IHealth
{
    float Current { get; }
    float Max { get; }

    void Take(float value);
    void Heal(float value);
}