using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    private IHealth health;


    private void Start()
    {
        // »щем IHealth на родительском EnemyView или PlayerView
        var enemyView = GetComponentInParent<EnemyView>();
        if (enemyView != null && enemyView.Enemy != null)
        {
            Init(enemyView.Enemy.Health);
            return;
        }

        

        Debug.LogWarning("HealthBar: no IHealth found in parent!", this);
    }

    public void Init(IHealth health)
    {
        // отписка если переиспользуетс€
        if (this.health != null)
        {
            this.health.OnHealthChanged -= UpdateHealthBar;
            this.health.OnDeath -= OnDeathHandler;
        }

        this.health = health;

        slider.maxValue = health.Max;
        UpdateHealthBar(health.Current);

        health.OnHealthChanged += UpdateHealthBar;
        health.OnDeath += OnDeathHandler;
    }

    private void UpdateHealthBar(float current)
    {
        slider.value = current;

        if (colorGradient != null && health.Max > 0)
        {
            float normalized = current / health.Max;
            fillImage.color = colorGradient.Evaluate(normalized);
        }
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