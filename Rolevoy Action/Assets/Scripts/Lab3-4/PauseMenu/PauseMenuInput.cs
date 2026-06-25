using UnityEngine;

public class PauseMenuInput : MonoBehaviour
{
    private PauseMenuController _controller;
    public void Initialize(PauseMenuController controller)
    {
        _controller = controller;
    }

    void Update()
    {
        if (_controller == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _controller.TogglePause();
        }
    }
}