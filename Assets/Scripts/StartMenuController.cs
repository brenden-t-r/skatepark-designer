using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private List<ParkMenuOption> parkMenuOptions;
    [SerializeField] private Button btnNew;
    
    void Start()
    {
        foreach (var parkMenuOption in parkMenuOptions)
        {
            Button btnLoad = parkMenuOption.btnLoad;
            Text parkName = parkMenuOption.textName;
            btnLoad.onClick.AddListener(() => LoadPark(parkName.text));
        }
        
        btnNew.onClick.AddListener(NewPark);
    }

    public void NewPark()
    {
        ParkData parkData = ScriptableObject.CreateInstance<ParkData>();
        parkData.title = "TestPark";
        parkData.author = "TestAuthor";
        ParkDataSaves.parkData = parkData;
        SceneManager.LoadScene("EditorScene");
    }
    
    public void LoadPark(string parkName)
    {
        string json = PlayerPrefs.GetString(parkName); 
        Debug.Log(json);
        ParkData parkData = ScriptableObject.CreateInstance<ParkData>();
        JsonUtility.FromJsonOverwrite(json, parkData);
        ParkDataSaves.parkData = parkData;
        SceneManager.LoadScene("EditorScene");
    }
}
