using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; // Ayarlar paneli
    [SerializeField] private Button openSettingsButton; // Ayarlarý açma butonu
    [SerializeField] private Button closeSettingsButton; // Ayarlarý kapatma butonu
    [SerializeField] private Button muteButton; // Sesi kapatma butonu
    [SerializeField] private Button unmuteButton; // Sesi açma butonu

    [SerializeField] private Color activeColor = Color.gray; // Aktif buton rengi
    [SerializeField] private Color defaultColor = Color.white; // Varsayýlan buton rengi

    private void Start()
    {
        // Sesi kontrol etmek için baþlangýç ayarlarýný yükle
        bool isSoundOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        ApplySoundSettings(isSoundOn);

        // Butonlarýn týklanma olaylarýný dinle
        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
        muteButton.onClick.AddListener(MuteSound);
        unmuteButton.onClick.AddListener(UnmuteSound);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true); // Ayarlar panelini aç
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); // Ayarlar panelini kapat
    }

    public void MuteSound()
    {
        ApplySoundSettings(false);
        PlayerPrefs.SetInt("SoundEnabled", 0); // Ayarý kaydet
    }

    public void UnmuteSound()
    {
        ApplySoundSettings(true);
        PlayerPrefs.SetInt("SoundEnabled", 1); // Ayarý kaydet
    }

    private void ApplySoundSettings(bool isSoundOn)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSound(isSoundOn);
        }

        // Buton renklerini ayarla
        muteButton.image.color = isSoundOn ? defaultColor : activeColor;
        unmuteButton.image.color = isSoundOn ? activeColor : defaultColor;
    }

    private void OnDestroy()
    {
        // Butonlarýn dinleyicilerini kaldýr
        openSettingsButton.onClick.RemoveListener(OpenSettings);
        closeSettingsButton.onClick.RemoveListener(CloseSettings);
        muteButton.onClick.RemoveListener(MuteSound);
        unmuteButton.onClick.RemoveListener(UnmuteSound);
    }
}
