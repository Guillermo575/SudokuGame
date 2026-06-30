using UnityEngine;
using TMPro;
using System.Collections;
public class TextDisplayController : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public float displayDuration = 5f;
    public float fadeDuration = 1f;
    private Coroutine currentCoroutine;
    public void OnEnable()
    {
        SetAlpha(0f);
    }
    public void ShowText(string message)
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI no asignado.");
            return;
        }
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(DisplayAndFadeCoroutine(message));
    }
    private IEnumerator DisplayAndFadeCoroutine(string message)
    {
        textMeshPro.text = message;
        SetAlpha(1f);
        yield return new WaitForSeconds(displayDuration);
        float elapsed = 0f;
        Color originalColor = textMeshPro.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(0f);
    }
    private void SetAlpha(float alpha)
    {
        if (textMeshPro == null) return;
        Color color = textMeshPro.color;
        color.a = alpha;
        textMeshPro.color = color;
    }
}