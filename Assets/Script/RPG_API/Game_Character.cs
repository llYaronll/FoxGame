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
        public int level;//����
        public int upLevelExperiencePoint;//�ɯũһݸg���
        public int experiencePoint;//�g���
        public int upPoint;//�ɯ��I
        public int initUpLevelPoint;//initialization
        public int growthValue;//�g�禨����
        public int growthUpPoint;//�ɯ��I������

        //�g��Ȧ������
        public int EXGrowthFunction()
        {
            return growthValue * level + upLevelExperiencePoint;
        }

        //�ɯ��I�������
        public int UpPointGrowthFunction()
        {
            return growthUpPoint;
        }

        //�]�w�g���
        public void SetExperience(int exValue)
        {
            experiencePoint = exValue;
            LevelUp();
        }

        //�W�[�g���
        public void AddExperiencePoint(int exValue)
        {
            SetExperience(exValue + experiencePoint);
        }

        //���Ŵ���
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

        //�W�[�@�ӵ���
        public void AddLevel()
        {
            level = level + 1;
            upLevelExperiencePoint = EXGrowthFunction();
            upPoint = upPoint + UpPointGrowthFunction();
        }

        //�W�[���� ��J�W�[���ŭ�
        public void AddLevel(int levelValue)
        {
            for (int i = 0; i < levelValue; i++)
            {
                AddLevel();
            }
        }

        //��l�Ƥɯ��I
        public void initUpPoint()
        {
            upPoint = level * UpPointGrowthFunction() + initUpLevelPoint;
        }

    }
}