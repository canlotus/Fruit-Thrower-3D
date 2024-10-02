using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance;

    [SerializeField] private GameObject[] fruitPrefabs; // 12 farkl� meyve prefab'�
    [SerializeField] private GameObject bombPrefab; // Bomba prefab'�
    private Queue<Cube> cubeQueue = new Queue<Cube>(); // K�p kuyru�u

    public static event System.Action OnCubeSpawned;  // Yeni bir k�p spawnland���nda tetiklenecek event

    [HideInInspector] public int maxCubeIndex = 11; // En b�y�k meyve index'i (0-11 aras�)

    private Cube lastSpawnedCube; // Son spawnlanan k�p
    private bool isRandomSpawned = false; // Rastgele spawnlanma durumu

    private void Awake()
    {
        Instance = this;
        InitializeCubeQueue(); // Ba�lang��ta kuyruk olu�turulur
    }

    // K�p kuyru�unu ba�lat�r ve ilk k�pleri olu�turur
    private void InitializeCubeQueue()
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            GameObject cubeObject = Instantiate(fruitPrefabs[i]);
            Cube cube = cubeObject.GetComponent<Cube>();
            cube.gameObject.SetActive(false); // Ba�lang��ta devre d��� b�rak
            cubeQueue.Enqueue(cube); // Kuyru�a ekle
        }
    }

    // K�p� olu�turur ve index ayarlar
    public Cube Spawn(int index, Vector3 position, bool isRandom = false)
    {
        GameObject cubeObject = Instantiate(fruitPrefabs[index], position, fruitPrefabs[index].transform.rotation);
        Cube cube = cubeObject.GetComponent<Cube>();
        cube.SetIndex(index); // Hangi meyve seviyesinde oldu�unu belirler

        if (isRandom)
        {
            lastSpawnedCube = cube;
            isRandomSpawned = true;
        }

        OnCubeSpawned?.Invoke();  // Yeni bir k�p spawnland���nda event'i tetikle

        return cube;
    }

    // Rastgele bir k�p olu�turur
    public Cube SpawnRandom()
    {
        int randomIndex = GenerateRandomIndex(); // Rastgele meyve index'i �retir
        return Spawn(randomIndex, transform.position, true); // Rastgele spawn i�lemi oldu�unu belirten bayra�� true yap
    }

    // K�p� yok eder ve kuyru�a geri koyar
    public void DestroyCube(Cube cube)
    {
        cube.gameObject.SetActive(false); // K�p� inaktif hale getiririz
        cubeQueue.Enqueue(cube); // K�p� kuyru�a geri ekleriz
    }

    // �lk 5 meyveden (index 0-4) rastgele bir k�p index'i �retir
    private int GenerateRandomIndex()
    {
        return Random.Range(0, 5); // 0'dan 4'e kadar olan meyvelerden rastgele se�im
    }

    // Son spawnlanan k�p� d�nd�r�r
    public Cube GetLastSpawnedCube()
    {
        return lastSpawnedCube;
    }

    // Bombay� spawnla ve kontrol edilebilir hale getir
    public Cube SpawnBomb()
    {
        if (lastSpawnedCube != null && isRandomSpawned) // Sadece rastgele spawnlanan k�pler i�in ge�erli
        {
            Vector3 position = lastSpawnedCube.transform.position;
            Destroy(lastSpawnedCube.gameObject); // Son spawnlanan k�p� yok et
            GameObject bombObject = Instantiate(bombPrefab, position, bombPrefab.transform.rotation); // Bombay� yerine spawnla
            Cube bombCube = bombObject.AddComponent<Cube>(); // Bombay� kontrol edilebilir hale getir
            isRandomSpawned = false; // Bayra�� s�f�rla
            return bombCube; // Bombay� geri d�nd�r
        }
        return null; // E�er son spawnlanan k�p yoksa veya rastgele de�ilse null d�nd�r
    }
}
