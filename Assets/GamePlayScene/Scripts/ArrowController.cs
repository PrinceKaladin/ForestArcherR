using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ArrowController : MonoBehaviour
{
    public float flightTime = 1f;
    public float arcHeight = 50f;
    private float initialSpeed = 50f;

    public Sprite hitSprite;
    public float tipOffset = 30f;
    public float heightOffset = 0f;

    public GameObject scoreImagePrefab; 
    public Action OnArrowHit;

    private Vector2 targetPoint;
    private bool hitTarget = false;

    public void Launch(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 200f);

        hitTarget = false;

        foreach (var hit in hits)
        {
            TargetPoint tp = hit.collider.GetComponent<TargetPoint>();
            if (tp != null)
            {
                targetPoint = hit.point;
                targetPoint.y += heightOffset;
                hitTarget = true;
                break;
            }
        }

        if (!hitTarget)
        {
            targetPoint = (Vector2)transform.position + direction * 300f;
        }

        StartCoroutine(FlyArrow(direction));
    }

    IEnumerator FlyArrow(Vector2 direction)
    {
        Vector3 startPos = transform.position;

        float straightTime = 0.5f;
        float t = 0f;
        while (t < straightTime)
        {
            t += Time.deltaTime;
            transform.position += (Vector3)direction.normalized * initialSpeed * Time.deltaTime;
            transform.up = direction.normalized;
            yield return null;
        }

        Vector3 endPos = targetPoint;
        t = 0f;
        Vector3 previousPos = transform.position;
        float flightDur = flightTime;

        while (t < 1f)
        {
            t += Time.deltaTime / flightDur;

            Vector3 pos = Vector3.Lerp(transform.position, endPos, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * arcHeight;

            Vector3 frameDir = (pos - previousPos).normalized;
            if (frameDir != Vector3.zero)
                transform.up = frameDir;

            transform.position = pos;
            previousPos = pos;

            yield return null;
        }

        transform.position = endPos;

        if (hitTarget)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Vector3 toTarget = (endPos - startPos).normalized;
            transform.position -= toTarget * tipOffset;

            if (hitSprite != null)
                GetComponent<Image>().sprite = hitSprite;

            GameObject gameCanvas = GameObject.Find("GameCanvas");
            if (gameCanvas != null)
            {
                transform.SetParent(gameCanvas.transform, true);
                transform.localScale *= 0.5f;

                if (scoreImagePrefab != null)
                {
                    Instantiate(
                        scoreImagePrefab,
                        transform.position,
                        Quaternion.identity,
                        gameCanvas.transform
                    );
                }
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayArrowHit();
            }

            TryVibrate();

            GameManager.Instance.ArrowHit();
            OnArrowHit?.Invoke();
        }

        else
        {
            GameManager.Instance.ArrowMissed();
            Destroy(gameObject, 1f);
        }
    }

    public void SetHeightOffset(float offset)
    {
        heightOffset = offset;
    }

    void TryVibrate()
    {
        if (PlayerData.Instance == null)
            return;

        if (!PlayerData.Instance.VibrationOn)
            return;

#if UNITY_ANDROID || UNITY_IOS
    Handheld.Vibrate();
#endif
    }

}
