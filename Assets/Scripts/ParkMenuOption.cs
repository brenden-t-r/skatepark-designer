using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParkMenuOption : MonoBehaviour
{
    [SerializeField] private Button btnLoad;
    
    void Start()
    {
        btnLoad.onClick.AddListener(LoadPark);
    }

    void LoadPark()
    {
        SceneManager.LoadScene("EditorScene");
    }
}
