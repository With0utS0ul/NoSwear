using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button backButton;

    // События, на которые будет подписываться Контроллер
    public event Action<float> OnVolumeChanged;
    public event Action OnBackClicked;

    public void Initialize(float initialVolume)
    {
        volumeSlider.value = initialVolume;

        // Привязываем внутренние UI-колбэки к публичным событиям
        volumeSlider.onValueChanged.AddListener(TriggerVolumeChanged);
        backButton.onClick.AddListener(TriggerBackClicked);
    }

    private void TriggerVolumeChanged(float value) => OnVolumeChanged?.Invoke(value);
    private void TriggerBackClicked() => OnBackClicked?.Invoke();

    private void OnDestroy()
    {
        // Очищаем подписки UI элементов
        volumeSlider.onValueChanged.RemoveListener(TriggerVolumeChanged);
        backButton.onClick.RemoveListener(TriggerBackClicked);
    }
}