using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace EditorScene.UI
{
    public class UIEditor : MonoBehaviour
    {
        // Park menu
        [SerializeField] private Text textParkName;
        [SerializeField] private Button btnSave, btnClear, btnExit;

        // Save Menu
        [SerializeField] private GameObject saveMenuPanel;
        [SerializeField] private InputField saveMenuInputName;
        [SerializeField] private InputField saveMenuInputAuthor;
        [SerializeField] private Button saveMenuBtnSave;
        [SerializeField] private Button saveMenuBtnCancel;
    
        //[SerializeField] private BuildController buildController;

        private void Start()
        {
            btnSave.onClick.AddListener(SaveMenuOpen);
            btnClear.onClick.AddListener(ClearPark);
            btnExit.onClick.AddListener(ExitToMainMenu);
            saveMenuBtnSave.onClick.AddListener(SaveMenuSave);
            saveMenuBtnCancel.onClick.AddListener(SaveMenuCancel);
            textParkName.text = ParkDataSaves.parkData.title;
            saveMenuPanel.SetActive(false);
        }

        private void ClearPark()
        {
            TilemapManager._instance.ClearMap();
        }

        private void SaveMenuOpen()
        {
            saveMenuPanel.SetActive(true);
        }

        private void SaveMenuCancel()
        {
            saveMenuPanel.SetActive(false);
        }

        private void SaveMenuSave()
        {
            ParkDataSaves.parkData.title = saveMenuInputName.text;
            ParkDataSaves.parkData.author = saveMenuInputAuthor.text;
            textParkName.text = saveMenuInputName.text;
            TilemapManager._instance.SaveMap();
            saveMenuPanel.SetActive(false);
        }

        private void ExitToMainMenu()
        {
            SceneManager.LoadScene("StartMenuScene");
        }
    }
}
