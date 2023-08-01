using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_GameApi
{

    [System.Serializable]
    public class Game_Character
    {
        public string name;
        public int id;
        public Level LV;
        public int stamina;
        public int power;
        public int speed;
        public int MP;
        public int spiritualPower;
        public int defense;
        public float criticalHitHarm;
        public float criticalHitRate;
        public int hitRate;
        public int luck;

        public Game_Character()
        {

            init("player1",0,new Level(),100,10,10,100,10,0,(float)0.1,(float)0.1,1,1);
        }

        public void init(string name, int id, Level lV, int stamina, int power, int speed, int mP, int spiritualPower, int defense, float criticalHitHarm, float criticalHitRate, int hitRate, int luck)
        {
            this.name = name;
            this.id = id;
            LV = lV;
            this.stamina = stamina;
            this.power = power;
            this.speed = speed;
            MP = mP;
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
}