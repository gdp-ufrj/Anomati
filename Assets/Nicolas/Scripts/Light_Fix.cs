using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light_Fix : MonoBehaviour
{
    public GameObject lightObject;
    public Light2D[] lights;

    void LateUpdate()
    {
        if (lightObject.activeSelf)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i] != null)
                {
                    lights[i].intensity += 0.0001f;
                    lights[i].intensity -= 0.0001f;
                }
            }
        }
    }
}
