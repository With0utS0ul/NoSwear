using UnityEngine;
using UnityEngine.UI;
public class PauseMenuView : MonoBehaviour
{
    public GameObject root;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button saveButton;
    public Button loadButton;
    public Toggle peacefulModeToggle;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}