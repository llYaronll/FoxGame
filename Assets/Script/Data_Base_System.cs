using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using UnityEngine.Experimental.GlobalIllumination;
using SoraHareSakura_GameApi;
using System.Data;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace SoraHareSakura_DataBaseSystem
{
    public class Data_Base_System : MonoBehaviour
    {
        public string findObj;
        // <summary>
        // �s�W����� GameData_GameRoleTable �ΨӦs���h�Ө��� �]�i�H�гy����
        // �s�W������ GameData_TeamTable �ΨӦs�������T
        // </summary>
        public GameData_GameRoleTable gameRoleTable;//�ͭx���
        public GameData_GameRoleTable enemyTable;//�ĤH���

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

        public void CreateRole(int id)
        {
            Character_Attribute roleData = new Character_Attribute();
            roleData.init();
            roleData.id = 0;
            roleData.name = "NewRole";
            roleData.level.Init(100);

            roleData.stamina.SetInit(100,100);
            roleData.magicPoint.SetInit(100, 100);

            roleData.power.SetInit(10);
            roleData.spiritualPower.SetInit(10);
            roleData.speed.SetInit(5);

            if (id == 0)
            {
                gameRoleTable.Add(roleData);
            }
            else if(id == 1)
            {
                enemyTable.Add(roleData);
            }
        }

        //�W�[������� 0 ���ͭx�� 1 ���ĤH��
        public void AddRole(Character_Attribute roleData, int id)
        {
            if (id == 0)
            {
                gameRoleTable.Add(roleData);
            }
            else if (id == 1)
            {
                enemyTable.Add(roleData);
            }
        }

        //�W�[�����ͭx���
        public void AddRoleToGameRoleTable(Character_Attribute roleData)
        {
            AddRole(roleData, 0);
        }

        //�W�[�����ĤH���
        public void AddRoleToEnemyTable(Character_Attribute roleData)
        {
            AddRole(roleData, 1);
        }
    }

}
