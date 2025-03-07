using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private float screenHalfWidthWorld;
    private GameManager gameManager;
    private float currentSpeed = 0f;
    private float accelerationTime = 0.5f;
    private float acceleration; 

    void Start()
    {
        gameManager = GameManager.Instance;
        CalculateScreenBounds();
        acceleration = moveSpeed / accelerationTime;
    }

    void Update()
    {
        if(gameManager.GetFlag())
        {
            HandleInput();
            MovePlayer();
        }
        ClampPosition();
    }

    void HandleInput()
    {
        moveInput = Vector2.zero; // Reset movement input

        // Touch Input (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float screenHalf = Screen.width / 2;

            if (touch.phase == UnityEngine.TouchPhase.Began || 
                touch.phase == UnityEngine.TouchPhase.Moved || 
                touch.phase == UnityEngine.TouchPhase.Stationary)
            {
                moveInput.x = (touch.position.x < screenHalf) ? -1f : 1f;
            }
        }
        // Mouse Input (For Testing on PC)
        else if (Input.GetMouseButton(0)) // Left-click held down
        {
            float screenHalf = Screen.width / 2;
            moveInput.x = (Input.mousePosition.x < screenHalf) ? -1f : 1f;
        }
        
        // Keyboard input (processed only if no touch/mouse input is active)
        if (moveInput.x == 0)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
        }
    }

    void MovePlayer()
    {
        if (moveInput.x != 0)
        {
            // Accelerate towards max speed
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Instantly stop when no input is given
            currentSpeed = 0f;
        }

        // Move basket
        transform.Translate(moveInput.x * currentSpeed * Time.deltaTime, 0, 0);
    }

    void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -screenHalfWidthWorld,screenHalfWidthWorld);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void CalculateScreenBounds()
    {
        float objectHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    
        // Calculate screen half-width dynamically based on the camera's aspect ratio
        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        screenHalfWidthWorld = screenHalfWidth - objectHalfWidth;
    }
}
