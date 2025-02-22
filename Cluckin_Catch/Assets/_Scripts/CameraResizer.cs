using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    private float targetAspect = 16f / 9f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdjustCamera();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustCamera();
    }
    void AdjustCamera()
    {
        float windowAspect = (float) Screen.width / (float) Screen.height;
        float scaleHeight = windowAspect/targetAspect;
        Camera cam = Camera.main;

        if (scaleHeight < 1.0f) // Too tall (e.g., 16:10, 4:3)
        {
            GetComponent<Camera>().rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
        }
        else // Too wide (e.g., ultrawide monitors, free aspect)
        {
            float scaleWidth = 1.0f / scaleHeight;
            GetComponent<Camera>().rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
        }

    }
}
