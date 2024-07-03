using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoraHareSakura_General
{
    public class Level 
    {
        public int level;//���� 0~n || 1~n
        public int upExp;//���ɤU�@�Ū��g���
        public int baseExp;//��¦�ɯŸg���
        public int nowExp;//�{�b�g���

        public Level()
        {
            level = 1;
            upExp = 100;
            baseExp = 0;
            nowExp = 0;
        }

        public Level(int level, int upExp, int baseExp, int nowExp)
        {
            this.level = level;
            this.upExp = upExp;
            this.baseExp = baseExp;
            this.nowExp = nowExp;
        }

        public Level(Level newlevel)
        {
            level = newlevel.level;
            upExp = newlevel.upExp;
            baseExp = newlevel.baseExp;
            nowExp = newlevel.nowExp;
        }

        public void AddExp(int exp)
        {
            nowExp += exp;
        }

        public void AutoUpLevel()
        {
            while(nowExp > NextLevelExperience())
            {
                nowExp -= NextLevelExperience();
                level++;
            }
        }

        public int NextLevelExperience()
        {
            return baseExp + (level-1) * upExp;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}