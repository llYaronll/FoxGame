using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public List<GameData_Effect> effect;
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
            effect = new List<GameData_Effect>();
            scopeOfUse = ScopeOfUse.none;
            when = Situation.none;
            imagePath = "";
        }
    }

    public class GameData_Effect
    {

    }
}
