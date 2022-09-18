using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private Button btnNew, btnExit;
    [SerializeField] private GameObject viewportContent;
    [SerializeField] private GameObject parkMenuOptionPrefab;
    private Dictionary<string, GameObject> parkMenuOptionsMap = new Dictionary<string, GameObject>();
    
    void Start()
    {
        LoadSavedParks();
        btnNew.onClick.AddListener(NewPark);
        btnExit.onClick.AddListener(() => Application.Quit());
    }

    void LoadSavedParks()
    {
        string parksJson = PlayerPrefs.GetString("Parks");
        ParkDataSaves.ListWrapper<String> parks = parksJson != "" 
            ? JsonUtility.FromJson<ParkDataSaves.ListWrapper<String>>(parksJson) 
            : new ParkDataSaves.ListWrapper<string>();
        Debug.Log(parks.list.Count);
        for (int i = 0; i < parks.list.Count; i++)
        {
            String parkDataJson = PlayerPrefs.GetString(parks.list[i]);
            Debug.Log(parkDataJson);
            if (parkDataJson.Equals(""))
            {
                Debug.Log(parks.list[i] + " is empty.");
                continue;
            }
            ParkData parkData = ScriptableObject.CreateInstance<ParkData>();
            JsonUtility.FromJsonOverwrite(parkDataJson, parkData);
            addParkMenuOption(parkData);
        }
    }

    void addParkMenuOption(ParkData parkData)
    {
        GameObject prefab = Instantiate(parkMenuOptionPrefab, viewportContent.transform);
        ParkMenuOption option = prefab.GetComponentInChildren<ParkMenuOption>();
        option.textName.text = parkData.title;
        option.btnLoad.onClick.AddListener(() => LoadPark(parkData));
        option.btnDelete.onClick.AddListener(() => DeletePark(parkData.title));
        parkMenuOptionsMap.Add(parkData.title, prefab);
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

    public void LoadPark(ParkData parkData)
    {
        ParkDataSaves.parkData = parkData;
        SceneManager.LoadScene("EditorScene");
    }

    public void DeletePark(string parkName)
    {
        // Delete park
        PlayerPrefs.DeleteKey(parkName);
        
        // Delete park from park list
        string parksJson = PlayerPrefs.GetString("Parks");
        ParkDataSaves.ListWrapper<String> parks = parksJson != "" 
            ? JsonUtility.FromJson<ParkDataSaves.ListWrapper<String>>(parksJson) 
            : new ParkDataSaves.ListWrapper<string>();
        parks.list = parks.list.Where(x => !x.Equals(parkName)).ToList();
        parksJson = JsonUtility.ToJson(parks);
        PlayerPrefs.SetString("Parks", parksJson);
        
        // Delete park menu option
        // TODO
        parkMenuOptionsMap.TryGetValue(parkName, out GameObject option);
        Destroy(option);
        parkMenuOptionsMap.Remove(parkName);
    }
}
