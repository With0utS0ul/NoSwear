using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button backButton;

    private IAudioService audioService;

    private void Start()
    {
        audioService = GameEntryPoint.Instance.AudioService;

        // Загружаем сохранённую громкость (опционально)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = savedVolume;
        audioService.SetVolume(savedVolume);

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        backButton.onClick.AddListener(OnBack);
    }

    private void OnVolumeChanged(float value)
    {
        audioService.SetVolume(value);
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void OnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}