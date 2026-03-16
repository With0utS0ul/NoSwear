using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthComp))]
public class HealthUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Camera worldCamera;

    [Header("Settings")]
    [SerializeField] private bool faceCamera = true;

    private HealthComp _health;
    private Camera _mainCam;

    private void Awake()
    {
        _health = GetComponent<HealthComp>();
        _mainCam = worldCamera ?? Camera.main;

        if (canvasTransform != null)
        {
            var canvas = canvasTransform.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = _mainCam;
            }
        }

        if (hpSlider != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = _health.MaxHP;
            hpSlider.value = _health.CurHP;
        }
    }

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateSlider;
        _health.OnDeath += HandleDeathVisuals;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= UpdateSlider;
        _health.OnDeath -= HandleDeathVisuals;
    }

    private void UpdateSlider(float current, float max)
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = max;
            hpSlider.value = current;
        }
    }

    private void HandleDeathVisuals()
    {
        if (hpSlider != null)
        {
            hpSlider.value = 0;
        }
    }

    private void LateUpdate()
    {
        if (faceCamera && canvasTransform != null && _mainCam != null)
        {
            canvasTransform.rotation = Quaternion.LookRotation(
                _mainCam.transform.position - canvasTransform.position);
        }
    }
}