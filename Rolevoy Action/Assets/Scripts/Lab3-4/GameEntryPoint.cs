using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    public static GameEntryPoint Instance { get; private set; }

    public IAudioService AudioService { get; private set; }
    public ISaveService SaveService { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitServices();
    }

    private void InitServices()
    {
        AudioService = new AudioService();
        SaveService = new PlayerPrefsSaveService();
    }
}