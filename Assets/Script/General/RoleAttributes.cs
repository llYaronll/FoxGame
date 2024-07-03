using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class RoleAttributes
    {
        public string name;//名稱
        public string description;//描述

        public Attributes hp; //血量
        public Attributes mp;//魔力量
        public Attributes speed;//速度

        public Level level;

        public Attributes criticalHit;//爆擊倍率
        public Attributes criticalHitProbability;//爆擊機率

        public Attributes physicalAttack;//物理攻擊 無屬性
        public Attributes physicalDefense;//物理防禦 無屬性

        public Attributes magicAttack;//魔法攻擊 非無屬性
        public Attributes magicDefense;//魔法防禦 非無屬性

        public RoleAttributes()
        {
            hp = new Attributes(100.0f, 50.0f, 0.0f, 100.0f);
            mp = new Attributes(100.0f, 50.0f, 0.0f, 100.0f);
            speed = new Attributes(10.0f, 1.0f, 0.0f, 10.0f);

            level = new Level(1, 0, 100, 0);

            criticalHit = new Attributes(10.0f, 1.0f, 0.0f, 10.0f);
            criticalHitProbability = new Attributes(10.0f, 1.0f, 0.0f, 10.0f);

            physicalAttack = new Attributes(10.0f, 1.0f, 0.0f, 10.0f);
            physicalDefense = new Attributes(5.0f, 1.0f, 0.0f, 5.0f);
            magicAttack = new Attributes(10.0f, 1.0f, 0.0f, 10.0f);
            magicDefense = new Attributes(5.0f, 1.0f, 0.0f, 5.0f);
        }

        
        public RoleAttributes(string name,string description, Attributes hp, Attributes mp, Attributes speed, Attributes criticalHit, Attributes criticalHitProbability, Attributes physicalAttack, Attributes physicalDefense, Attributes magicAttack, Attributes magicDefense)
        {
            this.name = name;
            this.description = description;

            this.hp = hp;
            this.mp = mp;
            this.speed = speed;

            this.criticalHit = criticalHit;
            this.criticalHitProbability = criticalHitProbability;

            this.physicalAttack = physicalAttack;
            this.physicalDefense = physicalDefense;

            this.magicAttack = magicAttack;
            this.magicDefense = magicDefense;
        }

        public RoleAttributes(RoleAttributes roleData)
        {
            name = roleData.name;
            description = roleData.description;

            hp = roleData.hp;
            mp = roleData.mp;
            speed = roleData.speed;

            criticalHit = roleData.criticalHit;
            criticalHitProbability = roleData.criticalHitProbability;

            physicalAttack = roleData.physicalAttack;
            physicalDefense = roleData.physicalDefense;

            magicAttack = roleData.magicAttack;
            magicDefense = roleData.magicDefense;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}

