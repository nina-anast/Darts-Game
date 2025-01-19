using UnityEngine;

public class WindAffectedObject : MonoBehaviour
{
    public Rigidbody rb; // Reference to the object's Rigidbody
    private WindManager windManager;

    private void Start()
    {
        // Find the WindManager in the scene
        windManager = Object.FindFirstObjectByType<WindManager>();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if (windManager != null && rb != null)
        {
            // Apply the wind force to the object
            Vector3 windForce = windManager.GetWindForce();
            rb.AddForce(windForce, ForceMode.Force);
        }
    }
}
