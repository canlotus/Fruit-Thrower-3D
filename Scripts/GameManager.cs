using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // GameOverPanel'i baðlamak için
    [SerializeField] private TouchSlider touchSlider; // TouchSlider referansý

    private void Start()
    {
        // Baþlangýçta paneli kapalý tut
        gameOverPanel.SetActive(false);
        touchSlider.enabled = true; // Baþlangýçta touchSlider etkin

        // Oyun baþladýðýnda zaman ölçeðini sýfýrdan çýkart
        Time.timeScale = 1; // Bu satýr, oyunun tekrar baþladýðýnda normal hýzda çalýþmasýný saðlar
    }

    public void GameOver()
    {
        // Game Over olduðunda paneli aktif et
        gameOverPanel.SetActive(true);
        touchSlider.enabled = false; // Game over olduðunda touchSlider'ý devre dýþý býrak
    }

    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu called");
        Time.timeScale = 1; // Ana menüye dönerken zaman ölçeðini sýfýrdan çýkar
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitToMainMenu()
    {
        // Ana menüye dön
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
