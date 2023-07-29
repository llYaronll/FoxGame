using System.Collections;
using System.Collections.Generic;
using SoraHareSakura_Game_Api;
using UnityEngine;

namespace SoraHareSakura_GameData_Api
{
    [System.Serializable]
    //�C����Ʈw �D��w�s
    public class GameData_Items
    {
        public int id;
        public List<Game_Item> items;
    }

    [System.Serializable]
    //���a�w�s �D��w�s
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

}
