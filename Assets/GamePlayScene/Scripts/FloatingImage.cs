using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingImage : MonoBehaviour
{
    public float floatDistance = 50f;
    public float floatTime = 0.7f;    

    private RectTransform rect;
    private Image img;
    private CanvasGroup cg;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        StartCoroutine(FloatUp());
    }

    IEnumerator FloatUp()
    {
        Vector3 startPos = rect.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / floatTime;
            rect.position = Vector3.Lerp(startPos, endPos, t);
            cg.alpha = 1f - t; 
            yield return null;
        }

        Destroy(gameObject);
    }
}
