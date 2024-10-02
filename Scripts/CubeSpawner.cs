using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance;

    [SerializeField] private GameObject[] fruitPrefabs; // 12 farklý meyve prefab'ý
    [SerializeField] private GameObject bombPrefab; // Bomba prefab'ý
    private Queue<Cube> cubeQueue = new Queue<Cube>(); // Küp kuyruðu

    public static event System.Action OnCubeSpawned;  // Yeni bir küp spawnlandýðýnda tetiklenecek event

    [HideInInspector] public int maxCubeIndex = 11; // En büyük meyve index'i (0-11 arasý)

    private Cube lastSpawnedCube; // Son spawnlanan küp
    private bool isRandomSpawned = false; // Rastgele spawnlanma durumu

    private void Awake()
    {
        Instance = this;
        InitializeCubeQueue(); // Baþlangýçta kuyruk oluþturulur
    }

    // Küp kuyruðunu baþlatýr ve ilk küpleri oluþturur
    private void InitializeCubeQueue()
    {
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            GameObject cubeObject = Instantiate(fruitPrefabs[i]);
            Cube cube = cubeObject.GetComponent<Cube>();
            cube.gameObject.SetActive(false); // Baþlangýçta devre dýþý býrak
            cubeQueue.Enqueue(cube); // Kuyruða ekle
        }
    }

    // Küpü oluþturur ve index ayarlar
    public Cube Spawn(int index, Vector3 position, bool isRandom = false)
    {
        GameObject cubeObject = Instantiate(fruitPrefabs[index], position, fruitPrefabs[index].transform.rotation);
        Cube cube = cubeObject.GetComponent<Cube>();
        cube.SetIndex(index); // Hangi meyve seviyesinde olduðunu belirler

        if (isRandom)
        {
            lastSpawnedCube = cube;
            isRandomSpawned = true;
        }

        OnCubeSpawned?.Invoke();  // Yeni bir küp spawnlandýðýnda event'i tetikle

        return cube;
    }

    // Rastgele bir küp oluþturur
    public Cube SpawnRandom()
    {
        int randomIndex = GenerateRandomIndex(); // Rastgele meyve index'i üretir
        return Spawn(randomIndex, transform.position, true); // Rastgele spawn iþlemi olduðunu belirten bayraðý true yap
    }

    // Küpü yok eder ve kuyruða geri koyar
    public void DestroyCube(Cube cube)
    {
        cube.gameObject.SetActive(false); // Küpü inaktif hale getiririz
        cubeQueue.Enqueue(cube); // Küpü kuyruða geri ekleriz
    }

    // Ýlk 5 meyveden (index 0-4) rastgele bir küp index'i üretir
    private int GenerateRandomIndex()
    {
        return Random.Range(0, 5); // 0'dan 4'e kadar olan meyvelerden rastgele seçim
    }

    // Son spawnlanan küpü döndürür
    public Cube GetLastSpawnedCube()
    {
        return lastSpawnedCube;
    }

    // Bombayý spawnla ve kontrol edilebilir hale getir
    public Cube SpawnBomb()
    {
        if (lastSpawnedCube != null && isRandomSpawned) // Sadece rastgele spawnlanan küpler için geçerli
        {
            Vector3 position = lastSpawnedCube.transform.position;
            Destroy(lastSpawnedCube.gameObject); // Son spawnlanan küpü yok et
            GameObject bombObject = Instantiate(bombPrefab, position, bombPrefab.transform.rotation); // Bombayý yerine spawnla
            Cube bombCube = bombObject.AddComponent<Cube>(); // Bombayý kontrol edilebilir hale getir
            isRandomSpawned = false; // Bayraðý sýfýrla
            return bombCube; // Bombayý geri döndür
        }
        return null; // Eðer son spawnlanan küp yoksa veya rastgele deðilse null döndür
    }
}
