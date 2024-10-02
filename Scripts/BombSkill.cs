using UnityEngine;

public class BombSkill : MonoBehaviour
{
    public float explosionDelay = 2f; // Patlamadan �nceki gecikme s�resi
    public float explosionRadius = 1.5f; // Patlama yar��ap�
    public LayerMask cubeLayer; // Hangi layer'daki nesneler etkilenecek

    [SerializeField] private Material blinkMaterial; // Yan�p s�nme efektinde kullan�lacak materyal
    private Material originalMaterial; // Bomban�n orijinal materyali
    private MeshRenderer meshRenderer;
    private float blinkInterval = 0.5f; // Yan�p s�nme aral���
    private bool isMoving = false; // Bomba hareket etmeye ba�lad� m�?

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.materials[4]; // Orijinal materyali kaydet
            InvokeRepeating("Blink", 0f, blinkInterval); // Yan�p s�nme efektini ba�lat
        }
    }

    private void Update()
    {
        if (isMoving && explosionDelay > 0f)
        {
            explosionDelay -= Time.deltaTime;
            if (explosionDelay <= 0f)
            {
                Explode();
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true; // Bomba hareket etmeye ba�lad���nda patlama zamanlay�c�s�n� ba�lat

        // Oyuncunun bombay� kontrol etmesini engelle
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.DisableControl(); // Player script'inde kontrol� devre d��� b�rak
        }
    }

    private void Blink()
    {
        if (meshRenderer != null && blinkMaterial != null)
        {
            // Element4'�n rengini orijinal ve blinkMaterial aras�nda de�i�tir
            Material[] materials = meshRenderer.materials;
            materials[4] = materials[4] == originalMaterial ? blinkMaterial : originalMaterial;
            meshRenderer.materials = materials;
        }
    }

    private void Explode()
    {
        int totalScoreToAdd = 0; // Patlama s�ras�nda toplanacak toplam puan

        // Patlaman�n etki alan�ndaki t�m nesneleri yok et ve puanlar�n� topla
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, cubeLayer);
        foreach (Collider nearbyObject in colliders)
        {
            Cube cube = nearbyObject.GetComponent<Cube>();
            if (cube != null)
            {
                int cubeScore = (int)Mathf.Pow(2, cube.CubeIndex + 1); // Her meyvenin puan�n� al
                totalScoreToAdd += cubeScore; // Toplam puana ekle
                CubeSpawner.Instance.DestroyCube(cube); // Meyveyi yok et
            }
        }

        // Toplanan puanlar� ScoreManager'a ekle
        if (totalScoreToAdd > 0)
        {
            ScoreManager.Instance.AddScore(totalScoreToAdd);
        }

        // Patlama efektini �al��t�r
        ExplosionManager.Instance?.PlayExplosion(transform.position);

        // Son olarak, bombay� da yok et
        Destroy(gameObject);

        // Yeni bir k�p spawnla (patlama sonras�)
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.Invoke("SpawnNewCube", 0.3f);
            player.EnableControl(); // Kontrol� yeniden etkinle�tir
        }
    }
}
