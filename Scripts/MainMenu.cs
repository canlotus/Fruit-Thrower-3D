using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText; // Yüksek skoru göstermek için

    private void Start()
    {
        // PlayerPrefs'ten yüksek skoru al ve göster
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Bu metot Play butonuna atanacak
    public void PlayGame()
    {
        // SampleScene adlý sahneye geçiþ yapar
        SceneManager.LoadScene("SampleScene");
    }

    // Çýkýþ butonu için bir metot (opsiyonel)
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
