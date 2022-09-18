using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private Button btnNew;
    [SerializeField] private Transform viewportContent;
    [SerializeField] private GameObject parkMenuOptionPrefab;
    
    void Start()
    {
        LoadSavedParks();
        btnNew.onClick.AddListener(NewPark);
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
            addParkMenuOption(i, parkData);
        }
    }

    void addParkMenuOption(int index, ParkData parkData)
    {
        GameObject prefab = Instantiate(parkMenuOptionPrefab, viewportContent);
        RectTransform rectTransform = prefab.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.sizeDelta = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(0, index * -120f);
        ParkMenuOption option = prefab.GetComponentInChildren<ParkMenuOption>();
        option.textName.text = parkData.title;
        Button btnLoad = option.btnLoad;
        btnLoad.onClick.AddListener(() => LoadPark(parkData));
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
}
