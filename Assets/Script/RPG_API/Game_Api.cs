using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SoraHareSakura_Game_Api
{
    //General props ���q�D�� special prosps �S��D��
    [System.Serializable]
    public enum ItemType
    {
        generalProps,
        specialProps
    }

    //��֨ϥ�
    [System.Serializable]
    public enum ScopeOfUse
    {
        none,//�L��H
        user,//�ϥΪ�
        partner,//����
        enemy//�ĤH
    }

    //�ϥγ��X
    [System.Serializable]
    public enum Situation
    {
        none,//�L���X
        anyTime,//����ɭ�
        menuTime,//��椶����
        fighting//�԰���
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
