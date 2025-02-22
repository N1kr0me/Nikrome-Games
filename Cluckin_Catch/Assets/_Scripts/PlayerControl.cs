using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private float screenHalfWidthWorld;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        CalculateScreenBounds();
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
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                // Move towards touch smoothly
                moveInput.x = Mathf.Clamp(touchWorldPos.x - transform.position.x, -1, 1);
            }
            else
            {
                moveInput = Vector2.zero;
            }
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
        }
    }

    void MovePlayer()
    {
        transform.Translate(moveInput.x*moveSpeed*Time.deltaTime,0,0);
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
