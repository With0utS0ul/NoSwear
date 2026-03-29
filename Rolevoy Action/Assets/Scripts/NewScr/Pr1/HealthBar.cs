using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private IHealth health;

    public void Init(IHealth health)
    {
        this.health = health;
        slider.maxValue = health.Max;
        slider.value = health.Current;

        health.OnHealthChanged += UpdateHealthBar;
        health.OnDeath += OnDeathHandler;
    }

    private void UpdateHealthBar(float current)
    {
        slider.value = current;
    }

    private void OnDeathHandler()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
            health.OnDeath -= OnDeathHandler;
        }
    }
}