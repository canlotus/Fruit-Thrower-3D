using UnityEngine;

public class GecisliOyunLoad : MonoBehaviour
{
    [SerializeField] private int loadAdAfterSpawns = 20;  // Ka� meyve spawnland���nda reklam y�klenecek
    [SerializeField] private int showAdAfterSpawns = 50;  // Ka� meyve spawnland���nda reklam g�sterilecek
    private int spawnCount = 0;  // �u ana kadar spawnlanan meyve say�s�

    private GecislOyun gecisliOyun;  // GecislOyun scriptine referans

    private void Start()
    {
        gecisliOyun = GetComponent<GecislOyun>();  // Ayn� GameObject'te bulunan GecislOyun scriptine referans al

        CubeSpawner.OnCubeSpawned += OnCubeSpawned;  // Evente abone ol
    }

    private void OnDestroy()
    {
        CubeSpawner.OnCubeSpawned -= OnCubeSpawned;  // Eventten ��k
    }

    private void OnCubeSpawned()
    {
        spawnCount++;  // Meyve spawn say�s�n� artt�r

        if (spawnCount == loadAdAfterSpawns)  // E�er yeterli say�da meyve spawnland�ysa
        {
            gecisliOyun.LoadInterstitialAd();  // Reklam� y�kle
        }

        if (spawnCount == showAdAfterSpawns)  // E�er reklam g�sterme zaman� geldiyse
        {
            gecisliOyun.ShowInterstitialAd();  // Reklam� g�ster
            spawnCount = 0;  // Spawn say�s�n� s�f�rla
        }
    }
}
