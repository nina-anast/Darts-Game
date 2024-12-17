using UnityEngine;
using UnityEngine.UIElements;

public class DartMovement : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float Force, Angle;

    public float InteractRadius;
    public LayerMask InteractableLayer;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();

        Quaternion rotation = Quaternion.AngleAxis(Angle, transform.right);
        Rigidbody.AddForce(rotation * (Force * Vector3.forward), ForceMode.VelocityChange);

        Debug.Log("Game Started");
    }

    private void FixedUpdate()
    {
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
                float lerpFactor = angle * Time.deltaTime; // Use the angle as the interpolation factor
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpFactor);
            }
        }
    }

    // if it hits the floor it should stop moving
    private void OnCollisionEnter(Collision collision)
    {
        // find possible colliders
        Collider[] interactables = Physics.OverlapSphere(transform.position, InteractRadius, InteractableLayer);

        // if it doesn't find the floor within the radius continue
        if (interactables.Length == 0) return;

        // else print the message
        Debug.Log("dart hit the floor.");

        // set velocity to zero
        Rigidbody.linearVelocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;  
    }
}
