using UnityEngine.SceneManagement;

public class SettingsController
{
    private readonly IAudioService _audioService;
    private readonly SettingsModel _model;
    private readonly SettingsView _view;

    public SettingsController(IAudioService audioService, SettingsModel model, SettingsView view)
    {
        _audioService = audioService;
        _model = model;
        _view = view;

        // Загружаем данные в модель
        _model.Load();

        // Инициализируем View текущими данными из модели
        _view.Initialize(_model.Volume);

        // Актуализируем состояние аудиосистемы при старте
        _audioService.SetVolume(_model.Volume);

        // Подписываемся на действия пользователя во View
        _view.OnVolumeChanged += HandleVolumeChanged;
        _view.OnBackClicked += HandleBackClicked;
    }

    private void HandleVolumeChanged(float value)
    {
        // Обновляем модель и сохраняем
        _model.Volume = value;
        _model.Save();

        // Обновляем аудио-сервис
        _audioService.SetVolume(value);
    }

    private void HandleBackClicked()
    {
        // Отписываемся перед сменой сцены
        _view.OnVolumeChanged -= HandleVolumeChanged;
        _view.OnBackClicked -= HandleBackClicked;

        SceneManager.LoadScene("MainMenu");
    }
}