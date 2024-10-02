using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Eyes : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private float moveDistance = 30f;
    private float moveDuration = 2f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        StartCoroutine(MoveImage());
    }

    private IEnumerator MoveImage()
    {
        while (true)
        {
            // X ekseninde saða hareket et
            yield return StartCoroutine(MoveToPosition(originalPosition + new Vector2(moveDistance, 0), moveDuration));

            // 2 saniye bekle
            yield return new WaitForSeconds(moveDuration);

            // X ekseninde sola geri hareket et
            yield return StartCoroutine(MoveToPosition(originalPosition, moveDuration));

            // 2 saniye bekle
            yield return new WaitForSeconds(moveDuration);
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition, float duration)
    {
        Vector2 startingPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }
}
