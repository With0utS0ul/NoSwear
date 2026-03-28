using UnityEngine;

public class AudioService : IAudioService
{
    private AudioSource source;

    public AudioService()
    {
        var go = new GameObject("AudioService");
        source = go.AddComponent<AudioSource>();
        GameObject.DontDestroyOnLoad(go);
    }

    public void PlayMusic(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void SetVolume(float value)
    {
        source.volume = value;
    }
}