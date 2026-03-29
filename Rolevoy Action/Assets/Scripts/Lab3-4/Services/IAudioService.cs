using UnityEngine;

public interface IAudioService
{
    void PlayMusic(AudioClip clip);
    void SetVolume(float value);
}