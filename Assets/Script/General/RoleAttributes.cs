using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class RoleAttributes
    {
        public string name;//�W��
        public string description;//�y�z

        public Attributes hp; //��q
        public Attributes mp;//�]�O�q
        public Attributes speed;//�t��

        public Level level;

        public Attributes criticalHit;//�z�����v
        public Attributes criticalHitProbability;//�z�����v

        public Attributes physicalAttack;//���z���� �L�ݩ�
        public Attributes physicalDefense;//���z���m �L�ݩ�

        public Attributes magicAttack;//�]�k���� �D�L�ݩ�
        public Attributes magicDefense;//�]�k���m �D�L�ݩ�

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

