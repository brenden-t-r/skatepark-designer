using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private readonly float PAN_SPEED = 1f;
    private readonly int ZOOM_SPEED = 2;
    [SerializeField] private Transform _camera;
    [SerializeField] private PixelPerfectCamera _ppCamera;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private Button btnUp;
    [SerializeField] private Button btnDown;
    [SerializeField] private Button btnZoomOut, btnZoomIn;
    [SerializeField] private Button btnSave, btnClear;
    [SerializeField] private Text textParkName;
    [SerializeField] private Build _build;
    
    void Start()
    {
        btnLeft.onClick.AddListener(PanLeft);
        btnRight.onClick.AddListener(PanRight);
        btnUp.onClick.AddListener(PanUp);
        btnDown.onClick.AddListener(PanDown);
        btnZoomOut.onClick.AddListener(ZoomOut);
        btnZoomIn.onClick.AddListener(ZoomIn);
        btnSave.onClick.AddListener(SavePark);
        btnClear.onClick.AddListener(ClearPark);
        textParkName.text = ParkDataSaves.parkData.title;
    }

    void PanLeft()
    {
        Vector3 pos = _camera.transform.position;
        _camera.position = new Vector3(pos.x - PAN_SPEED, pos.y, pos.z);
    }

    void PanRight()
    {
        Vector3 pos = _camera.transform.position;
        _camera.position = new Vector3(pos.x + PAN_SPEED, pos.y, pos.z);
    }
    
    void PanUp()
    {
        Vector3 pos = _camera.transform.position;
        _camera.position = new Vector3(pos.x, pos.y + PAN_SPEED, pos.z);
    }
    
    void PanDown()
    {
        Vector3 pos = _camera.transform.position;
        _camera.position = new Vector3(pos.x, pos.y - PAN_SPEED, pos.z);
    }

    void ZoomOut()
    {
        _ppCamera.refResolutionX *= ZOOM_SPEED;
        _ppCamera.refResolutionY *= ZOOM_SPEED;
    }

    void ZoomIn()
    {
        _ppCamera.refResolutionX /= ZOOM_SPEED;
        _ppCamera.refResolutionY /= ZOOM_SPEED;
    }

    void SavePark()
    {
        _build.SaveMap();
    }

    void ClearPark()
    {
        _build.ClearMap();
    }
}
