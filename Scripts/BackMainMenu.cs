using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        // Ana men� sahnesini y�kle
        SceneManager.LoadScene("MainMenu");
    }
}
