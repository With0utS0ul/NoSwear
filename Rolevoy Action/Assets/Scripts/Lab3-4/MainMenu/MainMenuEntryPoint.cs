using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MainMenuView view;
    [SerializeField] private AudioClip gameBackgroundMusic;

    private MainMenuController _controller;

    private void Awake()
    {
        _controller = new MainMenuController(view);

        var audioService = GameEntryPoint.Instance?.AudioService;
        if (audioService != null && gameBackgroundMusic != null)
        {
            audioService.PlayMusic(gameBackgroundMusic);
        }
    }

    private void OnDestroy()
    {
        _controller?.Dispose();
    }


}