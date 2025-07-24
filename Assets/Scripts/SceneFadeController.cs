using UnityEngine;
using System.Threading.Tasks;
public class SceneFadeController : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        // Make sure we start with fade invisible
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public async Task FadeOutAsync()
    {
        if (fadeCanvasGroup == null) return;

        fadeCanvasGroup.blocksRaycasts = true;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            await Task.Yield();
        }

        fadeCanvasGroup.alpha = 1;
    }

    public async Task FadeInAsync()
    {
        if (fadeCanvasGroup == null) return;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsed / fadeDuration);
            await Task.Yield();
        }

        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}
