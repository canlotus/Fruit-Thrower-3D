using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    // Skor artt���nda �alacak ses dosyas�
    public AudioClip scoreIncreaseClip;

    private void Awake()
    {
        // Singleton instance atamas�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ses y�netimi her sahnede kals�n
            audioSource = GetComponent<AudioSource>();

            // Ses ayarlar�n� y�kle ve uygula
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

    // Belirli bir ses dosyas�n� �al
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && !audioSource.mute)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Skor artt���nda �alacak ses
    public void PlayScoreIncreaseSound()
    {
        PlaySound(scoreIncreaseClip);
    }
}
