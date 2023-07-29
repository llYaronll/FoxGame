using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using UnityEngine.Experimental.GlobalIllumination;

namespace SoraHareSakura_DataBaseSystem
{
    public class Data_Base_System : MonoBehaviour
    {
        public string findObj;
        public GameData_Reserve dataReserve;

        // Start is called before the first frame update
        void Start()
        {
            if(dataReserve == null)
            {
                dataReserve = new GameData_Reserve();
            }
        }

    
        // Update is called once per frame
        void Update()
        {
            //GameObject.Find(findObj).GetComponent<Text>().text = "¶Ç¿é";
        }
    }

}
