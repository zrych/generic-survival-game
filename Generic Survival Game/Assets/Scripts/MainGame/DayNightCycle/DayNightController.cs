using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI; // make sure to use UnityEngine.UI

public class DayNightController : MonoBehaviour
{
    public enum TimeState { Day, Night, BloodMoon }
    public TimeState currentState = TimeState.Day;

    public float dayDuration = 120f;
    public float nightDuration = 120f;
    public float transitionSpeed = 0.2f;
    public int dayCount = 1;

    public static DayNightController Instance;

    [SerializeField] private CanvasGroup nightOverlay;
    [SerializeField] private UnityEngine.UI.Image overlayColor;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D[] sceneLights;
    [SerializeField] private TextMeshProUGUI dayText;

    [SerializeField] GameObject waveSpawnZone;

    private Color ogNightColor;
    private Color bloodMoonColor = new Color(0.6f, 0.1f, 0.1f, 0.4f);

    private float timer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (overlayColor != null)
            ogNightColor = overlayColor.color;

        if (sceneLights != null)
        {
            foreach (Light2D light in sceneLights)
            {
                if (light != null) light.enabled = false;
            }
        }

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMusic("Morning", 1f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (currentState == TimeState.Day && timer >= dayDuration) CycleToNight();
        if ((currentState == TimeState.Night || currentState == TimeState.BloodMoon) && timer >= nightDuration) CycleToDay();
        UpdateLighting();
    }

    void CycleToNight()
    {
        if (dayCount % 7 == 0)
        {
            currentState = TimeState.BloodMoon;
            BeginBloodMoon();
            if (MusicManager.Instance != null)
                MusicManager.Instance.PlayMusic("BloodMoon", 1f);
        }
        else
        {
            currentState = TimeState.Night;
            if (MusicManager.Instance != null)
                MusicManager.Instance.PlayMusic("Night", 1f);
        }

        timer = 0f;
        SetAllLights(true);
        BeginNight();
    }

    void CycleToDay()
    {
        if (overlayColor != null)
            overlayColor.color = ogNightColor;

        currentState = TimeState.Day;
        timer = 0f;
        dayCount++;
        if (dayText != null)
            dayText.text = $"Day {dayCount}";

        SetAllLights(false);
        BeginDay();

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayMusic("Morning", 1f);
    }

    private void SetAllLights(bool isOn)
    {
        if (sceneLights != null)
        {
            foreach (Light2D light in sceneLights)
            {
                if (light != null)
                    light.enabled = isOn;
            }
        }
    }

    private void BeginNight()
    {
        if (MonsterSpawnManager.Instance != null)
            MonsterSpawnManager.Instance.NightStarted();
    }

    private void BeginDay()
    {
        if (MonsterSpawnManager.Instance != null)
            MonsterSpawnManager.Instance.DayStarted();
    }

    private void UpdateLighting()
    {
        if (globalLight == null) return;

        globalLight.color = new Color(0.6f, 0.7f, 1f, 1f);
        float targetIntensity = currentState == TimeState.Day ? 1f : 0.05f;
        globalLight.intensity = Mathf.MoveTowards(globalLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);

        if (nightOverlay != null)
            StartCoroutine(FadeNight(currentState != TimeState.Day));

        if (Mathf.Abs(globalLight.intensity - targetIntensity) < 0.01f)
        {
            globalLight.intensity = targetIntensity;
            if (currentState == TimeState.Day)
            {
                globalLight.color = Color.white;
                SetAllLights(false);
                if (nightOverlay != null)
                    StartCoroutine(FadeNight(false));
            }
        }
    }

    private IEnumerator FadeNight(bool isNight)
    {
        if (nightOverlay == null) yield break;

        float target = isNight ? 0.65f : 0f;
        while (Mathf.Abs(nightOverlay.alpha - target) > 0.01f)
        {
            nightOverlay.alpha = Mathf.Lerp(nightOverlay.alpha, target, Time.deltaTime * fadeSpeed);
            yield return null;
        }
        nightOverlay.alpha = target;
    }

    private void BeginBloodMoon()
    {
        if (overlayColor != null)
            overlayColor.color = bloodMoonColor;

        if (MonsterSpawnManager.Instance != null)
            MonsterSpawnManager.Instance.NightStarted();

        if (waveSpawnZone != null)
            waveSpawnZone.SetActive(true);
    }
}
