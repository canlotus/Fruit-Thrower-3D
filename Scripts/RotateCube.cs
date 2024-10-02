using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 10f; // X eksenindeki dönüþ hýzý
    [SerializeField] private float rotationSpeedY = 10f; // Y eksenindeki dönüþ hýzý

    void Update()
    {
        // Küpü X ve Y eksenlerinde döndür
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, 0f);
    }
}
