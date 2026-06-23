using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Toggle peacefulModeToggle;

    // События для контроллера
    public event Action OnResumeClicked;
    public event Action OnMainMenuClicked;
    public event Action OnSaveClicked;
    public event Action OnLoadClicked;
    public event Action<bool> OnPeacefulToggled;

    public bool IsActive => root.activeSelf;

    private void Start()
    {
        

        // Подписываем внутренние UI элементы
        resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
        mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());
        saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
        loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        peacefulModeToggle.onValueChanged.AddListener(value => OnPeacefulToggled?.Invoke(value));
    }

    public void SetActive(bool isActive)
    {
        root.SetActive(isActive);

        if (isActive)
        {
            // Если меню открылось: освобождаем курсор и делаем его видимым
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Если меню закрылось: блокируем курсор в центре экрана и скрываем
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPeacefulToggleState(bool isOn)
    {
        if (peacefulModeToggle != null)
        {
            peacefulModeToggle.isOn = isOn;
        }
    }

    private void OnDestroy()
    {
        // View сама убирает за своими UI-компонентами
        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
        loadButton.onClick.RemoveAllListeners();
        peacefulModeToggle.onValueChanged.RemoveAllListeners();
    }
}