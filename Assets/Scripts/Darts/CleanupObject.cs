using UnityEngine;

// this script is created in dart with the AimDart script
public class CleanupObject : MonoBehaviour
{
    // default lifetime
    private float _lifetime = 15.0f;

    public void Init(float lifetime)
    {
        if (lifetime <= 0)
            _lifetime = 15.0f;
        else
            _lifetime = lifetime;

        // destroy G.O. after given lifetime
        Destroy(gameObject, _lifetime);
    }
}
