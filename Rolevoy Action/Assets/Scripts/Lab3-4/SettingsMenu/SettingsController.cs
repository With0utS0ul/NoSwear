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

        _model.Load();
        _view.Initialize(_model.Volume);
        _audioService.SetVolume(_model.Volume);
        _view.OnVolumeChanged += HandleVolumeChanged;
        _view.OnBackClicked += HandleBackClicked;
    }

    private void HandleVolumeChanged(float value)
    {
        _model.Volume = value;
        _model.Save();
        _audioService.SetVolume(value);
    }

    private void HandleBackClicked()
    {
        Dispose();

        SceneManager.LoadScene("MainMenu");
    }

    public void Dispose()
    {
        _view.OnVolumeChanged -= HandleVolumeChanged;
        _view.OnBackClicked -= HandleBackClicked;
    }
}