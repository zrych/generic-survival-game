using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public enum TimeState { Day, Night }
    public TimeState currentState = TimeState.Day;

    public float dayDuration = 120f;
    public float nightDuration = 120f;

    public float transitionSpeed = 0.2f;

    public int dayCount = 1;

    public static DayNightController Instance;

    [SerializeField] private CanvasGroup nightOverlay;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D[] sceneLights;
    [SerializeField] private TextMeshProUGUI dayText;

    private float timer;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SetAllLights(false);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (currentState == TimeState.Day && timer >= dayDuration) CycleToNight();
        if (currentState == TimeState.Night && timer >= nightDuration) CycleToDay();
        UpdateLighting();
    }

    void CycleToNight()
    {
        currentState = TimeState.Night;
        timer = 0f;
        SetAllLights(true);
        BeginNight();
    }
    void CycleToDay()
    {
        currentState = TimeState.Day;
        timer = 0f;
        dayCount++;
        dayText.text = $"Day {dayCount}";
        SetAllLights(false);
        BeginDay();
    }
    void UpdateLighting()
    {
        globalLight.color = new Color(0.6f, 0.7f, 1f, 1f);
        float targetIntensity = currentState == TimeState.Day ? 1f : 0.05f;
        globalLight.intensity = Mathf.MoveTowards(
            globalLight.intensity,
            targetIntensity,
            Time.deltaTime * transitionSpeed
        );
        StartCoroutine(FadeNight(true));

        if (Mathf.Abs(globalLight.intensity - targetIntensity) < 0.01f)
        {
            globalLight.intensity = targetIntensity;
            if (currentState == TimeState.Day)
            {
                globalLight.color = Color.white;
                SetAllLights(false);
                StartCoroutine(FadeNight(false));
            }

        }
    }

    private void SetAllLights(bool isOn)
    {
        foreach (Light2D light in sceneLights) { light.enabled = isOn; }
    }

    private void BeginNight()
    {
        MonsterSpawnManager.Instance.NightStarted();
    }

    private void BeginDay()
    {
        MonsterSpawnManager.Instance.DayStarted();
    }

    private IEnumerator FadeNight(bool isNight)
    {
        float target = isNight ? 0.65f : 0f;
        while (Mathf.Abs(nightOverlay.alpha - target) > 0.01f)
        {
            nightOverlay.alpha = Mathf.Lerp(nightOverlay.alpha, target, Time.deltaTime * fadeSpeed);
            yield return null;
        }
    }
}
