using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    private IHealth health;

    public void Init(IHealth health)
    {
        this.health = health;
        slider.maxValue = health.Max;
        slider.value = health.Current;
        if (colorGradient != null)
            fillImage.color = colorGradient.Evaluate(health.Current / health.Max);

        health.OnHealthChanged += UpdateHealthBar;
        health.OnDeath += OnDeathHandler;
    }

    private void UpdateHealthBar(float current)
    {
        slider.value = current;
        if (colorGradient != null)
            fillImage.color = colorGradient.Evaluate(current / health.Max);
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