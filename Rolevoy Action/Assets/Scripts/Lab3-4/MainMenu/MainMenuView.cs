using UnityEngine;
using UnityEngine.UI;
using System;
public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    // Чистые события для Контроллера
    public event Action OnPlayClicked;
    public event Action OnSettingsClicked;
    public event Action OnExitClicked;

    private void Start()
    {
        playButton.onClick.AddListener(TriggerPlay);
        settingsButton.onClick.AddListener(TriggerSettings);
        exitButton.onClick.AddListener(TriggerExit);
    }

    private void TriggerPlay() => OnPlayClicked?.Invoke();
    private void TriggerSettings() => OnSettingsClicked?.Invoke();
    private void TriggerExit() => OnExitClicked?.Invoke();

    private void OnDestroy()
    {
        // View сама чистит за своими UI элементами
        playButton.onClick.RemoveListener(TriggerPlay);
        settingsButton.onClick.RemoveListener(TriggerSettings);
        exitButton.onClick.RemoveListener(TriggerExit);
    }
}