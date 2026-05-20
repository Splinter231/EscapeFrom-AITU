using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight;
    public float cycleSpeed = 0.5f;

    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.15f, 0.1f, 0.25f);

    private float timer;

    void Update()
    {
        timer += Time.deltaTime * cycleSpeed;

        float t = (Mathf.Sin(timer) + 1f) / 2f;

        globalLight.color = Color.Lerp(nightColor, dayColor, t);
        globalLight.intensity = Mathf.Lerp(0.25f, 0.9f, t);
    }
}