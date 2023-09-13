using SoraHareSakura_Fight_System;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoraHareSakura_GameApi
{
    [System.Serializable]
    public class Game_Character : Character_Attribute
    {
        public Game_Character()
        {
            init(0,"player1",new Level(),100,10,10,100,10);
        }

        public Game_Character(Character_Attribute A)
        {
            Copy(A);
        }

        public void init(int id, string name, Level level, int stamina, int magicPoint, int power, int speed, int spiritualPower)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.stamina = new AttributeValue(stamina);
            this.magicPoint = new AttributeValue(magicPoint);
            this.power = new AttributeValue(power);
            this.speed = new AttributeValue(speed);
            this.spiritualPower = new AttributeValue(spiritualPower);
            equipmentSlots = new Game_EquipmentColumn();
        }

        public bool PutOnEquipment(Game_Equipment a)
        {
            bool putOn = false;
            equipmentSlots.equipmentSlots.ForEach(equipmentSlot =>
            {
                if(a.equipmentType.Equals(equipmentSlot.equipmentType))
                {
                    equipmentSlot.equipment = a;
                    putOn = true;
                    return;
                }
            });
            if(putOn)UpDataFigherAttributeOfEquipment();
            return putOn;
        }

        public void UpDataFigherAttributeOfEquipment()
        {
            //int
            stamina.UpDataEquipment(equipmentSlots.SumAttributeValueInt("stamina"));
            magicPoint.UpDataEquipment(equipmentSlots.SumAttributeValueInt("magicPoint"));
            power.UpDataEquipment(equipmentSlots.SumAttributeValueInt("power"));
            spiritualPower.UpDataEquipment(equipmentSlots.SumAttributeValueInt("spiritualPower"));
            speed.UpDataEquipment(equipmentSlots.SumAttributeValueInt("speed"));

            attack.UpDataEquipment(equipmentSlots.SumAttributeValueInt("attack"));
            magicAttack.UpDataEquipment(equipmentSlots.SumAttributeValueInt("magicAttack"));
            magicDefense.UpDataEquipment(equipmentSlots.SumAttributeValueInt("magicDefense"));
            defense.UpDataEquipment(equipmentSlots.SumAttributeValueInt("defense"));

            //float
            criticalHitHarm.UpDataEquipment(equipmentSlots.SumAttributeValueFloat("criticalHitHarm"));
            criticalHitRate.UpDataEquipment(equipmentSlots.SumAttributeValueFloat("criticalHitRate"));

            //int
            hitRate.UpDataEquipment(equipmentSlots.SumAttributeValueInt("hitRate"));
            luck.UpDataEquipment(equipmentSlots.SumAttributeValueInt("luck"));

            actionValue.UpDataEquipmentValue(equipmentSlots.SumAttributeValueFloat("actionValue"));
        }

        //stamina ��O
        public void UpStamina(int value)//������O
        {
            stamina.AddUpPoint(value);
        }

        //�����O��
        public (int now,int max) Stamina()
        {
            return (stamina.nowValue, stamina.maxValue);
        }

        public string ShowStaminaString()
        {
            string showText = Stamina().now + "/" + Stamina().max;
            return showText;
        }

        //magic point �]�O
        public void UpMagicPoint(int value)//�����]�O�I
        {
            magicPoint.AddUpPoint(value);
        }

        //����]�O
        public (int now,int max) MagicPoint()
        {
            return (magicPoint.nowValue, magicPoint.maxValue);
        }

        //power
        public void UpPower(int value)//���ɤO�q�I
        {
            power.AddUpPoint(value);
        }

        public int Power()//��ܤO�q�I
        {
            return power.maxValue;
        }

        //speed
        public void UpSpeed(int value)//���ɳt���I
        {
            speed.AddUpPoint(value);
        }

        public int Speed()//��ܳt���I
        {
            return speed.maxValue;
        }

        //spiritua power
        public void UpSpiritualPower(int value)//���ɪk�O�I
        {
            spiritualPower.AddUpPoint(value); 
        }

        public int SpiritualPower()//��ܪk�O�I
        {
            return spiritualPower.maxValue;
        }

        //��ܼƭ�
        //attack
        public int AttackValue()
        {
            return attack.maxValue;
        }

        //defense
        public int DefenseValue()
        {
            return defense.maxValue;
        }

        //magic attack
        public int MagicAttack()
        {
            return magicAttack.maxValue;
        }

        //magic defense
        public int MagicDefense()
        {
            return magicDefense.maxValue;
        }

        //critical hit harm
        public float CriticalHitHarm()
        {
            return criticalHitHarm.maxValue;
        }

        //critical hit rate
        public float CriticalHitRate()
        {
            return criticalHitRate.maxValue;
        }

        //hit rate
        public int HitRate()
        {
            return hitRate.maxValue;
        }

        //luck
        public int Luck()
        {
            return luck.maxValue;
        }

        //action value
        public (float now,float max) ActionValue()
        {
            return (actionValue.actionValue, actionValue.maxActionValue);
        }

        //skill
        public void AddSkill(Game_Skill enterSkill)
        {
            Game_Skill skill = new Game_Skill();
            skill.JsonToThis(enterSkill.ToJson());
            skills.Add(skill);
        }

    }

}