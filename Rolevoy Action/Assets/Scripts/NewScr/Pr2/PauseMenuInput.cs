using UnityEngine;

public class PauseMenuInput : MonoBehaviour
{
    [SerializeField] private PauseMenuView view;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = view.root.activeSelf;

            view.root.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;
        }
    }
}