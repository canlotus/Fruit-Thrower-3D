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

        // E�er �arp��an nesnede Cube bile�eni yoksa, metottan ��k
        if (otherCube == null)
        {
            return; // Hata mesaj� vermeden ��k�� yap�yoruz.
        }

        // Ayn� index'e sahip meyveler �arp��t���nda
        if (otherCube != null && cube.CubeID > otherCube.CubeID)
        {
            if (cube.CubeIndex == otherCube.CubeIndex)
            {
                Vector3 contactPoint = collision.contacts[0].point;

                // E�er max index'ten k���kse bir �st seviyeye ge�
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

                    // Patlama efektini �al��t�r, renk belirtmeden
                    FX.Instance.PlayCubeExplosionFX(contactPoint);

                    // Patlama �evresindeki meyveleri etkilemeli
                    Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                    float explosionForce = 100f;
                    float explosionRadius = 1.5f;

                    foreach (Collider coll in surroundedCubes)
                    {
                        if (coll.attachedRigidbody != null)
                            coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                    }

                    // Puan� hesapla ve ScoreManager'a ekle
                    int scoreToAdd = (int)Mathf.Pow(2, newIndex); // 2^newIndex �eklinde puan� hesapla
                    ScoreManager.Instance?.AddScore(scoreToAdd);   // Puan� ScoreManager'a g�nder

                    // �arp��an k�pleri yok et
                    CubeSpawner.Instance.DestroyCube(cube);
                    CubeSpawner.Instance.DestroyCube(otherCube);
                }
            }
        }
    }
}
