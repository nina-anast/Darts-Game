using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// this script is running in an empty game object.
// it's main purpose is to control the darts with given inputs from player.
// inputs create, make initial conditions of dart and when thrown -> dart movement is activated.
public class AimDart : MonoBehaviour
{
    // initial z position of dart from player
    public float DistanceFromCamera = 10.0f;
    public GameObject DartPrefab;
    private GameObject _dartInstace;
    private Camera _cam;

    // for arrows inputs 
    public float RotationSpeed = 10.0f;
    // can be changed from mouse scroll
    private float _appliedForce = 50.0f;

    // Visualize force
    public Slider Slider;
    public Image Image;
    // isn't used
    public Color NewColor;

    // to get initial angle and position of dart
    private float _rotAngle;
    private Vector3 _currentDartPos;

    // Show how many throws happened
    private int _throws = 0;
    // update texts in ui to let player know of useful data
    public TextMeshProUGUI ThrowsTxt;
    public TextMeshProUGUI LastShot;
    public TextMeshProUGUI Multi;

    // Reference to WindManager (find script)
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

    // not used
    private void ChangeColor(Color color)
    {
        Image.color = color;
    }

    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        // reads input value and assigns it in force and in slider. 
        // should change color too but doesn't work correctly.
        _appliedForce += ctx.ReadValue<Vector2>().y;
        Slider.value = _appliedForce;
        ChangeColor(NewColor);
    }

    private void Throw(InputAction.CallbackContext obj)
    {
        // if no dart exists, skip
        if (_dartInstace == null)
            return;

        // Add CleanupObject to dart for later removal
        _dartInstace.AddComponent<CleanupObject>().Init(30.0f);

        // Add DartMovement to apply physics and wind effects
        var movement = _dartInstace.AddComponent<DartMovement>();

        // Initialize DartMovement with applied force and wind manager
        movement.Init(_appliedForce, WindManager, LastShot, Multi);

        // Make instance null to allow new dart creation
        _dartInstace = null;
    }

    private void InitThrow(InputAction.CallbackContext obj)
    {
        // if dart exists, skip
        if (_dartInstace != null)
            return;

        // if it doesn't exist make a new instance
        _dartInstace = Instantiate(DartPrefab);
        // make it's position same as mouse (look at Aim)
        _dartInstace.transform.position = _currentDartPos;

        // add 1 throw in players tries
        _throws += 1;
        // update singleton and text
        SavedData.Instance.UpdateThrows(_throws);
        ThrowsTxt.text = $"Throws: {_throws:F0}";
    }

    // to put dart back to 0 angle
    private void ResetRotation(InputAction.CallbackContext obj)
    {
        _rotAngle = 0;
    }

    private void Update()
    {
        // skip if no dart
        if (_dartInstace == null)
            return;

        // rotate dart with const speed (see angle in SetStartRotation)
        _dartInstace.transform.Rotate(Vector3.right, RotationSpeed * _rotAngle * Time.deltaTime);
    }

    private void SetStartRotation(InputAction.CallbackContext ctx)
    {
        // read angle and assign it
        _rotAngle -= ctx.ReadValue<Vector2>().y;
    }

    private void Aim(InputAction.CallbackContext ctx)
    {
        // if there is no dart, skip
        if (_dartInstace == null)
            return;

        // read mouse position
        Vector2 mousePos = ctx.ReadValue<Vector2>();
        // assign dart position as mouse position and distance from camera
        _currentDartPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DistanceFromCamera));
        _dartInstace.transform.position = _currentDartPos;
    }

    // remove input actions when changing scene
    private void OnDestroy()
    {
        // Initial position of dart
        var mousePosAction = InputSystem.actions.FindAction("Point");
        mousePosAction.performed -= Aim;

        // Rotation of dart
        var move = InputSystem.actions.FindAction("Move");
        move.performed -= SetStartRotation;
        move.canceled -= ResetRotation;

        // To start aiming
        var space = InputSystem.actions.FindAction("Jump");
        space.performed -= InitThrow;

        // To start throwing
        var click = InputSystem.actions.FindAction("Click");
        click.performed -= Throw;

        // To apply force
        var scroll = InputSystem.actions.FindAction("Zoom");
        scroll.performed -= ApplyForce;
    }
}
