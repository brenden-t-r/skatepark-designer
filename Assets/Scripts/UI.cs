using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class UI : MonoBehaviour
{
    private readonly float PAN_SPEED = 1f;
    private readonly int ZOOM_SPEED = 2;
    [SerializeField] private Transform _camera;
    [SerializeField] private PixelPerfectCamera _ppCamera;
    
    // Park menu
    [SerializeField] private Text textParkName;
    [SerializeField] private Button btnSave, btnClear, btnExit;

    // Navigation UI
    [SerializeField] private Button btnLeft, btnRight, btnUp, btnDown;
    [SerializeField] private Button btnZoomOut, btnZoomIn;

    // Save Menu
    [SerializeField] private GameObject saveMenuPanel;
    [SerializeField] private InputField saveMenuInputName;
    [SerializeField] private InputField saveMenuInputAuthor;
    [SerializeField] private Button saveMenuBtnSave;
    [SerializeField] private Button saveMenuBtnCancel;
    
    [SerializeField] private Build _build;
    
    void Start()
    {
        btnSave.onClick.AddListener(SaveMenuOpen);
        btnClear.onClick.AddListener(ClearPark);
        btnExit.onClick.AddListener(ExitToMainMenu);
        btnLeft.onClick.AddListener(PanLeft);
        btnRight.onClick.AddListener(PanRight);
        btnUp.onClick.AddListener(PanUp);
        btnDown.onClick.AddListener(PanDown);
        btnZoomOut.onClick.AddListener(ZoomOut);
        btnZoomIn.onClick.AddListener(ZoomIn);
        saveMenuBtnSave.onClick.AddListener(SaveMenuSave);
        saveMenuBtnCancel.onClick.AddListener(SaveMenuCancel);
        textParkName.text = ParkDataSaves.parkData.title;
        saveMenuPanel.SetActive(false);
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

    void ClearPark()
    {
        _build.ClearMap();
    }

    void SaveMenuOpen()
    {
        saveMenuPanel.SetActive(true);
    }

    void SaveMenuCancel()
    {
        saveMenuPanel.SetActive(false);
    }

    void SaveMenuSave()
    {
        ParkDataSaves.parkData.title = saveMenuInputName.text;
        ParkDataSaves.parkData.author = saveMenuInputAuthor.text;
        textParkName.text = saveMenuInputName.text;
        _build.SaveMap();
        saveMenuPanel.SetActive(false);
    }

    void ExitToMainMenu()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
