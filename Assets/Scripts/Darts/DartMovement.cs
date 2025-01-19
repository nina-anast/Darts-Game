using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
public class DartMovement : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float Force;
    private WindManager WindManager;
    public TextMeshProUGUI LastShot;
    public TextMeshProUGUI Multi;
    private AudioSource _audioSource;

    public void Init(float force, WindManager windManager, TextMeshProUGUI lastShot, TextMeshProUGUI multi)
    {
        Force = force;
        WindManager = windManager;
        LastShot = lastShot;
        Multi = multi;

        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, transform.right);

        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.right);
        Rigidbody.AddForce(rotation * (Force * Vector3.forward), ForceMode.VelocityChange);

        GetComponent<MeshCollider>().convex = true;
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (WindManager != null)
        {
            // Get the wind force from the WindManager
            Vector3 windForce = WindManager.GetWindForce();

            // Apply the wind force to the dart
            Rigidbody.AddForce(windForce, ForceMode.Force);
        }

        // Rotate the dart to align with its velocity direction
        RotateTowardsDirection();
    }


    private void RotateTowardsDirection(bool immediate = false)
    {
        if (Rigidbody.linearVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Rigidbody.linearVelocity.normalized, Vector3.up);

            if (immediate)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                float angle = Vector3.Angle(transform.forward, Rigidbody.linearVelocity.normalized);
                float lerpFactor = angle * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpFactor);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        if (collision.gameObject.name != "Dartboard")
        {
            LastShot.text = "Last Shot: 0";
            Multi.text = "";
        }
        Destroy(this);
        Destroy(Rigidbody);
    }
}