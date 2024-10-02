using UnityEngine;

public class GecisliOyunLoad : MonoBehaviour
{
    [SerializeField] private int loadAdAfterSpawns = 20;  // Kaç meyve spawnlandýðýnda reklam yüklenecek
    [SerializeField] private int showAdAfterSpawns = 50;  // Kaç meyve spawnlandýðýnda reklam gösterilecek
    private int spawnCount = 0;  // Þu ana kadar spawnlanan meyve sayýsý

    private GecislOyun gecisliOyun;  // GecislOyun scriptine referans

    private void Start()
    {
        gecisliOyun = GetComponent<GecislOyun>();  // Ayný GameObject'te bulunan GecislOyun scriptine referans al

        CubeSpawner.OnCubeSpawned += OnCubeSpawned;  // Evente abone ol
    }

    private void OnDestroy()
    {
        CubeSpawner.OnCubeSpawned -= OnCubeSpawned;  // Eventten çýk
    }

    private void OnCubeSpawned()
    {
        spawnCount++;  // Meyve spawn sayýsýný arttýr

        if (spawnCount == loadAdAfterSpawns)  // Eðer yeterli sayýda meyve spawnlandýysa
        {
            gecisliOyun.LoadInterstitialAd();  // Reklamý yükle
        }

        if (spawnCount == showAdAfterSpawns)  // Eðer reklam gösterme zamaný geldiyse
        {
            gecisliOyun.ShowInterstitialAd();  // Reklamý göster
            spawnCount = 0;  // Spawn sayýsýný sýfýrla
        }
    }
}
