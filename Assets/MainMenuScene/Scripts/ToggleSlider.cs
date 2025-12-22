using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleSlider : MonoBehaviour
{
    public enum ToggleType { Music, Sound, Vibration }

    [SerializeField] private ToggleType toggleType;
    [SerializeField] private Slider slider;
    [SerializeField] private Image handleImage;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private GameObject textOn;
    [SerializeField] private GameObject textOff;
    [SerializeField] private float smoothTime = 0.15f;

    private Coroutine moveRoutine;
    private bool isAnimating;

    private void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.wholeNumbers = false;

        slider.onValueChanged.AddListener(OnSliderChanged);

        slider.value = GetValue() ? 1f : 0f;
        UpdateVisuals(slider.value == 1f);
    }

    public void Toggle()
    {
        if (isAnimating)
            return;

        float target = slider.value >= 0.5f ? 0f : 1f;

        moveRoutine = StartCoroutine(SmoothMove(target));
    }

    private IEnumerator SmoothMove(float target)
    {
        isAnimating = true;

        float start = slider.value;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / smoothTime;
            slider.value = Mathf.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        slider.value = target;

        bool isOn = target == 1f;
        SetValue(isOn);
        UpdateVisuals(isOn);

        isAnimating = false;
    }

    private void OnSliderChanged(float value)
    {
        if (isAnimating)
            return;

        bool isOn = value >= 0.5f;
        slider.value = isOn ? 1f : 0f;

        SetValue(isOn);
        UpdateVisuals(isOn);
    }

    private bool GetValue()
    {
        return toggleType switch
        {
            ToggleType.Music => PlayerData.Instance.MusicOn,
            ToggleType.Sound => PlayerData.Instance.SoundOn,
            ToggleType.Vibration => PlayerData.Instance.VibrationOn,
            _ => false
        };
    }

    private void SetValue(bool value)
    {
        switch (toggleType)
        {
            case ToggleType.Music:
                PlayerData.Instance.MusicOn = value;
                break;
            case ToggleType.Sound:
                PlayerData.Instance.SoundOn = value;
                break;
            case ToggleType.Vibration:
                PlayerData.Instance.VibrationOn = value;
                break;
        }
    }

    private void UpdateVisuals(bool isOn)
    {
        handleImage.sprite = isOn ? activeSprite : inactiveSprite;
        textOn.SetActive(isOn);
        textOff.SetActive(!isOn);
    }
}
