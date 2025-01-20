using TMPro;
using UnityEngine;
// this script is created in dart with the AimDart script
// if the following components don't exist, it creates them
[RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
public class DartMovement : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float Force;
    private WindManager WindManager;
    public TextMeshProUGUI LastShot;
    public TextMeshProUGUI Multi;
    private AudioSource _audioSource;

    // gets data as inputs from AimDart
    public void Init(float force, WindManager windManager, TextMeshProUGUI lastShot, TextMeshProUGUI multi)
    {
        Force = force;
        WindManager = windManager;
        LastShot = lastShot;
        Multi = multi;

        // gets signed angle of dart h z axis
        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, transform.right);

        // gets rigit body to detect collision with constant static mesh geometry
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // adds rotation according to calculated angle
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.right);
        // adds force to rigid body with rotation
        // force looks in the same direction as dart 
        // it affects its speed
        Rigidbody.AddForce(rotation * (Force * Vector3.forward), ForceMode.VelocityChange);

        // gets collider
        GetComponent<MeshCollider>().convex = true;
        // gets audio source
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // if wind manager exists
        if (WindManager != null)
        {
            // Get the wind force from the WindManager
            Vector3 windForce = WindManager.GetWindForce();

            // Apply the wind force to the dart
            Rigidbody.AddForce(windForce, ForceMode.Force);
        }

        // Rotate the dart to align with its velocity direction
        // by default it is choosed to be false so there is a rotational speed
        RotateTowardsDirection();
    }


    private void RotateTowardsDirection(bool immediate = false)
    {
        // if rigit body speed is not 0
        if (Rigidbody.linearVelocity != Vector3.zero)
        {
            // get rotation due to gravity
            Quaternion targetRotation = Quaternion.LookRotation(Rigidbody.linearVelocity.normalized, Vector3.up);

            if (immediate)
            {
                // to change immediately rotation
                transform.rotation = targetRotation;
            }
            else
            {
                // to add rotational speed
                float angle = Vector3.Angle(transform.forward, Rigidbody.linearVelocity.normalized);
                float lerpFactor = angle * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpFactor);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // when collision occurs, add sound
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        // if collision is not with dartboard, update texts
        if (collision.gameObject.name != "Dartboard")
        {
            LastShot.text = "Last Shot: 0";
            Multi.text = "";
        }
        // destroy dart movement and rigid body
        Destroy(this);
        Destroy(Rigidbody);
    }
}