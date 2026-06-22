using UnityEngine;

public class SettingsController
{
    private IAudioService audioService;

    public SettingsController(IAudioService audioService)
    {
        this.audioService = audioService;
    }

    public void SetVolume(float value)
    {
        audioService.SetVolume(value);
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public float GetCurrentVolume()
    {
        // Возвращаем сохраненную громкость для инициализации слайдера
        return PlayerPrefs.GetFloat("Volume", 0.5f);
    }
}