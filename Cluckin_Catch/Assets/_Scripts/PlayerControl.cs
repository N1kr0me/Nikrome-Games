using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private float screenHalfWidthWorld;

    void Start()
    {
        CalculateScreenBounds();
    }

    void Update()
    {
        HandleInput();
        MovePlayer();
        ClampPosition();
    }

    void HandleInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                float touchX = (touch.position.x / Screen.width) * 2 - 1;
                moveInput.x = Mathf.Sign(touchX);
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
        float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        screenHalfWidthWorld = screenHalfWidth - objectHalfWidth;
    }
}
