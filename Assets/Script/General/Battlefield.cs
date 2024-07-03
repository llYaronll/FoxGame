using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SoraHareSakura_General
{
    public class Battlefield : MonoBehaviour
    {
        public List<RoleAI> playerRoleList;
        public List<RoleAI> enemyRoleList;
        public List<GameObject> OperationCardList;

        public bool BattleIsOver = false;
        //public int Wave;
        // Start is called before the first frame update
        void Start()
        {
            BattleIsOver = false;
            for (int i = 0; i < OperationCardList.Count; i++)
            {
                if (i < playerRoleList.Count)
                    OperationCardList[i].GetComponent<RoleCardOperate>().SetOperateRole(playerRoleList[i]);
                else
                    OperationCardList[i].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            ReviewTheSituation();
            if (BattleIsOver)
            {
                foreach(RoleAI roldObj in playerRoleList)
                {
                    roldObj.FighetEnd();
                }
                foreach(RoleAI roldObj in enemyRoleList)
                {
                    roldObj.FighetEnd();
                }
                Debug.Log("battle over");
            }
        }

        public void LoadData(List<RoleAI> playerRoles, List<RoleAI> enemyRoles)
        {
            playerRoleList = playerRoles;
            enemyRoleList = enemyRoles;
        }

        public void ReviewTheSituation()
        {
            int playerRoleCount = playerRoleList.Count;
            foreach(RoleAI roleObj in playerRoleList)
            {
                if (roleObj.Befeated())
                {
                    playerRoleCount--;
                }
            }
            if(playerRoleCount <= 0)
            {
                BattleIsOver = true;
            }
            int enemyRoleCount = enemyRoleList.Count;
            foreach (RoleAI roleObj in enemyRoleList) { 
                if (roleObj.Befeated()) { 
                    enemyRoleCount--; 
                } 
            }
            if(enemyRoleCount <= 0)
            {
                //Wave--;
                BattleIsOver = true;
            }
        }
    }
}
