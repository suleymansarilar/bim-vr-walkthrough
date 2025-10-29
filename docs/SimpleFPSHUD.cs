using UnityEngine;
using UnityEngine.UI;

public class SimpleFPSHUD : MonoBehaviour
{
    public Text fpsText;
    float smoothed;

    void Update()
    {
        float fps = 1f / Mathf.Max(Time.unscaledDeltaTime, 1e-4f);
        smoothed = Mathf.Lerp(smoothed, fps, 0.1f);
        if (fpsText) fpsText.text = $"FPS: {smoothed:0}";
    }
}