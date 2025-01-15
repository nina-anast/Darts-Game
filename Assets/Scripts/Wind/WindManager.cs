using UnityEngine;
using UnityEngine.UI;

public class WindManager : MonoBehaviour
{
    [Header("Wind Settings")]
    public Vector3 windDirection = Vector3.zero; // Current wind direction
    public float windAmplitude = 5f;            // Current wind strength

    [Header("Wind Change Settings")]
    public float directionChangeRate = 5f;    // How much the wind direction changes per second
    public float amplitudeChangeRate = 5f;    // How much the wind amplitude changes per second
    public float minAmplitude = 5f;             // Minimum wind strength
    public float maxAmplitude = 20f;            // Maximum wind strength

    [Header("UI Elements")]
    public RectTransform windArrow;             // Wind arrow UI element
    public Text amplitudeText;                  // Text to display amplitude

    private void Start()
    {
        // Initialize wind direction and amplitude
        windDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0f // Ensure wind stays in the horizontal plane
        ).normalized;

        windAmplitude = Random.Range(minAmplitude, maxAmplitude);
    }

    private void Update()
    {
        // Gradually change wind direction
        windDirection = new Vector3(
            windDirection.x + Random.Range(-directionChangeRate, directionChangeRate) * Time.deltaTime,
            windDirection.y + Random.Range(-directionChangeRate, directionChangeRate) * Time.deltaTime,
            0f
        ).normalized;

        // Gradually change wind amplitude
        windAmplitude += Random.Range(-amplitudeChangeRate, amplitudeChangeRate) * Time.deltaTime;
        windAmplitude = Mathf.Clamp(windAmplitude, minAmplitude, maxAmplitude);

        // Update wind indicator
        UpdateWindIndicator();

        // Debug visualization
        Debug.DrawRay(Vector3.zero, windDirection * windAmplitude, Color.blue);
    }

    /// <summary>
    /// Updates the wind arrow and amplitude text in the UI.
    /// </summary>
    private void UpdateWindIndicator()
    {
        if (windArrow != null)
        {
            // Set the rotation of the wind arrow
            float angle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            windArrow.rotation = Quaternion.Euler(0, 0, angle);

            // Scale the wind arrow based on amplitude
            float normalizedAmplitude = Mathf.InverseLerp(minAmplitude, maxAmplitude, windAmplitude);
            windArrow.localScale = new Vector3(1f, normalizedAmplitude, 1f);
        }

        if (amplitudeText != null)
        {
            // Update the amplitude text
            amplitudeText.text = $"Wind: {windAmplitude:F1}";
        }
    }

    /// <summary>
    /// Returns the current wind force.
    /// </summary>
    public Vector3 GetWindForce()
    {
        return windDirection * windAmplitude;
    }
}
