using UnityEngine;

public class UI : MonoBehaviour
{
    public float rotationSpeed = 2.0f; // Rotasyon baþlangýç hýzý
    public float maxRotation = 18.0f;  // Maksimum açý (derece)
    public float minRotation = -15.0f; // Minimum açý (derece)
    public float acceleration = 10.0f; // Ývmelenme miktarý

    private RectTransform rectTransform;
    private bool rotatingForward = true;
    private float currentSpeed;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Image bileþeninin RectTransform'unu alýn
        currentSpeed = rotationSpeed; // Baþlangýç hýzýný ayarla
    }

    void Update()
    {
        // Mevcut z rotasyonunu alýn
        float currentZRotation = rectTransform.localEulerAngles.z;

        // Rotasyon derecesini 0-360 arasýnda sýnýrlamak için
        if (currentZRotation > 180)
        {
            currentZRotation -= 360;
        }

        // Rotasyon yönünü belirle ve hýzýný ayarla
        if (rotatingForward)
        {
            currentSpeed += acceleration * Time.deltaTime; // Ývmelenme ile hýz artýr
            currentZRotation += currentSpeed * Time.deltaTime;
            if (currentZRotation >= maxRotation)
            {
                currentZRotation = maxRotation;
                rotatingForward = false;
                currentSpeed = rotationSpeed; // Hýzý sýfýrla
            }
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime; // Ývmelenme ile hýz artýr
            currentZRotation -= currentSpeed * Time.deltaTime;
            if (currentZRotation <= minRotation)
            {
                currentZRotation = minRotation;
                rotatingForward = true;
                currentSpeed = rotationSpeed; // Hýzý sýfýrla
            }
        }

        // Yeni z rotasyonunu uygulayýn
        rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, currentZRotation);
    }
}
