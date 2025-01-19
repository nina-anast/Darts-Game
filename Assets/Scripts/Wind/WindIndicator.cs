using UnityEngine;
using UnityEngine.UI;

public class WindIndicator : MonoBehaviour
{
    public RectTransform arrow; // Reference to the wind arrow RectTransform
    public WindManager windManager; // Reference to the WindManager

    private void Start()
    {
        if (windManager == null)
        {
            windManager = Object.FindFirstObjectByType<WindManager>();
        }
    }

    private void Update()
    {
        if (windManager != null && arrow != null)
        {
            // Get the wind direction and amplitude
            Vector3 windDirection = windManager.windDirection;
            float windAmplitude = windManager.windAmplitude;

            // Set arrow rotation based on wind direction
            float angle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            arrow.localRotation = Quaternion.Euler(0, 0, angle);

        }
    }
}
