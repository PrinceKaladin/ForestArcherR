using UnityEngine;
using System.Collections;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject howToPlay;

    private float duration = 0.35f;
    private float offsetY = 120f;
    private Coroutine currentRoutine;

    private void Awake()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        settings.SetActive(false);
        howToPlay.SetActive(false);

        menu.SetActive(true);

        var canvasGroup = menu.GetComponent<CanvasGroup>();
        var rect = menu.GetComponent<RectTransform>();

        canvasGroup.alpha = 1f;
        rect.anchoredPosition = Vector2.zero;
    }

    public void ShowSettings()
    {
        menu.SetActive(false);
        howToPlay.SetActive(false);

        Animate(settings);
    }

    public void ShowHowToPlay()
    {
        menu.SetActive(false);
        settings.SetActive(false);

        Animate(howToPlay);
    }

    private void Animate(GameObject target)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(AnimateRoutine(target));
    }

    private IEnumerator AnimateRoutine(GameObject target)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        var rect = target.GetComponent<RectTransform>();

        Vector2 endPos = rect.anchoredPosition;
        Vector2 startPos = endPos + Vector2.up * offsetY;

        float t = 0f;

        target.SetActive(true);
        canvasGroup.alpha = 0f;
        rect.anchoredPosition = startPos;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            canvasGroup.alpha = smooth;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, smooth);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        rect.anchoredPosition = endPos;
    }
}
