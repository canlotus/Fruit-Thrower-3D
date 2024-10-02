using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton pattern

    public TMP_Text totalScoreText;      // Toplam puaný gösterecek Text
    public TMP_Text addedScoreText;      // Eklenen puaný gösterecek Text
    public TMP_Text milestoneText;       // Eþik puan mesajýný gösterecek Text

    private int totalScore = 0;          // Toplam puan
    private int highScore = 0;           // En yüksek skor
    private int[] scoreMilestones = { 500, 2000, 5000, 10000, 50000, 100000 }; // Eþik puanlar
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

        // PlayerPrefs'ten en yüksek skoru al
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Puan ekleme fonksiyonu
    public void AddScore(int scoreToAdd)
    {
        Debug.Log($"Adding Score: {scoreToAdd}"); // Yeni eklenen satýr

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

        // Skor arttýðýnda ses çal
        SoundManager.Instance?.PlayScoreIncreaseSound();

        Invoke("HideAddedScore", 1.0f);
        CheckMilestone();
    }

    // Eklenen puaný gizle
    private void HideAddedScore()
    {
        addedScoreText.gameObject.SetActive(false);
    }

    // Eþik puanlarý kontrol et ve mesajý göster
    private void CheckMilestone()
    {
        for (int i = 0; i < scoreMilestones.Length; i++)
        {
            if (totalScore >= scoreMilestones[i] && totalScore < scoreMilestones[i] + 100) // Sadece bir kez göster
            {
                milestoneText.text = milestoneMessages[i];
                milestoneText.gameObject.SetActive(true);

                // 3 saniye sonra gizle
                Invoke("HideMilestoneText", 3.0f);
            }
        }
    }

    // Milestone mesajýný gizle
    private void HideMilestoneText()
    {
        milestoneText.gameObject.SetActive(false);
    }
}
