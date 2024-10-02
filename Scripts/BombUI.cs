using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BombUI : MonoBehaviour
{
    private Image image;
    private Color originalColor = Color.white;
    private Color targetColor = new Color(1f, 0.18f, 0.18f); // FF2F2F rengi
    private Color transparentColor = new Color(1f, 1f, 1f, 0f); // Transparent beyaz
    private float colorChangeSpeed = 2f;
    private float timer = 0f;
    private bool isTransitioning = false;
    private bool isFadingOut = false;

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(ColorAnimationRoutine());
    }

    private IEnumerator ColorAnimationRoutine()
    {
        while (true)
        {
            // 2 saniye boyunca beyaz kal
            image.color = originalColor;
            yield return new WaitForSeconds(2f);

            // 3 saniye boyunca renk deðiþimini gerçekleþtir
            for (float t = 0f; t < 3f; t += 0.5f)
            {
                isTransitioning = true;
                yield return StartCoroutine(SmoothColorChange(targetColor, 0.25f)); // 0.5 saniyede bir deðiþim
                yield return StartCoroutine(SmoothColorChange(originalColor, 0.25f));
            }
            isTransitioning = false;

            // Görseli transparan yap
            isFadingOut = true;
            yield return StartCoroutine(SmoothColorChange(transparentColor, 1f)); // 2 saniyede transparan
            yield return new WaitForSeconds(2f);

            // Görseli geri getir
            yield return StartCoroutine(SmoothColorChange(originalColor, 1f));
            isFadingOut = false;
        }
    }

    private IEnumerator SmoothColorChange(Color newColor, float duration)
    {
        float elapsedTime = 0f;
        Color startingColor = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(startingColor, newColor, elapsedTime / duration);
            yield return null;
        }

        image.color = newColor;
    }
}
