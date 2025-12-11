using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour
{
    public enum TimeState { Day, Night }
    public TimeState currentState = TimeState.Day;

    public float dayDuration = 120f;
    public float nightDuration = 120f;

    public float transitionSpeed = 0.2f;

    public int dayCount = 1;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D[] sceneLights;
    [SerializeField] private TextMeshProUGUI dayText;

    private float timer;
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
        float targetIntensity = currentState == TimeState.Day ? 1f : 0.05f;
        globalLight.intensity = Mathf.MoveTowards(
            globalLight.intensity,
            targetIntensity,
            Time.deltaTime * transitionSpeed
        );

        if (Mathf.Abs(globalLight.intensity - targetIntensity) < 0.01f)
        {
            globalLight.intensity = targetIntensity;
            if (currentState == TimeState.Day)
                 SetAllLights(false);
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
}
