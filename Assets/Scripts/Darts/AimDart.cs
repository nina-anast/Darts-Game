using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class AimDart : MonoBehaviour
{
    public float DistanceFromCamera = 10.0f;
    public GameObject DartPrefab;
    private GameObject _dartInstace;
    private Camera _cam;

    public float RotationSpeed = 10.0f;
    private float _appliedForce = 50.0f;

    // Visualize force
    public Slider Slider;
    public Image Image;
    public Color NewColor;

    private float _rotAngle;
    private Vector3 _currentDartPos;

    // Show how many throws happened
    private int _throws = 0;
    public TextMeshProUGUI ThrowsTxt;

    // Reference to WindManager
    public WindManager WindManager;

    private void Start()
    {
        // Locate WindManager if not assigned
        if (WindManager == null)
        {
            WindManager = Object.FindFirstObjectByType<WindManager>();
            if (WindManager == null)
            {
                Debug.LogError("WindManager not found! Please ensure there is a WindManager in the scene.");
            }
        }

        // Initial position of dart
        var mousePosAction = InputSystem.actions.FindAction("Point");
        mousePosAction.performed += Aim;

        _cam = Camera.main;

        // Rotation of dart
        var move = InputSystem.actions.FindAction("Move");
        move.performed += SetStartRotation;
        move.canceled += ResetRotation;

        // To start aiming
        var space = InputSystem.actions.FindAction("Jump");
        space.performed += InitThrow;

        // To start throwing
        var click = InputSystem.actions.FindAction("Click");
        click.performed += Throw;

        // To apply force
        var scroll = InputSystem.actions.FindAction("Zoom");
        scroll.performed += ApplyForce;

        // To visualize force
        Slider.value = _appliedForce;
        // Change slider color
        ChangeColor(NewColor);

        // Show throws
        ThrowsTxt.text = $"Throws: {_throws:F0}";
    }

    private void ChangeColor(Color color)
    {
        Image.color = color;
    }

    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        _appliedForce += ctx.ReadValue<Vector2>().y;
        Slider.value = _appliedForce;
        ChangeColor(NewColor);
    }

    private void Throw(InputAction.CallbackContext obj)
    {
        if (_dartInstace == null)
            return;

        // Add CleanupObject to dart for later removal
        _dartInstace.AddComponent<CleanupObject>().Init(30.0f);

        // Add DartMovement to apply physics and wind effects
        var movement = _dartInstace.AddComponent<DartMovement>();

        // Initialize DartMovement with applied force and wind manager
        movement.Init(_appliedForce, WindManager);

        // Make instance null to allow new dart creation
        _dartInstace = null;
    }

    private void InitThrow(InputAction.CallbackContext obj)
    {
        if (_dartInstace != null)
            return;

        _dartInstace = Instantiate(DartPrefab);
        _dartInstace.transform.position = _currentDartPos;

        _throws += 1;
        SavedData.Instance.UpdateThrows(_throws);
        ThrowsTxt.text = $"Throws: {_throws:F0}";
    }

    private void ResetRotation(InputAction.CallbackContext obj)
    {
        _rotAngle = 0;
    }

    private void Update()
    {
        if (_dartInstace == null)
            return;

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
        _currentDartPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DistanceFromCamera));
        _dartInstace.transform.position = _currentDartPos;
    }
}
