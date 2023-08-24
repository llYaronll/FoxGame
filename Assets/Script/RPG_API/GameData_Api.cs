using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameApi;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace SoraHareSakura_GameData_Api
{
    [System.Serializable]
    //遊戲資料庫 道具庫存
    public class GameData_Items
    {
        public int id;
        public List<Game_Item> items;
    }

    [System.Serializable]
    //玩家庫存 道具庫存
    public class GameData_Reserve
    {
        public int id;
        public List<Game_Item> items;

        public GameData_Reserve()
        {
            id = 0;
            items = new List<Game_Item>();
        }

        public GameData_Reserve(int reserveId)
        {
            id = reserveId;
        }
        public GameData_Reserve(int id, List<Game_Item> items)
        {
            this.id = id;
            this.items = items;
        }
    }

    [System.Serializable]
    public class GameData_StateTable
    {
        public int id;
        public List<Game_State> game_States;

        public Game_State FindAndAddState(string stateName)
        {
            Game_State NoFight = new Game_State();
            Game_State reg = game_States.Find(state => state.name.Equals(stateName));
            if(reg == null)
            {
                return null;
            }
            NoFight.Copy(reg);
            return NoFight;
        }
    }

    [System.Serializable]
    public class GameData_EquipmentTypeTable
    {
        public List<string> equipmentType;
    }

    [System.Serializable]
    public class GameData_AttackType
    {
        public List<string> attackType;
    }

    [System.Serializable]
    public class GameData_SkillTable
    {
        public List<Game_Skill> skillTable;
    }
}
