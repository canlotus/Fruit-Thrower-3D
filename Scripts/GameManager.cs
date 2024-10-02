using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // GameOverPanel'i ba�lamak i�in
    [SerializeField] private TouchSlider touchSlider; // TouchSlider referans�

    private void Start()
    {
        // Ba�lang��ta paneli kapal� tut
        gameOverPanel.SetActive(false);
        touchSlider.enabled = true; // Ba�lang��ta touchSlider etkin

        // Oyun ba�lad���nda zaman �l�e�ini s�f�rdan ��kart
        Time.timeScale = 1; // Bu sat�r, oyunun tekrar ba�lad���nda normal h�zda �al��mas�n� sa�lar
    }

    public void GameOver()
    {
        // Game Over oldu�unda paneli aktif et
        gameOverPanel.SetActive(true);
        touchSlider.enabled = false; // Game over oldu�unda touchSlider'� devre d��� b�rak
    }

    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu called");
        Time.timeScale = 1; // Ana men�ye d�nerken zaman �l�e�ini s�f�rdan ��kar
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitToMainMenu()
    {
        // Ana men�ye d�n
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
