using UnityEngine;

public class AudioService : IAudioService
{
    private AudioSource source;

    public AudioService()
    {
        var go = new GameObject("AudioService");
        source = go.AddComponent<AudioSource>();
        GameObject.DontDestroyOnLoad(go);


        // Включаем зацикливание для фоновой музыки
        source.loop = true;

        // При старте игры загружаем сохраненную громкость (если её нет, то 0.5f по дефолту)
        source.volume = PlayerPrefs.GetFloat("GameVolume", 0.5f);
    }

    public void PlayMusic(AudioClip clip)
    {
        // Проверка, чтобы не перезапускать один и тот же трек по кругу при перезагрузке сцены
        if (source.clip == clip) return;

        source.clip = clip;
        source.Play();
    }

    public void SetVolume(float value)
    {
        source.volume = value;
        // Сохраняем настройки в память устройства, чтобы они не сбрасывались при перезапуске игры
        PlayerPrefs.SetFloat("GameVolume", value);
        PlayerPrefs.Save();
    }
}