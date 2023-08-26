using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoraHareSakura_Fight_System;
using UnityEngine.UI;
using SoraHareSakura_DataBaseSystem;
using UnityEngine.UIElements;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameApi;

namespace SoraHareSakura_Fight_System
{
    public class Game_Batter : MonoBehaviour
    {
        public Text text;
        public Game_Actor gameActor;
        public int queuePosition;//0�e�� 1���� 2��� ...
        public float counter;

        //�u����ܧڤ��
        public PriorityTable partnerPriority;
        //�u����ܼĤ��
        public PriorityTable enemyPriority;
        
        public string NextSkill;

        // Start is called before the first frame update
        void Start()
        {
            if(gameActor == null)
                gameActor = new Game_Actor();
            if (text == null)
                text = gameObject.GetComponentInChildren<Text>();
            counter = 0;
            if(gameActor.name == "")
            {
                gameActor.name = gameObject.name;
            }
            //partnerPriority = new PriorityTable();
            //enemyPriority = new PriorityTable();
        }

        // Update is called once per frame
        void Update()
        {
            Survive();
        }

        private void FixedUpdate()
        {
            ShowText();
            counter += Time.fixedDeltaTime * Time.timeScale;
            for(int i = 0; i < gameActor.states.Count; i++)
            {
                if (!gameActor.states[i].TimeFlow(Time.fixedDeltaTime * Time.timeScale))
                {
                    gameActor.states[i] = null;
                }
            }
            if(gameActor.states != null)
            {
                gameActor.states.RemoveAll(state => state == null);
            }
            
            if(counter >= 1)
            {
                gameActor.RunState();
                counter = 0;
            }

        }
        
        //�Τ�r�ԭz�ݩ�
        public void ShowText()
        {
            if (text == null) return;
            text.text = "";
            text.text += gameActor.name + "\n";
            text.text += "HP" + gameActor.stamina.newValue + "/" + gameActor.stamina.maxValue + "\n";
            text.text += "MP" + gameActor.magicPoint.newValue + "/" + gameActor.magicPoint.maxValue + "\n";

            int av = (int)gameActor.actionValue.actionValue;
            int mav = (int)gameActor.actionValue.maxActionValue;
            text.text += "��ʭ�" + av + "/" + mav + "\n";

            text.text += "�����O" + gameActor.attack.maxValue + "\n";
            text.text += "���m�O" + gameActor.defense.maxValue + "\n";
            text.text += "�]�k�����O" + gameActor.magicAttack.maxValue + "\n";
            text.text += "�]�k���m�O" + gameActor.magicDefense.maxValue + "\n";
        }

        public void Survive()
        {
            if(gameActor.stamina.newValue <= 0)
            {
                Game_State k = GameObject.Find("DataBase").GetComponent<Data_Base_System>().stateTable.FindAndAddState("�԰�����");
                gameActor.AddState(k);
            }
        }

        public bool IsSurvive()
        {
            foreach(Game_State state in gameActor.states)
            {
                if(state.limitType == LimitAction.unableToAct)
                {
                    return false;
                }
            }
            return true;
        }

        //�]�w�ؼ��u����
        public void SetTarget(int id,int number,bool isPartner)
        {
            if (isPartner)
            {
                partnerPriority.Sort();
                partnerPriority.ToNumber(id,number);
                partnerPriority.InitSort();
            }
            else
            {
                enemyPriority.Sort();
                enemyPriority.ToNumber(id, number);
                enemyPriority.InitSort();
            }
        }

        //�]�wAI��ܼĤH
        public void AISelectSkill()
        {
            int a = Random.Range(0, gameActor.skills.Count-1);
            if(gameActor.skills.Count == 0) return;
            NextSkill = gameActor.skills[a].name;
            gameActor.actionValue.maxActionValue = gameActor.skills[a].SkillConsumeActionValue();
            gameActor.actionValue.SetRestore(0);
        }

        //find skill by name
        public Game_Skill FindSkill(string skillName)
        {
            return gameActor.skills.Find(skill => skill.name.Equals(skillName));
        }

    }
}
