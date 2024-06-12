using UnityEngine;

public class CameraAspectUtility : MonoBehaviour
{
    private static Camera _backgroundCam;
    private Camera _cam;
    private int lastWidth = -1, lastHeight = -1;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        if (!_cam)
        {
            _cam = Camera.main;
        }
        if (!_cam)
        {
            Debug.LogError("No camera available");
            return;
        }
        
		UpdateCamera ();
    }

    
    public void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight) {
            lastWidth = Screen.width;
            lastHeight = Screen.height;

            UpdateCamera();
        }
    }
    private void UpdateCamera()
    {
        if (!ResolutionService.FixedAspectRatio || !_cam)
        {
            return;
        }

        float currentAspectRatio = (float)Screen.width / Screen.height;

        // If the current aspect ratio is already approximately equal to the desired aspect ratio,
        // use a full-screen Rect (in case it was set to something else previously)
        if ((int)(currentAspectRatio * 100) / 100.0f == (int)(ResolutionService.TargetAspectRatio * 100) / 100.0f)
        {
            _cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            if (_backgroundCam)
            {
                Destroy(_backgroundCam.gameObject);
            }
            return;
        }

        // Pillarbox
        if (currentAspectRatio > ResolutionService.TargetAspectRatio)
        {
            float inset = 1.0f - ResolutionService.TargetAspectRatio / currentAspectRatio;
            _cam.rect = new Rect(inset / 2, 0.0f, 1.0f - inset, 1.0f);
        }
        // Letterbox
        else
        {
            float inset = 1.0f - currentAspectRatio / ResolutionService.TargetAspectRatio;
            _cam.rect = new Rect(0.0f, inset / 2, 1.0f, 1.0f - inset);
        }

        if (!_backgroundCam)
        {
            // Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
            _backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
            _backgroundCam.depth = int.MinValue;
            _backgroundCam.clearFlags = CameraClearFlags.SolidColor;
            _backgroundCam.backgroundColor = Color.black;
            _backgroundCam.cullingMask = 0;
        }
    }
}