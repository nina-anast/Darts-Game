using UnityEngine;
using UnityEngine.InputSystem;

public class AimDart : MonoBehaviour
{
    public float DistanceFromCamera = 10.0f;
    public GameObject DartPrefab;
    private GameObject _dartInstace;
    private Camera _cam;

    public float RotationSpeed = 10.0f;
    [SerializeField]
    private float _force = 50.0f;

    private float _rotAngle;
    private Vector3 _currentDartPos;

    private void Start()
    {
        // initial position of dart
        var mousePosAction = InputSystem.actions.FindAction("Point");
        mousePosAction.performed += Aim;

        _cam = Camera.main;

        // rotation of dart
        var move = InputSystem.actions.FindAction("Move");
        move.performed += SetStartRotation;
        move.canceled += ResetRotation;

        // to start aiming
        var space = InputSystem.actions.FindAction("Jump");
        space.performed += InitThrow;

        // to start throwing
        var click = InputSystem.actions.FindAction("Click");
        click.performed += Throw;

        // to apply force
        var scroll = InputSystem.actions.FindAction("Zoom");
        scroll.performed += ApplyForce;
    }

    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        _force += ctx.ReadValue<Vector2>().y;
    }

    private void Throw(InputAction.CallbackContext obj)
    {
        if (_dartInstace == null)
            return;
        // when throw starts add cleaupobject to dart to remove it later
        _dartInstace.AddComponent<CleanupObject>().Init(30.0f);
        // add dart movement to have physics
        var movement = _dartInstace.AddComponent<DartMovement>();
        // initialize movement
        // _dartInstace.transform.rotation.eulerAngles.x is the current angle 
        movement.Init(_force);
        // make instance null to be able to make new arrow.
        _dartInstace = null;

    }

    private void InitThrow(InputAction.CallbackContext obj)
    {
        // if it's not null, dont create another
        if (_dartInstace != null)
            return;

        // create dart
        _dartInstace = Instantiate(DartPrefab);
        // position it with mouse
        _dartInstace.transform.position = _currentDartPos;
    }

    private void ResetRotation(InputAction.CallbackContext obj)
    {
        _rotAngle = 0;
    }

    private void Update()
    {
        if (_dartInstace == null)
            return;
        // if there is an instance rotate it according to user
        _dartInstace.transform.Rotate(Vector3.right, RotationSpeed * _rotAngle * Time.deltaTime);
    }

    private void SetStartRotation(InputAction.CallbackContext ctx)
    {
        _rotAngle -= ctx.ReadValue<Vector2>().y;
    }

    private void Aim(InputAction.CallbackContext ctx)
    {
        if (_dartInstace == null)
            return;

        Vector2 mousePos = ctx.ReadValue<Vector2>();
        // create a position in world space (3d space) from mouse position (in screen space)
        // by also using a distance from the camera
        _currentDartPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DistanceFromCamera));

        _dartInstace.transform.position = _currentDartPos;
    }
}