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
using Unity.VisualScripting;
using System.ComponentModel;
using System.Linq;
using SoraHareSakura_DataBaseSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.Build;
using Random = UnityEngine.Random;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine.XR;

namespace SoraHareSakura_Fight_System
{
    [System.Serializable]
    public class Game_Actor : Character_Attribute
    {
        public Game_Actor()
        {
            this.id = 0;
            this.name = "";
            this.level = new Level();

            this.stamina = new AttributeValue();
            this.magicPoint = new AttributeValue();
            this.actionValue = new Action_Value();

            this.power = new AttributeValue(10);
            this.spiritualPower = new AttributeValue(10);
            this.speed = new AttributeValue(10);

            this.attack = new AttributeValue(0);
            this.defense = new AttributeValue(0);
            this.magicAttack = new AttributeValue(0);
            this.magicDefense = new AttributeValue(0);

            this.criticalHitHarm = new AttributeValueFloat(0);
            this.criticalHitRate = new AttributeValueFloat(0);
            this.hitRate = new AttributeValue(10);
            this.luck = new AttributeValue(10);

            this.equipmentSlots = new Game_EquipmentColumn();
            this.states = new List<Game_State>();
            this.skills = new List<Game_Skill>();
        }
          
        public Game_Actor(Character_Attribute character)
        {
            Copy(character);
        }

        public void TimeFlow(float elapsedTime)
        {
            //行動值
            float restoreActionValueRate = 1;
            actionValue.Restore(elapsedTime * speed.maxValue * restoreActionValueRate);

            //狀態時間
            states.ForEach(state =>
            {
                if (state.type == StateType.Aging)
                {
                    state.TimeFlow(elapsedTime);
                }
            });

            //當狀態時間歸零時 解除狀態
            states.RemoveAll(state => state.timeLeft == 0 && state.type == StateType.Aging);
        }

        //使用技能
        public Game_Skill Skill(string skillName)
        {
            Game_Skill useSkill = skills.Find(skill => skill.name == skillName);
            return useSkill;
        }
        public bool UseSkill(string skillName)
        {
            Game_Skill useSkill = skills.Find(skill => skill.name == skillName);
            if (useSkill == null) return false;
            bool isUse = useSkill.consume.Consume(this);
            return isUse;
        }

        //添加狀態 
        public void AddState(Game_State state)
        {
            bool isAdd = false;
            for(int i = 0;i < states.Count; i++)
            {
                if (states[i].name == state.name)
                {
                    if (states[i].type == StateType.Aging) {
                        states[i].timeLeft = state.duration;
                        isAdd = true;
                        break;
                    }
                }
            }
            if(isAdd)return;
            states.Add(state);
        }

        public void RunState()
        {
            foreach(Game_State state in states)
            {
                state.Run(this);
            }
        }    }

    [System.Serializable]
    public class Attack_ModeTable
    {
        List<Attack_Mode> attackModes;

        public void Init()
        {
            if(attackModes == null)
            {
                attackModes = new List<Attack_Mode>();
            }
        }
        public void AddAttackMode(Attack_Mode item)
        {
            attackModes.Add(item);
        }
        public string Run(int round, int hpRatio, int mpRatio, List<string> stateName)
        {
            List<string> attackId = new List<string>();
            foreach(Attack_Mode _AttackMode in attackModes)
            {
                if (_AttackMode.AttackIF(round, hpRatio, mpRatio, stateName)){
                    for(int i = 0; i < _AttackMode.Score(); i++)
                    {
                        attackId.Add(_AttackMode.command);
                    }
                }
            }
            int aR = UnityEngine.Random.Range(0, attackId.Count-1);
            return attackId[aR];
        }
    }

    [System.Serializable]
    public class Attack_Mode: Game_Command
    { 
        //attackMode = skill if rateValue
        public string AttackMode()
        {
            return command;
        }

        public bool AttackIF(int round,int hpRatio,int mpRatio,List<string> stateName)
        {
            (string ifCommandString,List<string> ifArgsString) = CommandParser(args[0],',');//command,args1,args2

            bool ifEnd = false; 
            List<int> range = new List<int>();

            switch (ifCommandString)
            {
                case "Always":
                    ifEnd = true;
                    break;
                case "Round":
                    int startRound = int.Parse(ifArgsString[0]);
                    int nextRound = int.Parse(ifArgsString[1]);
                    ifEnd = IsRound(round, startRound, nextRound);
                    break;
                case "HP":
                    range.Add(int.Parse(ifArgsString[0]));
                    range.Add(int.Parse(ifArgsString[1]));
                    ifEnd = IsRange(hpRatio,range);
                    break;
                case "MP":
                    range.Add(int.Parse(ifArgsString[0]));
                    range.Add(int.Parse(ifArgsString[1]));
                    ifEnd = IsRange(mpRatio, range);
                    break;
                case "State":
                    foreach (string i in stateName)
                    {
                        if (ifArgsString[0].Equals(i)) { ifEnd = true; break; }
                    }
                    break;
                default:
                    break;
            }

            return ifEnd;
        }

        public bool IsRound(int round,int startRound,int nextRound)
        {
            if(round == startRound)
            {
                return true;
            }
            if((round-startRound)%nextRound == 0)
            {
                return true;
            }
            return false;
        }

        public bool IsRange(int value,List<int> range)
        {
            range.Sort();
            if(value >= range[0] && value <= range[1])
            {
                return true;
            }
            return false;
        }

        public int Score()
        {
            return int.Parse(args[1]);
        }
    }

    //優先表 用於設定攻擊目標
    [System.Serializable]
    public class PriorityTable
    {
        public List<Priority_Obj> priorityObjs;

        public PriorityTable(List<Priority_Obj> priorityObjs)
        {
            this.priorityObjs = priorityObjs;
        }

        public PriorityTable()
        {
            priorityObjs = new List<Priority_Obj>();
            for(int i = 0; i < priorityObjs.Count; i++)
            {
                AddObj(i,0);
            }
        }

        public void InitSort()
        {
            for(int i = 0; i < priorityObjs.Count; i++)
            {
                priorityObjs[i].priorityValue = priorityObjs.Count - i;
            }
        }

        public void InitTable(int priorityListCount)
        {
            priorityObjs = new List<Priority_Obj>();
            for (int i = 0; i < priorityListCount; i++)
            {
                Priority_Obj newPriorityObj = new Priority_Obj();
                newPriorityObj.id = i;
                newPriorityObj.priorityValue = i;
                priorityObjs.Add(newPriorityObj);
            }
            InitSort();
        }

        public void InitTable(List<int> priorityList)
        {
            priorityObjs = new List<Priority_Obj>();
            for(int i = 0; i < priorityList.Count; i++)
            {
                Priority_Obj newPriorityObj = new Priority_Obj();
                newPriorityObj.id = i;
                newPriorityObj.priorityValue = priorityList[i];
                priorityObjs.Add(newPriorityObj);
            }
            InitSort();
        }

        //找最大優先的 目標Id
        public int TopPriorityId()
        {
            Sort();
            return priorityObjs[0].id;
        }

        //找最大優先 目標Id 用id表來選擇查找範圍
        public int TopPriorityId(List<int> idTable)
        {
            PriorityTable regTable = new PriorityTable();
            for (int id = 0; id < idTable.Count; id++)
            {
                regTable.priorityObjs.Add(Find(idTable[id]));
            }
            regTable.Sort();
            return regTable.priorityObjs[0].id;
        }

        //回傳最大優先表 用id表來選擇回傳範圍
        public PriorityTable PriorityListRange(List<int> idTable)
        {
            PriorityTable regTable = new PriorityTable();
            for (int id = 0; id < idTable.Count; id++)
            {
                regTable.priorityObjs.Add(Find(idTable[id]));
            }
            regTable.Sort();
            return regTable;
        }

        //找第number優先的 目標Id
        public int NumberPriorityId(int number)
        {
            Sort();
            number = BoundsCheck(number);
            return priorityObjs[number].id;
        }

        //找number個優先的 目標Id
        public List<int> NumberPriorityIds(int end)
        {
            return NumberPriorityIds(0,end);
        }

        public List<int> NumberPriorityIds(int start,int end)
        {
            start = BoundsCheck(start);
            end = BoundsCheck(end);
            if(start > end) {int reg = start; start = end; end = reg; }
            List<int> reid = new List<int>();
            for(int i = start; i <= end; i++)
            {
                reid.Add(priorityObjs[i].id);
            }
            return reid;
        }

        //檢查邊界
        public int BoundsCheck(int number)
        {
            if (number < 0) number = 0;
            if (number > priorityObjs.Count) number = priorityObjs.Count - 1;
            return number;
        }

        //將指定id 移到 number位置 (0~count-1)
        public void ToNumber(int id,int number)
        {
            number = BoundsCheck(number);
            Priority_Obj startA = Find(id);
            priorityObjs.Remove(startA);
            priorityObjs.Insert(number, startA);
        }

        //排序
        public void Sort()
        {
            priorityObjs.Sort((a, b) => b.priorityValue.CompareTo(a.priorityValue));
        }

        //增加優先順序
        public void AddObj(int id,int priorityValue)
        {
            Priority_Obj k = new Priority_Obj();
            k.id = id;
            k.priorityValue = priorityValue;
            priorityObjs.Add(k);
        }

        //用id 找優先物件
        public Priority_Obj Find(int id)
        {
            id = BoundsCheck(id);
            return priorityObjs.Find(x => x.id == id);
        }

        //增加優先值
        public void AddPriority(int id,int addValue)
        {
            id = BoundsCheck(id);
            Find(id).priorityValue += addValue;
        }
        
        //設定優先值
        public void SetPriority(int id,int priorityValue)
        {
            id = BoundsCheck(id);
            Find(id).priorityValue = priorityValue;
        }

        //用列表設定優先值 id為優先值的ID
        public void SetPriority(List<int> priorityValueTable)
        {
            for(int i = 0;i < priorityValueTable.Count; i++)
            {
                Find(i).priorityValue = priorityValueTable[i];
            }
        }

        public string ToArrayString()
        {
            string ru = "";
            for (int i = 0; i < priorityObjs.Count; i++)
            {
                ru += "(" + priorityObjs[i].id.ToString() + "," + priorityObjs[i].priorityValue.ToString() + "),";
            }
            return ru;
        }

    }

    //優先物件 方便排序
    [System.Serializable]
    public class Priority_Obj
    {
        public int id;
        public int priorityValue;
    }

    public class Fight_System : MonoBehaviour
    {
        public List<Game_Batter> enemyTeam;
        public List<Game_Batter> playerTeam;
        public List<List<int>> enemyArea;
        public List<List<int>> playerArea;
        public Command_System commandSystem;
        public GameData_StateTable gameDataStateTable;
        public GameData_SkillTable gameDataSkillTable;
        public Target_Selector targetSelectorEnemy;
        public Target_Selector targetSelectorPartner;
        public float oneTime;


        // Start is called before the first frame update
        void Start()
        {
            oneTime = 0;
            if (commandSystem == null)
            {
                commandSystem = GameObject.Find("Command_System").GetComponent<Command_System>();
                gameDataStateTable = GameObject.Find("DataBase").GetComponent<Data_Base_System>().stateTable;
                gameDataSkillTable = GameObject.Find("DataBase").GetComponent<Data_Base_System>().skillTable;
            }
            enemyArea = new List<List<int>>();
            playerArea = new List<List<int>>();

            for (int i = 0;i < 3;i++)//0 1 2
            {
                enemyArea.Add(new List<int>());
                playerArea.Add(new List<int>());
            }

            for(int i = 0; i < enemyTeam.Count; i++)//i=id
            {
                enemyArea[enemyTeam[i].queuePosition].Add(i);
                enemyTeam[i].partnerPriority = new PriorityTable();
                enemyTeam[i].partnerPriority.InitTable(enemyTeam.Count);
                enemyTeam[i].enemyPriority = new PriorityTable();
                enemyTeam[i].enemyPriority.InitTable(playerTeam.Count);
            }

            for(int j = 0; j < playerTeam.Count; j++)//j=id
            {
                playerArea[playerTeam[j].queuePosition].Add(j);
                playerTeam[j].partnerPriority = new PriorityTable();
                playerTeam[j].partnerPriority.InitTable(playerTeam.Count);
                playerTeam[j].enemyPriority = new PriorityTable();
                playerTeam[j].enemyPriority.InitTable(enemyTeam.Count);
            }
        }

        public void LinkTeam()
        {
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
            for(int i = 0; i < enemyTeam.Count; i++)
            {
                float actionValue = enemyTeam[i].gameActor.actionValue.actionValue;
                if (enemyTeam[i].NextSkill == null || enemyTeam[i].NextSkill.Equals("")) {
                    enemyTeam[i].AISelectSkill();
                }
                if (enemyTeam[i].gameActor.actionValue.ValueIsMax())
                {
                    AttackState(enemyTeam[i], enemyTeam[i].NextSkill);
                    enemyTeam[i].AISelectSkill();
                }
                else
                {
                    enemyTeam[i].gameActor.actionValue.Restore(Time.deltaTime);
                }
            }
        }

        public void PlayerAction()
        {
            //將玩家選擇的目標分享給玩家團隊
            List<int> tse = targetSelectorEnemy.ToOrder();
            List<int> tsp = targetSelectorPartner.ToOrder();
            for(int i =0;i < playerTeam.Count; i++)
            {
                playerTeam[i].enemyPriority.SetPriority(tse);
                playerTeam[i].partnerPriority.SetPriority(tsp);
                playerTeam[i].gameActor.actionValue.Restore(Time.deltaTime);
            }
        }

        public void PlayerSetTarget(Game_Actor target,int order)
        {
            bool isP = playerTeam.Exists(x => x.gameActor == target);
        }

        public void EnemysSetTarget()
        {
        }

        public void Fight_Data()
        {
            //player target

            //Enemys target
        }

        public Game_Skill FindSkill(string skillName)
        {
            return gameDataSkillTable.skillTable.Find(skill => skill.name.Equals(skillName));
        }

        public void ButtonAttacker(string skillName)
        {
            AttackState(playerTeam[0],skillName);
        }

        public void AttackState(Game_Batter attacker,string skillName)
        {
            if(attacker == null)
            {
                Debug.Log("null");
                return;
            }
            for(int i = 0;i < attacker.gameActor.states.Count; i++)
            {
                if(attacker.gameActor.states[i] != null)
                {
                    if (attacker.gameActor.states[i].limitType == LimitAction.unableToAct)
                    {
                        Debug.Log("No fight");
                        return;
                    }
                }
                
            }

            bool k = attacker.gameActor.UseSkill(skillName);
            if (!k) {
                Debug.Log("no use skill");
                return;
            }
            
            AttackSop(attacker,skillName);
        }

        //戰鬥流程
        public void AttackSop(Game_Batter attacker,string skillName)
        {
            List<int> targets = new List<int>();
            int skillD = attacker.gameActor.skills.Find(skill => skill.name == skillName).useSkillDistance;
            ScopeOfUse scopeOfUse = attacker.gameActor.skills.Find(skill => skill.name == skillName).scopeOfUse;
            int skillEnd = skillD - attacker.queuePosition;//
            skillEnd = Math.Clamp(skillEnd, 0, enemyArea.Count);
            int targetId = -1;

            //找目標
            
            switch (scopeOfUse)
            {
                case ScopeOfUse.user:
                    Attack(attacker.gameActor, attacker.gameActor, skillName);
                    break;
                case ScopeOfUse.enemy:
                    for (int i = 0; i < skillEnd; i++)
                    {
                        foreach (int pieceId in enemyArea[i])
                        {
                            targets.Add(pieceId);
                        }
                    }
                    if(targets.Count > 0)
                    {
                        targetId = attacker.enemyPriority.TopPriorityId(targets);
                    }
                    if(enemyTeam.Exists(gameBatter => gameBatter == attacker))
                    {
                        Attack(playerTeam[targetId].gameActor, attacker.gameActor, skillName);
                    }
                    else
                    {
                        Attack(enemyTeam[targetId].gameActor,attacker.gameActor,skillName);
                    }
                    break;
                case ScopeOfUse.partner:
                    for (int i = 0; i < skillEnd; i++)
                    {
                        foreach (int pieceId in playerArea[i])
                        {
                            targets.Add(pieceId);
                        }
                    }
                    targetId = attacker.partnerPriority.TopPriorityId(targets);
                    if (enemyTeam.Exists(gameBatter => gameBatter == attacker))
                    {
                        Attack(enemyTeam[targetId].gameActor, attacker.gameActor, skillName);
                    }
                    else
                    {
                        Attack(playerTeam[targetId].gameActor, attacker.gameActor, skillName);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Attack(Game_Actor target, Game_Actor attacker, string skillName)
        {
            float damage = AttackDamageValue(target, attacker, skillName);
            List<Game_State> addStates = new List<Game_State>();

            SkillHarmType skillHarmType = FindSkill(skillName).skillHarmType;
            switch (skillHarmType)
            {
                case SkillHarmType.HPHarm:
                    target.stamina.AddValue((int)-damage);
                    break;
                case SkillHarmType.MPHarm:
                    target.magicPoint.AddValue((int)-damage);
                    break;
                case SkillHarmType.AVHarm:
                    target.actionValue.Restore((int)-damage);
                    break;
                case SkillHarmType.HPRecover:
                    target.stamina.AddValue((int)damage);
                    break;
                case SkillHarmType.MPRecover:
                    target.magicPoint.AddValue((int)damage);
                    break;
                case SkillHarmType.AVRecover:
                    target.actionValue.Restore((int)damage);
                    break;
                default:
                    break;
            }
            
        }

        //計算攻擊基礎傷害值
        public float AttackDamageValue(Game_Actor target, Game_Actor attacker,string skillName)
        {
            SkillType skillType = FindSkill(skillName).type;
            float attackValue = 0;
            float defenseValue = 0;
            switch (skillType)
            {
                case SkillType.magic:
                    attackValue = attacker.magicAttack.maxValue + attacker.spiritualPower.maxValue;
                    defenseValue = target.magicDefense.maxValue;
                    break;
                case SkillType.physics:
                    attackValue = attacker.attack.maxValue + attacker.power.maxValue;
                    defenseValue = target.defense.maxValue;
                    break;
                case SkillType.none:
                    attackValue = attacker.attack.maxValue + attacker.magicAttack.maxValue + attacker.power.maxValue + attacker.spiritualPower.maxValue;
                    defenseValue = target.defense.maxValue + target.magicDefense.maxValue;
                    break;
                default:
                    break;
            }
            
            float damage = attackValue - defenseValue;
            Debug.Log(attacker.name + " add " + damage + " to " + target.name);
            float criticalValue = TryCritical(attacker) + 1;
            damage = damage * criticalValue;
            if(damage > 0)return damage;
            return 0.0f;
        }

        public float TryCritical(Game_Actor a)
        {
            float randomValue = Random.Range(0.0f, 1.0f);
            if(randomValue < 0.0f)return 0.0f;
            if(randomValue < a.criticalHitRate.maxValue)return a.criticalHitHarm.maxValue;
            return 0.0f;
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
