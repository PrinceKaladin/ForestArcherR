using UnityEngine;
using System.Collections;

public class MenuCanvasController : MonoBehaviour
{
    public GameObject Background;
    public GameObject PauseMenu;
    public GameObject Settings;
    public GameObject GameOver;

    public float duration = 0.5f;
    public float offsetY = 100f;

    private Coroutine currentRoutine;

    public void ShowPauseMenu()
    {
        HideMenusExceptBackground();
        Animate(PauseMenu);
        Time.timeScale = 0f;
    }

    public void ShowSettings()
    {
        HideMenusExceptBackground();
        Animate(Settings);
    }

    public void ShowGameOver()
    {
        HideMenusExceptBackground();
        Animate(GameOver);
    }

    private void HideMenusExceptBackground()
    {
        Background.SetActive(true);
        PauseMenu.SetActive(false);
        Settings.SetActive(false);
        GameOver.SetActive(false);
    }

    public void HideAllMenus()
    {
        Background.SetActive(false);
        PauseMenu.SetActive(false);
        Settings.SetActive(false);
        GameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Animate(GameObject target)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(AnimateRoutine(target));
    }

    private IEnumerator AnimateRoutine(GameObject target)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        RectTransform rect = target.GetComponent<RectTransform>();

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
