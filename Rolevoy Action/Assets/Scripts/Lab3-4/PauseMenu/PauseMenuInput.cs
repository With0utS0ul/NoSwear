using UnityEngine;

public class PauseMenuInput : MonoBehaviour
{
    private PauseMenuController _controller;

    // —юда мы передаем созданный контроллер (например, из инициализатора сцены игры)
    public void Initialize(PauseMenuController controller)
    {
        _controller = controller;
    }

    void Update()
    {
        if (_controller == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ѕередаем управление в контроллер
            _controller.TogglePause();
        }
    }
}