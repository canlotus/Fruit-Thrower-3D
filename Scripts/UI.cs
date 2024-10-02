using UnityEngine;

public class UI : MonoBehaviour
{
    public float rotationSpeed = 2.0f; // Rotasyon ba�lang�� h�z�
    public float maxRotation = 18.0f;  // Maksimum a�� (derece)
    public float minRotation = -15.0f; // Minimum a�� (derece)
    public float acceleration = 10.0f; // �vmelenme miktar�

    private RectTransform rectTransform;
    private bool rotatingForward = true;
    private float currentSpeed;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Image bile�eninin RectTransform'unu al�n
        currentSpeed = rotationSpeed; // Ba�lang�� h�z�n� ayarla
    }

    void Update()
    {
        // Mevcut z rotasyonunu al�n
        float currentZRotation = rectTransform.localEulerAngles.z;

        // Rotasyon derecesini 0-360 aras�nda s�n�rlamak i�in
        if (currentZRotation > 180)
        {
            currentZRotation -= 360;
        }

        // Rotasyon y�n�n� belirle ve h�z�n� ayarla
        if (rotatingForward)
        {
            currentSpeed += acceleration * Time.deltaTime; // �vmelenme ile h�z art�r
            currentZRotation += currentSpeed * Time.deltaTime;
            if (currentZRotation >= maxRotation)
            {
                currentZRotation = maxRotation;
                rotatingForward = false;
                currentSpeed = rotationSpeed; // H�z� s�f�rla
            }
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime; // �vmelenme ile h�z art�r
            currentZRotation -= currentSpeed * Time.deltaTime;
            if (currentZRotation <= minRotation)
            {
                currentZRotation = minRotation;
                rotatingForward = true;
                currentSpeed = rotationSpeed; // H�z� s�f�rla
            }
        }

        // Yeni z rotasyonunu uygulay�n
        rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, currentZRotation);
    }
}
