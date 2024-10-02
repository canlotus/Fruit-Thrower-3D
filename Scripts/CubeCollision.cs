using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    Cube cube;

    private void Awake()
    {
        cube = GetComponent<Cube>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Cube otherCube = collision.gameObject.GetComponent<Cube>();

        // Eðer çarpýþan nesnede Cube bileþeni yoksa, metottan çýk
        if (otherCube == null)
        {
            return; // Hata mesajý vermeden çýkýþ yapýyoruz.
        }

        // Ayný index'e sahip meyveler çarpýþtýðýnda
        if (otherCube != null && cube.CubeID > otherCube.CubeID)
        {
            if (cube.CubeIndex == otherCube.CubeIndex)
            {
                Vector3 contactPoint = collision.contacts[0].point;

                // Eðer max index'ten küçükse bir üst seviyeye geç
                if (cube.CubeIndex < CubeSpawner.Instance.maxCubeIndex)
                {
                    int newIndex = cube.CubeIndex + 1;

                    // Yeni meyveyi spawnla
                    Cube newCube = CubeSpawner.Instance.Spawn(newIndex, contactPoint + Vector3.up * 1.6f);

                    // Fiziksel kuvvet uygula
                    newCube.CubeRigidbody.AddForce(Vector3.up * 2f, ForceMode.Impulse);

                    // Tork ekle
                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);

                    // Patlama efektini çalýþtýr, renk belirtmeden
                    FX.Instance.PlayCubeExplosionFX(contactPoint);

                    // Patlama çevresindeki meyveleri etkilemeli
                    Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                    float explosionForce = 100f;
                    float explosionRadius = 1.5f;

                    foreach (Collider coll in surroundedCubes)
                    {
                        if (coll.attachedRigidbody != null)
                            coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                    }

                    // Puaný hesapla ve ScoreManager'a ekle
                    int scoreToAdd = (int)Mathf.Pow(2, newIndex); // 2^newIndex þeklinde puaný hesapla
                    ScoreManager.Instance?.AddScore(scoreToAdd);   // Puaný ScoreManager'a gönder

                    // Çarpýþan küpleri yok et
                    CubeSpawner.Instance.DestroyCube(cube);
                    CubeSpawner.Instance.DestroyCube(otherCube);
                }
            }
        }
    }
}
