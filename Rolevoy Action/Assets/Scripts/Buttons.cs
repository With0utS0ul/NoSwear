using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("ВыхоД");
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
