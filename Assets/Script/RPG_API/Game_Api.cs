using SoraHareSakura_DataBaseSystem;
using SoraHareSakura_GameApi;
using SoraHareSakura_GameData_Api;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoraHareSakura_Game_Api
{
    //General props 普通道具 special prosps 特殊道具
    [System.Serializable]
    public enum ItemType
    {
        generalProps,
        specialProps
    }

    //對誰使用
    [System.Serializable]
    public enum ScopeOfUse
    {
        none,//無對象
        user,//使用者
        partner,//隊友
        enemy//敵人
    }

    //使用場合
    [System.Serializable]
    public enum Situation
    {
        none,//無場合
        anyTime,//任何時候
        menuTime,//選單介面時
        fighting//戰鬥時
    }



    [System.Serializable]
    public class Game_Item
    {
        public int id;
        public string name;
        public ItemType type;
        public int amount;
        public int price;
        public bool isUse;
        public bool isConsumables;
        public string caption;
        public List<Game_Effect> effect;
        public ScopeOfUse scopeOfUse;
        public Situation when;
        public string imagePath;

        public Game_Item()
        {
            Init(0);
        }

        public Game_Item(int itemId)
        {
            Init(itemId);
        }

        public void Init(int itemId)
        {   
            id = itemId;
            name = "";
            type = ItemType.generalProps;
            amount = 0;
            price = 0;
            isUse = false;
            isConsumables = false;
            effect = new List<Game_Effect>();
            scopeOfUse = ScopeOfUse.none;
            when = Situation.none;
            imagePath = "";
        }

        public void Copy(Game_Item a)
        {
            JsonToThis(a.ToJson());
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }

    [System.Serializable]
    public class Game_Effect : Game_Command
    {
        public bool Run(Character_Attribute a)
        {
            bool reEnd = false;

            switch (command)
            {
                case "Restore":
                    RestoreValue(a);
                    break;
                case "AddState":
                    AddState(a);
                    break;
                case "AddEffectValue":
                    AddEffectValue(a);
                    break;
                default:
                    break;
            }
            return reEnd;
        }
        public bool RestoreValue(Character_Attribute a)
        {
            if(a == null)
            {
                return false;
            }
            string restoreAttribute = args[0];
            float valueRate = float.Parse(args[1])/100.0f;
            int value = int.Parse(args[2]);
            switch(restoreAttribute)
            {
                case "HP":
                    a.stamina.AddValue((int)(a.stamina.maxValue * valueRate));
                    a.stamina.AddValue(value);
                    break;
                case "MP":
                    a.magicPoint.AddValue((int)(a.magicPoint.maxValue * valueRate));
                    a.magicPoint.AddValue(value);
                    break;
                case "AV":
                    a.actionValue.Restore(a.actionValue.maxActionValue * valueRate);
                    a.actionValue.Restore(value);
                    break;
                default:
                    break;
            }
            return true;
        }

        public bool AddState(Character_Attribute a)
        {
            Game_State addState = new Game_State();
            GameData_StateTable stateTable = GameObject.Find("DataBase").GetComponent<Data_Base_System>().stateTable;
            addState.Copy(stateTable.game_States.Find(state => state.name.Equals(args[0])));
            if(addState == null)
            {
                return false;
            }
            a.states.Add(addState);
            return true;
        }
        
        public bool AddEffectValue(Character_Attribute a)
        {
            if(a == null)
            {
                return false;
            }
            float addValue = float.Parse(args[1]);
            switch (args[0])
            {
                case "MaxHP":
                    a.stamina.AddEffectAddValue((int)addValue);
                    break;
                case "MaxMP":
                    a.magicPoint.AddEffectAddValue((int)addValue);
                    break;
                case "MaxAV":
                    a.actionValue.AddEffectAddValue(addValue);
                    break;
                case "Power":
                    a.power.AddEffectAddValue((int)addValue);
                    break;
                case "SpiritualPower":
                    a.spiritualPower.AddEffectAddValue((int)addValue);
                    break;
                case "Speed":
                    a.speed.AddEffectAddValue((int)addValue);
                    break;
                case "attack":
                    a.attack.AddEffectAddValue((int)addValue);
                    break;
                case "magicAttack":
                    a.magicAttack.AddEffectAddValue((int)addValue);
                    break;
                default :
                    break;
            }
            return true;
        }
    }



    [System.Serializable]
    public class Game_EquipmentSlot
    {
        public int id;
        public string equipmentType;
        public Game_Equipment equipment;

        public Game_EquipmentSlot(int id,string equipmentType)
        {
            this.id = id;
            this.equipmentType = equipmentType;
        }

        public void Copy(Game_Equipment a)
        {
            this.id = a.id;
            this.equipmentType = a.equipmentType;

        }

        public bool PutOnEquipment(Game_Equipment equipment)
        {
            if (equipment.equipmentType.Equals(equipmentType))
            {
                Copy(equipment);
                return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class Game_EquipmentColumn
    {
        public List<Game_EquipmentSlot> equipmentSlots;

        public Game_EquipmentColumn()
        {
            init();
        }
        public Game_EquipmentColumn(GameData_EquipmentTypeTable equipmentTypes)
        {
            if(equipmentTypes == null)
            {
                init();
                return;
            }
            int arrayId = 0;
            foreach(string equipmentType in equipmentTypes.equipmentType)
            {
                Game_EquipmentSlot slot = new(arrayId, equipmentType);
                equipmentSlots.Add(slot);
            }
        }

        public void init()
        {
            equipmentSlots = new List<Game_EquipmentSlot>();
        }

        public float SumAttackValue()
        {
            return 0;
        }
    }

    [System.Serializable]
    public class Game_Equipment
    {
        public int id;
        public string equipmentName;
        public string equipmentType;
        public List<GameCommand_AddAttributeValue> addAttributeValues;

        public Game_Equipment()
        {
            
        }

        public Game_Equipment(int id, string equipmentName, string equipmentType, List<GameCommand_AddAttributeValue> addAttributeValues)
        {
            this.id = id;
            this.equipmentName = equipmentName;
            this.equipmentType = equipmentType;
            this.addAttributeValues = addAttributeValues;
        }

        public Game_Equipment(Game_Equipment a)
        {
            id = a.id;
            equipmentName = a.equipmentName;
            equipmentType = a.equipmentType;
            addAttributeValues = a.addAttributeValues;
        }

        public void init(int id, string equipmentName, string equipmentType, List<GameCommand_AddAttributeValue> addAttributeValues)
        {
            this.id = id;
            this.equipmentName = equipmentName;
            this.equipmentType = equipmentType;
            this.addAttributeValues = addAttributeValues;
        }
    }

    public class GameCommand_AddAttributeValue : Game_Command
    {
        public int AddValueInt()
        {
            return (int)float.Parse(args[0]);
        }

        public float AddValueFloat()
        {
            return float.Parse(args[0]);
        }
    }

    [System.Serializable]
    public class Character_Attribute
    {
        //character data
        public int id;
        public string name;

        //character level
        public Level level;

        //user can to up level
        //main up figher attribute
        public AttributeValue stamina;
        public AttributeValue magicPoint;
        public AttributeValue power;
        public AttributeValue spiritualPower;
        public AttributeValue speed;

        //use equipment to up value
        //main use figher
        public AttributeValue attack;
        public AttributeValue magicAttack;
        public AttributeValue magicDefense;
        public AttributeValue defense;
        public AttributeValueFloat criticalHitHarm;
        public AttributeValueFloat criticalHitRate;
        public AttributeValue hitRate;
        public AttributeValue luck;

        public Action_Value actionValue;

        //state
        public Game_EquipmentColumn equipmentSlots;
        public List<Game_State> states;
        public List<Game_Skill> skills;

        public void Copy(Character_Attribute a)
        {
            JsonToThis(a.ToJson());
            /*this.id = a.id;
            this.name = a.name;
            this.level.Copy(a.level);
            this.stamina.Copy(a.stamina);
            magicPoint.Copy(a.magicPoint);
            power.Copy(a.power);
            spiritualPower.Copy(a.spiritualPower);
            speed.Copy(a.speed);
            attack.Copy(a.attack);
            magicAttack.Copy(a.magicAttack);
            magicDefense.Copy(a.magicDefense);
            defense.Copy(a.defense);
            criticalHitHarm.Copy(a.criticalHitHarm);
            criticalHitRate.Copy(a.criticalHitRate);
            hitRate.Copy(a.hitRate);
            luck.Copy(a.luck);
            actionValue.Copy(a.actionValue);
            equipmentSlots = a.equipmentSlots;*/
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string a)
        {
            JsonUtility.FromJsonOverwrite(a, this);
        }
    }


    [System.Serializable]
    public class Level
    {
        public int level;//等級
        public int upLevelExperiencePoint;//升級所需經驗值
        public int experiencePoint;//經驗值
        public int upPoint;//升級點
        public int initUpLevelPoint;//initialization
        public int growthValue;//經驗成長值
        public int growthUpPoint;//升級點成長值

        public Level()
        {
        }
        public void Copy(Level copyObject)
        {
            JsonToThis(copyObject.ToJson());
        }

        //經驗值成長函數
        public int EXGrowthFunction()
        {
            return growthValue * level + upLevelExperiencePoint;
        }

        //升級點成長函數
        public int UpPointGrowthFunction()
        {
            return growthUpPoint;
        }

        //設定經驗值
        public void SetExperience(int exValue)
        {
            experiencePoint = exValue;
            LevelUp();
        }

        //增加經驗值
        public void AddExperiencePoint(int exValue)
        {
            SetExperience(exValue + experiencePoint);
        }

        //等級提升
        public void LevelUp()
        {
            int newEX = experiencePoint - upLevelExperiencePoint;
            while (newEX >= 0)
            {
                experiencePoint = newEX;
                AddLevel();
                newEX = experiencePoint - upLevelExperiencePoint;
            }
        }

        //增加一個等級
        public void AddLevel()
        {
            level = level + 1;
            upLevelExperiencePoint = EXGrowthFunction();
            upPoint = upPoint + UpPointGrowthFunction();
        }

        //增加等級 輸入增加等級值
        public void AddLevel(int levelValue)
        {
            for (int i = 0; i < levelValue; i++)
            {
                AddLevel();
            }
        }

        //初始化升級點
        public void initUpPoint()
        {
            upPoint = level * UpPointGrowthFunction() + initUpLevelPoint;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }

    public class Attribute<numberType>
    {
        public numberType newValue;//現在值
        public numberType maxValue;//最大值 也用作固定值
        public numberType initValue;//初始值
        public numberType effectAddValue;//狀態效果增加最大值的值

        //public List<int> equipmentValue;
        public numberType equipmentSumValue;//裝備增加最大值的值

        public int upPoint;//升級點
        public float upValue;//每點升多少值

        public void Copy(Attribute<numberType> A)
        {
            newValue = A.newValue;
            maxValue = A.maxValue;
            initValue = A.initValue;
            effectAddValue = A.effectAddValue;
            equipmentSumValue = A.equipmentSumValue;
            upPoint = A.upPoint;
            upValue = A.upValue;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }

    [System.Serializable]
    public class AttributeValue : Attribute<int>
    {
        //public int newValue;
        //public int maxValue;
        //public int initValue;
        //public int effectAddValue;

        //public List<int> equipmentValue;
        //public int equipmentSumValue;

        //public int upPoint;//升級點
        //public float upValue;//每點升多少值

        public AttributeValue()
        {
            newValue = 100;
            maxValue = 100;
            initValue = 100;
            upPoint = 0;
            upValue = 0;
            effectAddValue = 0;
            //equipmentValue = new List<int>();
            equipmentSumValue = 0;
        }

        public AttributeValue(int value)
        {
            init(value, value, value, 0, 0);
        }

        public void init(int newValue, int maxValue, int initValue, int upPoint, float upValue)
        {
            this.newValue = newValue;
            this.maxValue = maxValue;
            this.initValue = initValue;
            this.upPoint = upPoint;
            this.upValue = upValue;
            effectAddValue = 0;
            equipmentSumValue = 0;
        }

        public void AddUpPoint(int addUpPoint)
        {
            upPoint = upPoint + addUpPoint;
            SetUpPoint(upPoint);
        }

        public void SetUpPoint(int setUpPoint)
        {
            upPoint = setUpPoint;
            UpMaxValueData();
        }

        public void SetEffectAddValue(int value)
        {
            effectAddValue = value;
            UpMaxValueData();
        }

        public void AddEffectAddValue(int addValue)
        {
            SetEffectAddValue(effectAddValue + addValue);
        }

        public void UpDataEquipment(List<int> equipmentValue)
        {
            equipmentSumValue = equipmentValue.Sum();
            UpMaxValueData();
        }

        public void UpMaxValueData()
        {
            //int sumEquipmentValue = equipmentValue.Sum();
            //maxValue = (int)(initValue + effectAddValue + sumEquipmentValue + upPoint*upValue);
            maxValue = (int)(initValue + effectAddValue + equipmentSumValue + upPoint * upValue);
        }

        public void AddValue(int addValue)
        {
            AddValue(addValue, false);
        }

        public void AddValue(int addValue, bool yesOver)
        {
            newValue = newValue + addValue;
            if (newValue < 0) newValue = 0;
            if (yesOver) return;
            if (newValue > maxValue) newValue = maxValue;
        }

        public bool ConsumeValue(int consumeValue)
        {
            if (newValue < consumeValue) return false;
            newValue = newValue - consumeValue;
            return true;
        }
    }

    [System.Serializable]
    public class AttributeValueFloat : Attribute<float>
    {

        public AttributeValueFloat()
        {
            newValue = 100;
            maxValue = 100;
            initValue = 100;
            upPoint = 0;
            upValue = 0;
            effectAddValue = 0;
            equipmentSumValue = 0;
        }

        public AttributeValueFloat(float value)
        {
            init(value, value, value, 0, 0);
        }

        public void init(float newValue, float maxValue, float initValue, int upPoint, float upValue)
        {
            this.newValue = newValue;
            this.maxValue = maxValue;
            this.initValue = initValue;
            this.upPoint = upPoint;
            this.upValue = upValue;
            effectAddValue = 0;
            equipmentSumValue = 0;
        }

        public void AddUpPoint(int addUpPoint)
        {
            upPoint = upPoint + addUpPoint;
            SetUpPoint(upPoint);
        }

        public void SetUpPoint(int setUpPoint)
        {
            upPoint = setUpPoint;
            UpMaxValueData();
        }

        public void SetEffectAddValue(float value)
        {
            effectAddValue = value;
            UpMaxValueData();
        }

        public void AddEffectAddValue(float addValue)
        {
            SetEffectAddValue(effectAddValue + addValue);
        }

        public void UpDataEquipment(List<float> equipmentValue)
        {
            equipmentSumValue = equipmentValue.Sum();
            UpMaxValueData();
        }

        public void UpMaxValueData()
        {
            maxValue = initValue + effectAddValue + equipmentSumValue + upPoint * upValue;
        }

        public void AddValue(float addValue)
        {
            AddValue(addValue, false);
        }

        public void AddValue(float addValue, bool yesOver)
        {
            newValue = newValue + addValue;
            if (newValue < 0) newValue = 0;
            if (yesOver) return;
            if (newValue > maxValue) newValue = maxValue;
        }

        public bool ConsumeValue(float consumeValue)
        {
            if (newValue < consumeValue) return false;
            newValue = newValue - consumeValue;
            return true;
        }
    }

    [System.Serializable]
    public enum SkillType
    {
        none,
        magic,
        physics
    }

    [System.Serializable]
    public enum SkillHarmType
    {
        none,
        HPHarm,
        MPHarm,
        AVHarm,
        HPRecover,
        MPRecover,
        AVRecover
    }

    [System.Serializable]
    public class Game_Skill
    {
        public int id;
        public string name;
        public SkillType type;
        public ScopeOfUse scopeOfUse;
        public Situation whenToUse;
        public int useSkillDistance;
        //public string attackType;
        public ConsumeCommand consume;
        public SkillHarmType skillHarmType;
        public string harm;//計算傷害函式 目前沒用
        public bool isCritical;
        public List<Game_Effect> commands;
        public string imagePath;

        public Game_Skill()
        {
            id = 0;
            name = "";
            type = SkillType.none;
            scopeOfUse = ScopeOfUse.none;
            whenToUse = Situation.none;
            commands = new List<Game_Effect>();
            consume = new ConsumeCommand();
            consume.command = "consumeHP";
            consume.args.Add("0");
            skillHarmType = SkillHarmType.none;
            isCritical = false;
            harm = "";
            imagePath = "";
        }

        public void UseSkill(Character_Attribute target)
        {
            commands.ForEach(command =>
            {
                command.Run(target);
            });
        }

        public bool UseSkillOk(Character_Attribute user)
        {
            return consume.Consume(user);
        }

        public float SkillConsumeActionValue()
        {
            return consume.ConsumeAV();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }

    [System.Serializable]
    public class ConsumeCommand : Game_Command
    {
        public bool Consume(Character_Attribute target)
        {
            bool reEnd = false;
            float consumeValue = float.Parse(args[0]);
            switch (command)
            {
                case "consumeHP":
                    reEnd = target.stamina.ConsumeValue((int)consumeValue);
                    break;
                case "consumeMP":
                    reEnd = target.magicPoint.ConsumeValue((int)consumeValue);
                    break;
                case "consumeAV":
                    reEnd = target.actionValue.ConsumeValue(consumeValue);
                    break;
                default:
                    break;
            }
            return reEnd;
        }

        public float ConsumeAV()
        {
            float consumeValue = float.Parse(args[0]);
            if (command.Equals("consumeAV"))
            {
                return consumeValue;
            }
            return 0;
        }
    }

    [System.Serializable]
    public class Action_Value
    {
        public float actionValue;//行動值
        public float maxActionValue;//最大行動值

        public float initValue;//初始行動值
        public float effectAddValue;//狀態效果增加的最大值
        public float buffValue;//

        //public List<float> equipmentValue;
        public float equipmentSumValue;//裝備增加的最大值

        public Action_Value()
        {
            actionValue = 0;
            maxActionValue = 100;

            initValue = 100;
            effectAddValue = 0;
            //equipmentValue = new List<float>();
            equipmentSumValue = 0;
        }

        public void Copy(Action_Value a)
        {
            actionValue = a.actionValue;
            maxActionValue = a.maxActionValue;
            initValue = a.initValue;
            effectAddValue = a.effectAddValue;
            equipmentSumValue = a.equipmentSumValue;
            buffValue = a.buffValue;
            equipmentSumValue = a.equipmentSumValue;

        }

        //回復行動值條
        public void Restore(float value)
        {
            actionValue = actionValue + value;
            if (actionValue > maxActionValue)
            {
                actionValue = maxActionValue;
            }
            if (actionValue < 0)
            {
                actionValue = 0;
            }
        }

        //設定行動值
        public void SetRestore(float value)
        {
            actionValue = value;
        }

        //回復行動值 可以超過上下限
        public void OverRestore(float value)
        {
            actionValue = actionValue + value;
        }

        //消耗行動值
        public bool ConsumeValue(float consumeValue)
        {
            if (actionValue < consumeValue) return false;
            actionValue = actionValue - consumeValue;
            return true;
        }

        //設定裝備後最大行動值增減
        public void UpDataEquipmentValue(List<float> equipmentValue)
        {
            equipmentSumValue = equipmentValue.Sum();
            UpMaxValueData();
        }

        //設定狀態後最大行動值增減
        public void UpDataBuffValue(List<float> buffValueSum)
        {
            buffValue = buffValueSum.Sum();
        }

        //設定使用物品後行動值增減
        public void AddEffectAddValue(float addValue)
        {
            effectAddValue = effectAddValue + addValue;
        }

        //更新最大行動值
        public void UpMaxValueData()
        {
            //float sumEquipmentValue = equipmentValue.Sum();
            //maxActionValue = initValue + effectAddValue + buffValue + sumEquipmentValue;
            maxActionValue = initValue + effectAddValue + buffValue + equipmentSumValue;
        }

        public bool ValueIsMax()
        {
            return actionValue >= maxActionValue;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }

    /*[System.Serializable]
    public class Target_Selector
    {
        public int queuePosition;
        public List<Game_Actor> enemys;
        public List<Game_Actor> partners;

        //優先選擇我方值
        public List<int> partnerPriority;

        //優先選擇敵方值
        public List<int> enemyPriority;

        public Target_Selector()
        {

        }

        public Target_Selector(int queuePosition,List<Game_Actor> enemys,List<Game_Actor> partners)
        {
            this.queuePosition = queuePosition;
            InitEnemysAndPartners(enemys, partners);
        }

        public Target_Selector(int queuePosition, List<Game_Actor> enemys, List<Game_Actor> partners, List<int> partnerPriority, List<int> enemyPriority)
        {
            this.queuePosition = queuePosition;
            InitEnemysAndPartners(enemys, partners);
            this.enemyPriority = enemyPriority;
            this.partners = partners;
        }



        //初始化敵我列表 與 敵我目標順序
        public void InitEnemysAndPartners(List<Game_Actor> enemys, List<Game_Actor> partners)
        {
            this.enemys = enemys;
            this.partners = partners;
            enemyPriority = new List<int>();
            partnerPriority = new List<int>();
            
            for (int i = 0; i < enemys.Count; i++)
            {
                enemyPriority.Add(enemys[i].target_selector.queuePosition + 1);
            }

            for (int j = 0; j < partners.Count; j++)
            {
                partnerPriority.Add(partners[j].target_selector.queuePosition + 1);
            }
        }

        //AI 
        public List<Game_Actor> SelectAttackTarget(ScopeOfUse targetSelectType,int useSkillDistance)
        {
            List<Game_Actor> result = new List<Game_Actor>();
            bool isEnemy = false;
            int targetId = 0;
            int SelectQueuePosition = queuePosition + useSkillDistance;//0最後位 1第二位 2前鋒 3敵前鋒 4敵中鋒 5敵後位  攻擊距離 1 2 3 4 5 6
            if(SelectQueuePosition == 4)
            {
                SelectQueuePosition = 2;
            }
            if(SelectQueuePosition == 5)
            {
                SelectQueuePosition = 1;
            }
            if(SelectQueuePosition == 3)
            {
                SelectQueuePosition = 0;
            }
            switch (targetSelectType)
            {
                case ScopeOfUse.none:
                    targetId = -1;
                    break;
                case ScopeOfUse.user:
                    targetId = -1;
                    break;
                case ScopeOfUse.partner:
                    targetId = partnerPriority.IndexOf(partnerPriority.Max());
                    break;
                case ScopeOfUse.enemy:
                    targetId = enemyPriority.Max();
                    List<int> priority = new List<int>();
                    for(int i = 0; i < enemyPriority.Count; i++)
                    {
                        if(enemys[i].target_selector.queuePosition >= SelectQueuePosition)
                        {
                            priority.Add(enemyPriority[i]);
                        }
                        else
                        {
                            priority.Add(-1);
                        }
                    }
                    targetId = enemyPriority.IndexOf(priority.Max());
                    if (priority[targetId] < 0 && targetId >= 0)
                    {
                        targetId = -1;
                    }
                    break;
                default:
                    break;
            }

            if(targetId < 0)
            {
                return null;
            }
            if (isEnemy)
            {
                result.Add(enemys[targetId]);
            }
            else
            {
                result.Add(partners[targetId]);
            }
            return result;
        }

        public void SetTarget(Game_Actor target,int priority,bool isPartner)
        {
            int targetId = 0;
            if (isPartner)
            {
                for(int i = 0;i < partners.Count; i++)
                {
                    if (target.Equals(partners[i].name))
                    {
                        targetId = i;
                    }
                }
                partnerPriority[targetId] = priority;
            }
            else
            {
                for(int j = 0;j < enemys.Count; j++)
                {
                    if (target.Equals(enemys[j].name))
                    {
                        targetId = j;
                    }
                }
                enemyPriority[targetId] = priority;
            }
        }
    }
    */

}
