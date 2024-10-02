using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        // Ana menü sahnesini yükle
        SceneManager.LoadScene("MainMenu");
    }
}
