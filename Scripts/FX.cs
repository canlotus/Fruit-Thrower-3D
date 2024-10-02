using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private ParticleSystem cubeExplosionFX;

    // Singleton class
    public static FX Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayCubeExplosionFX(Vector3 position)
    {
        cubeExplosionFX.transform.position = position;
        cubeExplosionFX.Play();
    }
}
