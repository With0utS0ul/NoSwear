using UnityEngine;

public class MainMenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MainMenuView view;

    private void Awake()
    {
        new MainMenuController(view);
    }
}