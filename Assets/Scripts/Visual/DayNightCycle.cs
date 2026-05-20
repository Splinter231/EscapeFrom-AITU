using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("2D Global Light")]
    public Light2D globalLight;

    [Header("Cycle Settings")]
    public float cycleSpeed = 0.02f;

    [Header("Light Colors")]
    public Color dayColor = new Color(1f, 0.95f, 0.8f);
    public Color nightColor = new Color(0.08f, 0.1f, 0.22f);

    [Header("Light Intensity")]
    public float dayIntensity = 0.85f;
    public float nightIntensity = 0.18f;

    private float timer;

    void Update()
    {
        if (globalLight == null)
        {
            return;
        }

        timer += Time.deltaTime * cycleSpeed;

        float cycleValue = (Mathf.Sin(timer) + 1f) / 2f;

        globalLight.color = Color.Lerp(nightColor, dayColor, cycleValue);
        globalLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, cycleValue);
    }
}