using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private readonly float PAN_SPEED = 1f;
    [SerializeField] private Camera _camera;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private Button btnUp;
    [SerializeField] private Button btnDown;
    void Start()
    {
        btnLeft.onClick.AddListener(PanLeft);
        btnRight.onClick.AddListener(PanRight);
        btnUp.onClick.AddListener(PanUp);
        btnDown.onClick.AddListener(PanDown);
    }

    void PanLeft()
    {
        Vector3 pos = _camera.transform.position;
        _camera.transform.position = new Vector3(pos.x - PAN_SPEED, pos.y, pos.z);
    }

    void PanRight()
    {
        Vector3 pos = _camera.transform.position;
        _camera.transform.position = new Vector3(pos.x + PAN_SPEED, pos.y, pos.z);
    }
    
    void PanUp()
    {
        Vector3 pos = _camera.transform.position;
        _camera.transform.position = new Vector3(pos.x, pos.y + PAN_SPEED, pos.z);
    }
    
    void PanDown()
    {
        Vector3 pos = _camera.transform.position;
        _camera.transform.position = new Vector3(pos.x, pos.y - PAN_SPEED, pos.z);
    }
}
