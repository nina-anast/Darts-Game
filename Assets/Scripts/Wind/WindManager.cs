using UnityEngine;
using UnityEngine.UI;

public class WindManager : MonoBehaviour
{
    [Header("Wind Settings")]
    public Vector3 windDirection = Vector3.zero; // Current wind direction
    public float windAmplitude = 2f;            // Current wind strength
    public Transform dartboard;

    [Header("Wind Change Settings")]
    public float directionChangeRate = 2.5f;    // How much the wind direction changes per second
    public float amplitudeChangeRate = 1f;    // How much the wind amplitude changes per second
    public float minAmplitude = 1f;           // Minimum wind strength
    public float maxAmplitude = 5f;          // Maximum wind strength

    [Header("UI Elements")]
    public RectTransform windArrow;           // Wind arrow UI element
    public Text amplitudeText;                // Text to display amplitude

    private void Start()
    {
        // Initialize wind direction and amplitude
        windDirection = GenerateRandomDirection();
        windAmplitude = Random.Range(minAmplitude, maxAmplitude);

        UpdateWindIndicator();
    }

    private void Update()
    {
        // Gradually change wind direction
        windDirection = Vector3.Lerp(
            windDirection,
            GenerateRandomDirection(),
            directionChangeRate * Time.deltaTime
        ).normalized;

        // Gradually change wind amplitude
        windAmplitude = Mathf.MoveTowards(
            windAmplitude,
            Random.Range(minAmplitude, maxAmplitude),
            amplitudeChangeRate * Time.deltaTime
        );

        // Update wind indicator
        UpdateWindIndicator();

        // Debug visualization in the scene
        Debug.DrawRay(Vector3.zero, windDirection * windAmplitude, Color.blue);
    }


    private void UpdateWindIndicator()
    {
        if (windArrow != null)
        {

            float angle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;

            windArrow.rotation = Quaternion.Euler(0, 0, angle);

            float normalizedAmplitude = Mathf.InverseLerp(minAmplitude, maxAmplitude, windAmplitude);

            windArrow.localScale = new Vector3(1f, normalizedAmplitude, 1f);
        }

        if (amplitudeText != null)
        {

            amplitudeText.text = $"Wind: {windAmplitude:F1}";
        }
    }


    private Vector3 GenerateRandomDirection()
    {
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0f 
        ).normalized;
    }


    public Vector3 GetWindForce()
    {
        return windDirection * windAmplitude;
    }
}
