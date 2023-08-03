using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using UnityEngine.Experimental.GlobalIllumination;
using SoraHareSakura_GameApi;
using UnityEngine.TextCore.Text;
using System;
using TMPro.EditorUtilities;

namespace SoraHareSakura_Fight_System
{
    public class Game_Actor
    {
        public int id;
        public string name;

        //base attribute
        public AttributeValue stamina;
        public AttributeValue MP;
        public int power;
        public int speed;
        public int spiritualPower;

        //critical hit
        public float criticalHitHarm;
        public float criticalHitRate;

        public int hitRate;
        public int luck;

        public List<Game_State> states;
        public List<Game_Skill> skills;
        public Action_Value actionValue;
        public List<string> targets;
        public int maxTargets;

        //figher attribute
        public int attack;
        public int magicAttack;
        public int magicDefense;
        public int defense;


        public Game_Actor(Game_Character character)
        {
            Action_Value initActionValue = new Action_Value();
            initActionValue.maxActionValue = 10;
            initActionValue.actionValue = 10;
            init(character.id, character.name, character.stamina, character.power, character.speed, character.MP, character.spiritualPower, character.defense, character.criticalHitHarm, character.criticalHitRate, character.hitRate, character.states, character.luck, initActionValue, character.magicDefense);
        }

        public Game_Actor(int id, string name, int stamina, int power, int speed, int mP, int spiritualPower, int defense, float criticalHitHarm, float criticalHitRate, int hitRate, List<Game_State> states, int luck, Action_Value actionValue, int magicDefense)
        {
            init(id, name, stamina, power, speed, mP, spiritualPower,defense, criticalHitHarm, criticalHitRate, hitRate, states, luck, actionValue,magicDefense);
        }

        public void init(int id, string name, AttributeValue stamina, int power, int speed, AttributeValue mP, int spiritualPower, int defense, float criticalHitHarm, float criticalHitRate, int hitRate, List<Game_State> states, int luck, Action_Value actionValue, int magicDefense)
        {
            this.id = id;
            this.name = name;
            this.stamina = stamina;
            this.power = power;
            this.speed = speed;
            MP = mP;
            this.spiritualPower = spiritualPower;
            this.magicDefense = magicDefense;
            this.defense = defense;
            this.criticalHitHarm = criticalHitHarm;
            this.criticalHitRate = criticalHitRate;
            this.hitRate = hitRate;
            this.states = states;
            this.luck = luck;
            this.actionValue = actionValue;
        }
        public void init(int id, string name, int stamina, int power, int speed, int mP, int spiritualPower, int defense, float criticalHitHarm, float criticalHitRate, int hitRate, List<Game_State> states, int luck, Action_Value actionValue,int magicDefense)
        {
            this.id = id;
            this.name = name;
            this.stamina = new AttributeValue(stamina);
            this.power = power;
            this.speed = speed;
            MP = new AttributeValue(mP);
            this.spiritualPower = spiritualPower;
            this.magicDefense = magicDefense;
            this.defense = defense;
            this.criticalHitHarm = criticalHitHarm;
            this.criticalHitRate = criticalHitRate;
            this.hitRate = hitRate;
            this.states = states;
            this.luck = luck;
            this.actionValue = actionValue;
        }

        public void TimeFlow(float elapsedTime)
        {
            //行動值
            float restoreActionValueRate = 1;
            actionValue.Restore(elapsedTime * speed * restoreActionValueRate);
            
            //狀態時間
            states.ForEach(state =>
            {
                if(state.type == StateType.Aging)
                {
                    state.TimeFlow(elapsedTime);
                }
            });

            //當狀態時間歸零時 解除狀態
            states.RemoveAll(state => state.timeLeft == 0 && state.type == StateType.Aging);
        
        }

        //使用技能
        public bool UseSkill(string skillName)
        {
            Game_Skill skill = skills.Find(skill => skill.name == skillName);
            string skillCommand = skill.consume.command;
            int value;
            bool skillUseOk = int.TryParse(skill.consume.args[0],out value);//(float)Convert.ToDouble(skill.consume.args[0]);
            if (!skillUseOk)
            {
                return false;
            }
            skillUseOk = SkillConsume(skillCommand,value);
            if (!skillUseOk)
            {
                return false;
            }
            return true;
        }

        public bool SkillConsume(string a, int value)
        {
            if (a.Equals("HP"))
            {
                return stamina.ConsumeValue(value);
            }
            if (a.Equals("MP"))
            {
                return MP.ConsumeValue(value);
            }
            if (a.Equals("AV"))
            {
                return actionValue.ConsumeValue(value);
            }
            return false;
        }

        public int SkillDamage(string damageType,float damage,Game_Actor target)
        {
            int damageValue = (int)damage;
            if (damageType.Equals("HP+"))
            {
                //target.stamina.AddValue(-);
            }
            if (damageType.Equals("MP+"))
            {
                //target.MP.AddValue(-);
            }
            if (damageType.Equals("AV+"))
            {
                //target.actionValue.Restore(-);
            }
            return damageValue;
        }

        //附加狀態
        public void AddState(Game_State stateAdd)
        {
            foreach (Game_State stateA in states)
            {
                if (stateA.name.Equals(stateAdd.name))
                {
                    stateA.timeLeft = stateA.timeLeft + stateAdd.duration;
                    return;
                }
            }
            stateAdd.timeLeft = stateAdd.duration;
            states.Add(stateAdd);
        }

        //攻擊

        public void Attack(Game_Actor target,string skillName)
        {
            bool useSkillOk = UseSkill(skillName);
            if (useSkillOk)
            {
                
            }
        }

        public int DamageFunction(Game_Actor b)//a is attacker ,b is target
        {
            Game_Actor a = this;
            int damageValue = a.power - b.defense;
            if (damageValue > 0)
            {
                return 0;
            }
            return damageValue;
        }

        public void SetTarget(List<string> targets)
        {
            this.targets = targets;
        }

        public void AddTarget(string target)
        {
            this.targets.Add(target);
        }

        public void DeleteTarget(string target)
        {
            targets.Remove(target);
        }

        public void ResetTarget()
        {
            this.targets = new List<string>();
        }

        public List<string> GetTargets()
        {
            return targets;
        }
    }

    public class Action_Value
    {
        public float actionValue;
        public float maxActionValue;

        //回復行動值條
        public void Restore(float value)
        {
            actionValue = actionValue + value;
            if(actionValue > maxActionValue)
            {
                actionValue = maxActionValue;
            }
            if(actionValue < 0)
            {
                actionValue = 0;
            }
        }

        public void SetRestore(float value)
        {
            actionValue = value;
        }

        public void OverRestore(float value)
        {
            actionValue = actionValue + value;
        }

        public bool ConsumeValue(float consumeValue)
        {
            if (actionValue < consumeValue) return false;
            actionValue = actionValue - consumeValue;
            return true;
        }
    }

    public class Fight_System : MonoBehaviour
    {
        public List<Game_Actor> enemys;
        public List<Game_Actor> playerTeam;
        public Command_System commandSystem;

        // Start is called before the first frame update
        void Start()
        {
            if(commandSystem == null)
            {
                commandSystem = GameObject.Find("Command_System").GetComponent<Command_System>();
            }
        }


        // Update is called once per frame
        void Update()
        {
            PlayerAction();
            EnemysAction();
        }

        private void FixedUpdate()
        {
            
        }

        public void EnemysAction()
        {

        }

        public void PlayerAction()
        {
            
        }

        public void Fight_Data()
        {
            //player target

            //Enemys target
        }

        public void SetPlayerTarget(string targetName)
        {
            playerTeam.ForEach(actor => actor.AddTarget(targetName));
        }

        public void Attack(Game_Actor target,Game_Actor attacker)
        {
            //attacker.Attack(target);
        }

        public void UseSkillOfPlayer(Game_Actor a,string skillName)
        {
            
        }

        //QTE
        public void QTE(int id)
        {
            
        }
    }

}
