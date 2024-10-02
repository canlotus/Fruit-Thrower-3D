using UnityEngine;

public class BombSkill : MonoBehaviour
{
    public float explosionDelay = 2f; // Patlamadan önceki gecikme süresi
    public float explosionRadius = 1.5f; // Patlama yarýçapý
    public LayerMask cubeLayer; // Hangi layer'daki nesneler etkilenecek

    [SerializeField] private Material blinkMaterial; // Yanýp sönme efektinde kullanýlacak materyal
    private Material originalMaterial; // Bombanýn orijinal materyali
    private MeshRenderer meshRenderer;
    private float blinkInterval = 0.5f; // Yanýp sönme aralýðý
    private bool isMoving = false; // Bomba hareket etmeye baþladý mý?

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.materials[4]; // Orijinal materyali kaydet
            InvokeRepeating("Blink", 0f, blinkInterval); // Yanýp sönme efektini baþlat
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
        isMoving = true; // Bomba hareket etmeye baþladýðýnda patlama zamanlayýcýsýný baþlat

        // Oyuncunun bombayý kontrol etmesini engelle
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.DisableControl(); // Player script'inde kontrolü devre dýþý býrak
        }
    }

    private void Blink()
    {
        if (meshRenderer != null && blinkMaterial != null)
        {
            // Element4'ün rengini orijinal ve blinkMaterial arasýnda deðiþtir
            Material[] materials = meshRenderer.materials;
            materials[4] = materials[4] == originalMaterial ? blinkMaterial : originalMaterial;
            meshRenderer.materials = materials;
        }
    }

    private void Explode()
    {
        int totalScoreToAdd = 0; // Patlama sýrasýnda toplanacak toplam puan

        // Patlamanýn etki alanýndaki tüm nesneleri yok et ve puanlarýný topla
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, cubeLayer);
        foreach (Collider nearbyObject in colliders)
        {
            Cube cube = nearbyObject.GetComponent<Cube>();
            if (cube != null)
            {
                int cubeScore = (int)Mathf.Pow(2, cube.CubeIndex + 1); // Her meyvenin puanýný al
                totalScoreToAdd += cubeScore; // Toplam puana ekle
                CubeSpawner.Instance.DestroyCube(cube); // Meyveyi yok et
            }
        }

        // Toplanan puanlarý ScoreManager'a ekle
        if (totalScoreToAdd > 0)
        {
            ScoreManager.Instance.AddScore(totalScoreToAdd);
        }

        // Patlama efektini çalýþtýr
        ExplosionManager.Instance?.PlayExplosion(transform.position);

        // Son olarak, bombayý da yok et
        Destroy(gameObject);

        // Yeni bir küp spawnla (patlama sonrasý)
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.Invoke("SpawnNewCube", 0.3f);
            player.EnableControl(); // Kontrolü yeniden etkinleþtir
        }
    }
}
