using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using UnityEngine.Experimental.GlobalIllumination;
using SoraHareSakura_GameApi;

namespace SoraHareSakura_DataBaseSystem
{
    public class Data_Base_System : MonoBehaviour
    {
        public string findObj;
        // <summary>
        // �s�W����� GameData_GameRoleTable �ΨӦs���h�Ө��� �]�i�H�гy����
        // �s�W������ GameData_TeamTable �ΨӦs�������T
        // </summary>
        public GameData_GameRoleTable gameRoleTable;
        public GameData_Reserve dataReserve;
        public GameData_StateTable stateTable;
        public GameData_SkillTable skillTable;

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
            //GameObject.Find(findObj).GetComponent<Text>().text = "�ǿ�";
        }

    }

}
