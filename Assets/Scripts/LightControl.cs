using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public Light2D lightGlobal;
    public float lightIntensity;

    void OnEnable()
    {
        lightGlobal.intensity = lightIntensity;
    }
}
