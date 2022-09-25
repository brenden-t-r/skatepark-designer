using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace EditorScene.UI
{
    /*
     * Camera panning and Pixel Perfect "zooming".
     */
    public class UINavigation : MonoBehaviour
    {
        private readonly float PAN_SPEED = 1f;
        private readonly int ZOOM_SPEED = 2;
        
        private Transform cameraTransform;
        private PixelPerfectCamera pixelPerfectCamera;
        [SerializeField] private Button btnLeft, btnRight, btnUp, btnDown;
        [SerializeField] private Button btnZoomOut, btnZoomIn;

        private void Start()
        {
            Camera cam = Camera.main;
            cameraTransform = cam.GetComponent<Transform>();
            pixelPerfectCamera = cam.GetComponent<PixelPerfectCamera>();
            btnLeft.onClick.AddListener(PanLeft);
            btnRight.onClick.AddListener(PanRight);
            btnUp.onClick.AddListener(PanUp);
            btnDown.onClick.AddListener(PanDown);
            btnZoomOut.onClick.AddListener(ZoomOut);
            btnZoomIn.onClick.AddListener(ZoomIn);
        }
        
        private void PanLeft()
        {
            Vector3 pos = cameraTransform.transform.position;
            cameraTransform.position = new Vector3(pos.x - PAN_SPEED, pos.y, pos.z);
        }

        private void PanRight()
        {
            Vector3 pos = cameraTransform.transform.position;
            cameraTransform.position = new Vector3(pos.x + PAN_SPEED, pos.y, pos.z);
        }
    
        private void PanUp()
        {
            Vector3 pos = cameraTransform.transform.position;
            cameraTransform.position = new Vector3(pos.x, pos.y + PAN_SPEED, pos.z);
        }
    
        private void PanDown()
        {
            Vector3 pos = cameraTransform.transform.position;
            cameraTransform.position = new Vector3(pos.x, pos.y - PAN_SPEED, pos.z);
        }

        private void ZoomOut()
        {
            pixelPerfectCamera.refResolutionX *= ZOOM_SPEED;
            pixelPerfectCamera.refResolutionY *= ZOOM_SPEED;
        }

        private void ZoomIn()
        {
            pixelPerfectCamera.refResolutionX /= ZOOM_SPEED;
            pixelPerfectCamera.refResolutionY /= ZOOM_SPEED;
        }
    }
}