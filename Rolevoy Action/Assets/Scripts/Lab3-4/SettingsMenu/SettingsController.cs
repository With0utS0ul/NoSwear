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
    }
}