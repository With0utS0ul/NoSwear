using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MainMenuView view;

    private void Awake()
    {
        new MainMenuController(view);

        var audioService = GameEntryPoint.Instance?.AudioService;
        if (audioService != null)
        {
            AudioClip gameMusic = Resources.Load<AudioClip>("Music/GameBackground"); // ”кажи свой путь в Resources
            if (gameMusic != null)
                audioService.PlayMusic(gameMusic);
        }
    }

    private void Start()
    {
        var audioService = GameEntryPoint.Instance?.AudioService;
        if (audioService != null)
        {
            AudioClip menuMusic = Resources.Load<AudioClip>("Music/MenuBackground"); // ”кажи свой путь
            if (menuMusic != null)
                audioService.PlayMusic(menuMusic);
        }
    }
}