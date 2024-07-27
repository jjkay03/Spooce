using UnityEditor;
using UnityEngine;

public class PlayerControle : MonoBehaviour {
    
    /* -------------------------------- Variables ------------------------------- */

    [Header("Stats")]
    public float maxFrontSpeed = 4f;
    public float maxSidesSpeed = 1f;
    public float maxBackSpeed = 0.5f;
    public float rotationSpeed = 140f;
    public float acceleration = 3f;
    public float deceleration = 2f;

    [Header("Camera")]
    public GameObject mainCamera;
    public float cameraSmoothing = 5f;
    public float cameraSizeLowSpeed = 3f;
    public float cameraSizeMaxSpeed = 5f;
    public float cameraZoomSpeed = 2f;
    public float cameraZoomSensitivity = 1f;

    [Header("Engines")]
    public GameObject engineMain;
    public GameObject engineLeft;
    public GameObject engineRight;
    public float engineFadeInSpeed = 0.5f;
    public float engineFadeOutSpeed = 4f;
    public float mainEngineMaxOpacity = 0.2f;
    public float sideEngineMaxOpacity = 0.1f;
    private SpriteRenderer mainEngineSprite;
    private SpriteRenderer leftEngineSprite;
    private SpriteRenderer rightEngineSprite;

    [Header("Keybinds")]
    public KeyCode[] moveFrontKey = { KeyCode.W };
    public KeyCode[] moveBackKey = { KeyCode.S };
    public KeyCode[] moveLeftKey = { KeyCode.Q };
    public KeyCode[] moveRightKey = { KeyCode.E };
    public KeyCode[] rotateLeftKey = { KeyCode.A };
    public KeyCode[] rotateRightKey = { KeyCode.D };

    private Vector2 velocity;
    private float rotationVelocity;

    /* ------------------------------ Main Classes ------------------------------ */
    
    // Start is called before the first frame update
    void Start() {
        velocity = Vector2.zero;
        rotationVelocity = 0f;

        // Get engine sprites
        mainEngineSprite = engineMain.GetComponent<SpriteRenderer>();
        leftEngineSprite = engineLeft.GetComponent<SpriteRenderer>();
        rightEngineSprite = engineRight.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
        HandleRotation();
        ApplyMovement();
        HandleCamera();
        HandleEngineSprites();
    }

    /* --------------------------------- Classes -------------------------------- */

    // Function to handle movement
    void HandleMovement() {
        Vector2 input = Vector2.zero;

        if (IsAnyKeyPressed(moveFrontKey)) input.y += 1;
        if (IsAnyKeyPressed(moveBackKey)) input.y -= 1;
        if (IsAnyKeyPressed(moveLeftKey)) input.x -= 1;
        if (IsAnyKeyPressed(moveRightKey)) input.x += 1;

        input = Vector2.ClampMagnitude(input, 1f);

        if (input.magnitude > 0) velocity += input * acceleration * Time.deltaTime;
        else velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);

        velocity.x = Mathf.Clamp(velocity.x, -maxSidesSpeed, maxSidesSpeed);
        velocity.y = Mathf.Clamp(velocity.y, -maxBackSpeed, maxFrontSpeed);
    }

    // Functions to handle rotations
    void HandleRotation() {
        float rotationInput = 0f;

        if (IsAnyKeyPressed(rotateLeftKey)) rotationInput -= 1f;
        if (IsAnyKeyPressed(rotateRightKey)) rotationInput += 1f;

        rotationVelocity = rotationInput * rotationSpeed;
    }

    // Function to apply movement to player
    void ApplyMovement() {
        transform.Translate(velocity * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationVelocity * Time.deltaTime);
    }

    // Function to handle camera movements
    void HandleCamera() {
        if (mainCamera != null) {
            // Handle camera position
            Vector3 targetPosition = transform.position;
            targetPosition.z = mainCamera.transform.position.z; // Keep the camera's original z-position
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraSmoothing * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            // Handle camera zoom
            float currentSpeed = velocity.magnitude;
            float maxSpeed = Mathf.Max(maxFrontSpeed, maxSidesSpeed, maxBackSpeed);
            float speedRatio = Mathf.Clamp01(currentSpeed / (maxSpeed * cameraZoomSensitivity));

            float targetSize = Mathf.Lerp(cameraSizeLowSpeed, cameraSizeMaxSpeed, speedRatio);
            Camera cam = mainCamera.GetComponent<Camera>();
            if (cam != null) {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, cameraZoomSpeed * Time.deltaTime);
            }
        }
    }

    // Function to handle engine sprites
    void HandleEngineSprites() {
        // Main engine
        bool isMovingForward = IsAnyKeyPressed(moveFrontKey);
        FadeSprite(mainEngineSprite, isMovingForward, mainEngineMaxOpacity);

        // Rotating left - right engine
        bool isRotatingRight = IsAnyKeyPressed(rotateRightKey);
        FadeSprite(rightEngineSprite, isRotatingRight, sideEngineMaxOpacity);

        // Rotating right - left engine
        bool isRotatingLeft = IsAnyKeyPressed(rotateLeftKey);
        FadeSprite(leftEngineSprite, isRotatingLeft, sideEngineMaxOpacity);
    }


    /* ---------------------------- Helper Functions ---------------------------- */

    // Helper function to fade sprites
    void FadeSprite(SpriteRenderer sprite, bool fadeIn, float maxOpacity) {
        if (sprite == null) return;
        Color color = sprite.color;
        if (fadeIn) color.a = Mathf.MoveTowards(color.a, maxOpacity, engineFadeInSpeed * Time.deltaTime);
        else color.a = Mathf.MoveTowards(color.a, 0f, engineFadeOutSpeed * Time.deltaTime);
        sprite.color = color;
    }

    // Helper fumction to check if a key is being pressed
    bool IsAnyKeyPressed(KeyCode[] keys) {
        foreach (KeyCode key in keys) if (Input.GetKey(key)) return true;
        return false;
    }

}
