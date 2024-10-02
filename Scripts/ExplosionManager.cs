using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect; // Patlama efektinin particle sistemi

    // Singleton class
    public static ExplosionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Patlama efektini belirtilen konumda çalýþtýr
    public void PlayExplosion(Vector3 position)
    {
        if (explosionEffect != null)
        {
            explosionEffect.transform.position = position;
            explosionEffect.Play();
        }
        else
        {
            Debug.LogWarning("Patlama efekti atanmamýþ!");
        }
    }
}
