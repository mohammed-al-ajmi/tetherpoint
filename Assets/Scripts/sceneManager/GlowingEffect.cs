using UnityEngine;

public class GlowingEffect : MonoBehaviour
{
    public Material targetMaterial;
    public Color glowColor = Color.green; // Glow color
    public float glowSpeed = 0.2f; // Speed 
    public float maxGlowIntensity = 0.2f;
    public float transparency = 0.3f;

    private void Start()
    {
        if (targetMaterial != null)
        {
            Color baseColor = targetMaterial.color;
            baseColor.a = transparency;
            targetMaterial.color = baseColor;
        }
    }

    private void Update()
    {
        if (targetMaterial != null)
        {
            float emissionIntensity = 0.05f * Mathf.PingPong(Time.time * glowSpeed, maxGlowIntensity);

            // Cycle through colors using a sine wave for smooth transitions
            float t = Mathf.PingPong(Time.time * glowSpeed, 0.3f); // Normalized value between 0 and 1
            Color dynamicColor = Color.Lerp(Color.green, Color.yellow, t); // Transition between red and blue

            // Combine the color with the intensity
            Color finalEmissionColor = dynamicColor * Mathf.LinearToGammaSpace(emissionIntensity);

            // Apply to the material
            targetMaterial.SetColor("_EmissionColor", finalEmissionColor);

        }
    }
}
