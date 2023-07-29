using System.Collections;
using System.Collections.Generic;
using SoraHareSakura_GameData_Api;
using SoraHareSakura_Game_Api;
using SoraHareSakura_DataBaseSystem;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Item_Show : MonoBehaviour
{
    public string dataBaseName;
    public int dataBaseSize;
    public GameObject itemShowUI;
    public GameObject itemShowBox;
    public GameData_Reserve gameDataReserve;
    // Start is called before the first frame update
    void Start()
    {
        dataBaseSize = 0;
        if (dataBaseName != "")
        {
            gameDataReserve = GameObject.Find(dataBaseName).GetComponent<Data_Base_System>().dataReserve;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(GameObject.Find(dataBaseName).GetComponent<Data_Base_System>().dataReserve.items.Count > dataBaseSize)
        {
            int loopTime = GameObject.Find(dataBaseName).GetComponent<Data_Base_System>().dataReserve.items.Count - dataBaseSize;
            for (int i = 0; i < loopTime; i++)
            {
                GameObject gameObjectBox = Instantiate(itemShowBox);
                gameObjectBox.GetComponentInChildren<Text>().text = gameDataReserve.items[i + dataBaseSize].name;
                gameObjectBox.transform.parent = itemShowUI.transform;
            }
            dataBaseSize = GameObject.Find(dataBaseName).GetComponent<Data_Base_System>().dataReserve.items.Count;
        }
    }
}
