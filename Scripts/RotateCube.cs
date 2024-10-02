using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float rotationSpeedX = 10f; // X eksenindeki d�n�� h�z�
    [SerializeField] private float rotationSpeedY = 10f; // Y eksenindeki d�n�� h�z�

    void Update()
    {
        // K�p� X ve Y eksenlerinde d�nd�r
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, 0f);
    }
}
