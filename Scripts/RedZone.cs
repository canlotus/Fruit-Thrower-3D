using UnityEngine;
using System.Collections;

public class RedZone : MonoBehaviour
{
    private float timeInZone = 0f; // Tetikleyici alan�nda ge�en s�re
    private bool isCubeInZone = false; // K�p tetikleyici alanda m�?
    private Renderer redZoneRenderer; // RedZone'un Renderer'�
    private Color originalColor; // RedZone'un orijinal rengi
    private Coroutine colorChangeCoroutine; // Renk de�i�tirme coroutine'i

    private void Start()
    {
        redZoneRenderer = GetComponent<Renderer>();
        originalColor = redZoneRenderer.material.color; // Orijinal rengi sakla
    }

    private void OnTriggerStay(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube != null && !cube.IsMainCube)
        {
            isCubeInZone = true;
            timeInZone += Time.deltaTime;

            if (timeInZone >= 2f)
            {
                Debug.Log("Gameover");
                FindObjectOfType<GameManager>().GameOver(); // GameManager'dan GameOver �a�r�l�r
                ResetZoneTimer(); // Zamanlay�c�y� s�f�rla
            }
            else if (colorChangeCoroutine == null)
            {
                // E�er renk de�i�im coroutine'i ba�lamad�ysa ba�lat
                colorChangeCoroutine = StartCoroutine(ChangeColorOverTime());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube != null && !cube.IsMainCube)
        {
            ResetZoneTimer(); // Zamanlay�c�y� s�f�rla ve renk de�i�imini durdur
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Color targetColor = new Color(255f / 255f, originalColor.g, originalColor.b); // Hedef renk (R = 255)

        while (true)
        {
            // R de�erini 110'dan 255'e ��kar
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                Color newColor = Color.Lerp(originalColor, targetColor, t);
                redZoneRenderer.material.color = newColor;
                yield return null;
            }

            elapsedTime = 0f;

            // R de�erini 255'ten 110'a d���r
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                Color newColor = Color.Lerp(targetColor, originalColor, t);
                redZoneRenderer.material.color = newColor;
                yield return null;
            }

            elapsedTime = 0f;
        }
    }

    // Zamanlay�c�y� s�f�rlayan fonksiyon
    private void ResetZoneTimer()
    {
        timeInZone = 0f;
        isCubeInZone = false;

        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine); // Renk de�i�im coroutine'ini durdur
            colorChangeCoroutine = null;
        }

        redZoneRenderer.material.color = originalColor; // Rengi orijinal haline d�nd�r
    }
}
