using UnityEngine;
using UnityEngine.UI;

public class CampfireFuelBar : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset;
    public Color low;
    public Color high;

    public void SetFuel(float fuel, float maxFuel)
    {
        slider.value = fuel;
        slider.maxValue = maxFuel;
        slider.fillRect.GetComponentInChildren<Image>().color =
            Color.Lerp(low, high, slider.normalizedValue);
    }

    private void Update()
    {
        slider.transform.position =
            Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
