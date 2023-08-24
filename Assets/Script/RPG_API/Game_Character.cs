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
            return putOn;
        }

        public void UpDataFigherAttributeOfEquipment()
        {

        }
    }

}