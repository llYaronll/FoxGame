using SoraHareSakura_Fight_System;
using SoraHareSakura_Game_Api;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoraHareSakura_GameApi
{ 
    [System.Serializable]
    public class Game_Character
    {
        public string name;
        public int id;
        public Level LV;
        public AttributeValue stamina;
        public AttributeValue MP;
        public int power;
        public int speed;
        public int spiritualPower;
        public float criticalHitHarm;
        public float criticalHitRate;
        public int hitRate;
        public List<Game_State> states;
        public List<Game_Skill> skills;
        public int luck;

        //figher attribute
        public int attack;
        public int magicAttack;
        public int magicDefense;
        public int defense;

        public Game_Character()
        {

            init("player1",0,new Level(),100,10,10,100,10,0,(float)0.1,(float)0.1,1,1);
        }

        public void init(string name, int id, Level lV, int stamina, int power, int speed, int mP, int spiritualPower, int defense, float criticalHitHarm, float criticalHitRate, int hitRate, int luck)
        {
            this.name = name;
            this.id = id;
            LV = lV;
            this.stamina = new AttributeValue(stamina);
            this.power = power;
            this.speed = speed;
            MP = new AttributeValue(mP);
            this.spiritualPower = spiritualPower;
            this.defense = defense;
            this.criticalHitHarm = criticalHitHarm;
            this.criticalHitRate = criticalHitRate;
            this.hitRate = hitRate;
            this.luck = luck;
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

    }

    [System.Serializable]
    public class AttributeValue
    {
        public int newValue;
        public int maxValue;
        public int initValue;

        public int upPoint;
        public float upValue;

        public AttributeValue()
        {
            newValue = 100;
            maxValue = 100;
            initValue = 100;
            upPoint = 1;
            upValue = 0;
        }

        public AttributeValue(int value)
        {
            init(value, value, value,1,0);
        }

        public void init(int newValue, int maxValue, int initValue,int upPoint,int upValue)
        {
            this.newValue = newValue;
            this.maxValue = maxValue;
            this.initValue = initValue;
            this.upPoint = upPoint;
            this.upValue = upValue;
        }

        public void AddUpPoint(int addUpPoint)
        {
            upPoint = upPoint + addUpPoint;
            SetUpPoint(upPoint)
        }

        public void SetUpPoint(int setUpPoint)
        {
            upPoint = setUpPoint;
            maxValue = (int)(initValue + upPoint*upValue);
        }

        public void AddValue(int addValue)
        {
            AddValue(addValue,false);
        }

        public void AddValue(int addValue,bool yesOver)
        {
            newValue = newValue + addValue;
            if(newValue < 0)newValue = 0;
            if(yesOver)return;
            if(newValue > maxValue) newValue = maxValue;
        }

        public bool ConsumeValue(int consumeValue)
        {
            if(newValue < consumeValue)return false;
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
    public class Game_Skill
    {
        public int id;
        public string name;
        public SkillType type;
        public ScopeOfUse scopeOfUse;
        public Situation whenToUse;
        public Game_Command consume;
        public Game_Command harm;
        public List<Game_Command> commands;
        public string imagePath;
    }
}