using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton pattern

    public TMP_Text totalScoreText;      // Toplam puan� g�sterecek Text
    public TMP_Text addedScoreText;      // Eklenen puan� g�sterecek Text
    public TMP_Text milestoneText;       // E�ik puan mesaj�n� g�sterecek Text

    private int totalScore = 0;          // Toplam puan
    private int highScore = 0;           // En y�ksek skor
    private int[] scoreMilestones = { 500, 2000, 5000, 10000, 50000, 100000 }; // E�ik puanlar
    private string[] milestoneMessages = { "Good", "Nice", "Awesome", "Wonderful", "Incredible", "Fantastic" }; // Mesajlar

    private void Awake()
    {
        // Singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // PlayerPrefs'ten en y�ksek skoru al
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Puan ekleme fonksiyonu
    public void AddScore(int scoreToAdd)
    {
        Debug.Log($"Adding Score: {scoreToAdd}"); // Yeni eklenen sat�r

        totalScore += scoreToAdd;
        totalScoreText.text = "" + totalScore.ToString();

        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        addedScoreText.text = "+" + scoreToAdd.ToString();
        addedScoreText.gameObject.SetActive(true);

        // Skor artt���nda ses �al
        SoundManager.Instance?.PlayScoreIncreaseSound();

        Invoke("HideAddedScore", 1.0f);
        CheckMilestone();
    }

    // Eklenen puan� gizle
    private void HideAddedScore()
    {
        addedScoreText.gameObject.SetActive(false);
    }

    // E�ik puanlar� kontrol et ve mesaj� g�ster
    private void CheckMilestone()
    {
        for (int i = 0; i < scoreMilestones.Length; i++)
        {
            if (totalScore >= scoreMilestones[i] && totalScore < scoreMilestones[i] + 100) // Sadece bir kez g�ster
            {
                milestoneText.text = milestoneMessages[i];
                milestoneText.gameObject.SetActive(true);

                // 3 saniye sonra gizle
                Invoke("HideMilestoneText", 3.0f);
            }
        }
    }

    // Milestone mesaj�n� gizle
    private void HideMilestoneText()
    {
        milestoneText.gameObject.SetActive(false);
    }
}
