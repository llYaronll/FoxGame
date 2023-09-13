using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameApi;
using TMPro.EditorUtilities;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

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

    //角色表單
    [System.Serializable]
    public class GameData_GameRoleTable
    {
        public int id;
        public string name;
        public List<Character_Attribute> characterTable;

        //新增角色
        public void CreateOne()
        {
            if(characterTable == null)
            {
                characterTable = new List<Character_Attribute>();
            }
            Character_Attribute a = new Character_Attribute();
            a.id = characterTable.Count;
            characterTable.Add(a);
        }

        public void Add(Character_Attribute A)
        {
            Character_Attribute b = new Character_Attribute();
            b.Copy(A);
            b.id = characterTable.Count;
            characterTable.Add(b);
        }

        public void Add(string jsonA)
        {
            Character_Attribute b = new Character_Attribute();
            b.JsonToThis(jsonA);
            b.id = characterTable.Count;
            characterTable.Add(b);
        }

        //修改角色
        public void Save(int id,string roleJson)
        {
            if (IsOver(id)) return;
            characterTable.Find(obj => obj.id == id).Copy(roleJson);
        }
        
        //刪除角色
        public void Delete(List<int> ids)
        {
            for(int i = 0; i < ids.Count; i++)
            {
                Delete(ids[i]);
            }
        }

        public void Delete(int id)
        {
            if (IsOver(id)) return;

            characterTable[id] = null;
            
            Compression();
        }

        public void Compression()
        {
            characterTable.RemoveAll(X => X == null);
        }
        
        //查詢角色
        public string FindStringDataById(int id)
        {
            return characterTable.Find(obj => obj.id == id).ToJson();
        }

        //判斷邊界
        public bool IsOver(int id)
        {
            if (characterTable == null) return true;
            if (characterTable.Count == 0) return true;
            if(id >= characterTable.Count)return true;
            if(id < 0)return true;
            return false;
        }


        public int Length()
        {
            return characterTable.Count; 
        }
    }

    //隊伍表單
    [System.Serializable]
    public class GameData_TeamTable
    {
        public int id;
        public int teamSize;
        public List<GameData_TeamGrid> characters;

        public Vector2Int mapSize;
        public List<List<int>> teamGrids;

        public GameData_TeamTable(int id, int teamSize, List<GameData_TeamGrid> characters)
        {
            this.id = id;
            this.teamSize = teamSize;
            this.characters = characters;
        }

        public GameData_TeamTable()
        {
            id=0;
            teamSize = 4;
            init();
        }

        public void init()
        {
            characters = new List<GameData_TeamGrid>();
            for(int i = 0;i < teamSize; i++)
            {
                characters.Add(new GameData_TeamGrid());
            }
        }

        public void init2D()
        {
            teamGrids = new List<List<int>>();
            for(int i = 0;i < mapSize.x; i++)
            {
                teamGrids.Add(new List<int>());
                for(int j = 0;j < mapSize.y; j++)
                {
                    teamGrids[i].Add(-1);
                }
            }
        }

        public void Add(int id,int position)
        {
            if(IsOver(position))return;
            characters[position].id = id;
            characters[position].queuePosition = 0;
        }

        public void Add(int id, int position,int queuePosition)
        {
            if (IsOver(position)) return;
            characters[position].id = id;
            characters[position].queuePosition = queuePosition;
        }

        public void Add2D(int id, int position, int queuePosition,Vector2Int teamGridPosition)
        {
            if (IsOverMap(teamGridPosition.x, teamGridPosition.y)) return;
            Add(id, position, queuePosition);
            teamGrids[teamGridPosition.x][teamGridPosition.y] = id;
        }

        public void Delect2D(int position)
        {
            if(IsOver(position))return;
            int id = characters[position].id;
            for(int i = 0; i < teamGrids.Count; i++)
            {
                for(int j = 0;j < teamGrids[i].Count; j++)
                {
                    if (teamGrids[i][j] == id)
                    {
                        teamGrids[i][j] = -1;
                        return;
                    }
                }
            }
        }

        public bool IsOver(int id)
        {
            if (characters == null) return true;
            if(characters.Count == 0) return true;
            if(id >= characters.Count)return true;
            if(id < 0) return true;
            return false;
        }

        public bool IsOverMap(int x,int y)
        {
            if (teamGrids == null) return true;
            if (teamGrids.Count == 0) return true;
            if (mapSize.x <= 0 || mapSize.y <= 0) return true;
            if (x >= mapSize.x || y >= mapSize.y) return true;
            if (x < 0 || y < 0) return true;
            return false;
        }
    }

    [System.Serializable]
    public class GameData_TeamGrid
    {
        public int team;
        public int id;
        public int queuePosition;

        public GameData_TeamGrid()
        {
            team = 0;
            id = -1;
            queuePosition = -1;
        }
    }
}
