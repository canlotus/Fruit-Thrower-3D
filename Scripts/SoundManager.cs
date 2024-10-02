using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    // Skor arttýðýnda çalacak ses dosyasý
    public AudioClip scoreIncreaseClip;

    private void Awake()
    {
        // Singleton instance atamasý
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ses yönetimi her sahnede kalsýn
            audioSource = GetComponent<AudioSource>();

            // Ses ayarlarýný yükle ve uygula
            bool isSoundOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
            SetSound(isSoundOn);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSound(bool isSoundOn)
    {
        audioSource.mute = !isSoundOn;
    }

    // Belirli bir ses dosyasýný çal
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && !audioSource.mute)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Skor arttýðýnda çalacak ses
    public void PlayScoreIncreaseSound()
    {
        PlaySound(scoreIncreaseClip);
    }
}
