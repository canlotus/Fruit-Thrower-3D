using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText; // Y�ksek skoru g�stermek i�in

    private void Start()
    {
        // PlayerPrefs'ten y�ksek skoru al ve g�ster
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Bu metot Play butonuna atanacak
    public void PlayGame()
    {
        // SampleScene adl� sahneye ge�i� yapar
        SceneManager.LoadScene("SampleScene");
    }

    // ��k�� butonu i�in bir metot (opsiyonel)
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
