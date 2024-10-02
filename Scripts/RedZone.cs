using UnityEngine;
using System.Collections;

public class RedZone : MonoBehaviour
{
    private float timeInZone = 0f; // Tetikleyici alanýnda geçen süre
    private bool isCubeInZone = false; // Küp tetikleyici alanda mý?
    private Renderer redZoneRenderer; // RedZone'un Renderer'ý
    private Color originalColor; // RedZone'un orijinal rengi
    private Coroutine colorChangeCoroutine; // Renk deðiþtirme coroutine'i

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
                FindObjectOfType<GameManager>().GameOver(); // GameManager'dan GameOver çaðrýlýr
                ResetZoneTimer(); // Zamanlayýcýyý sýfýrla
            }
            else if (colorChangeCoroutine == null)
            {
                // Eðer renk deðiþim coroutine'i baþlamadýysa baþlat
                colorChangeCoroutine = StartCoroutine(ChangeColorOverTime());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube != null && !cube.IsMainCube)
        {
            ResetZoneTimer(); // Zamanlayýcýyý sýfýrla ve renk deðiþimini durdur
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Color targetColor = new Color(255f / 255f, originalColor.g, originalColor.b); // Hedef renk (R = 255)

        while (true)
        {
            // R deðerini 110'dan 255'e çýkar
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                Color newColor = Color.Lerp(originalColor, targetColor, t);
                redZoneRenderer.material.color = newColor;
                yield return null;
            }

            elapsedTime = 0f;

            // R deðerini 255'ten 110'a düþür
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

    // Zamanlayýcýyý sýfýrlayan fonksiyon
    private void ResetZoneTimer()
    {
        timeInZone = 0f;
        isCubeInZone = false;

        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine); // Renk deðiþim coroutine'ini durdur
            colorChangeCoroutine = null;
        }

        redZoneRenderer.material.color = originalColor; // Rengi orijinal haline döndür
    }
}
