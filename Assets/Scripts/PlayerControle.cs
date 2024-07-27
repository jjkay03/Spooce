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

    [Header("Engines")]
    public GameObject engineMain;
    public GameObject leftMain;
    public GameObject rightMain;

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
    }

    // Update is called once per frame
    void Update() {
        HandleMovement();
        HandleRotation();
        ApplyMovement();
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

    // Fumction to check if a key is being pressed
    bool IsAnyKeyPressed(KeyCode[] keys) {
        foreach (KeyCode key in keys) if (Input.GetKey(key)) return true;
        return false;
    }
}