using UnityEngine;
using UnityEngine.U2D;

public class MobileResolution : MonoBehaviour
{
    private int LastWidth = 0;
    private int LastHeight = 0;
    private float sceneWidth = 17;
    private Camera _camera;

    public void Start()
    {
        _camera = GetComponent<Camera>();
    }
    
    public void FixedUpdate ()
    {
        KeepAspectRatio();
        AdjustCameraSize();
    }
    
    public void AdjustCameraSize()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        _camera.orthographicSize = (int) desiredHalfHeight;
    }

    public void KeepAspectRatio()
    {
        float width = Screen.width;
        float height = Screen.height;

        if(LastWidth != width) // if the user is changing the width
        {
            // update the height
            float heightAccordingToWidth = width / 9 * 16;
            Screen.SetResolution((int) width, (int) Mathf.Round(heightAccordingToWidth), false, 0);
        }
        else if(LastHeight != height) // if the user is changing the height
        {
            // update the width
            float widthAccordingToHeight = height / 16 * 9;
            Screen.SetResolution((int) Mathf.Round(widthAccordingToHeight), (int) height, false, 0);
        }

        LastWidth = (int) width;
        var lastHeight = height;
    }
}