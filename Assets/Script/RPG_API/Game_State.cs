using JetBrains.Annotations;
using SoraHareSakura_Fight_System;
using SoraHareSakura_Game_Api;
using SoraHareSakura_GameData_Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_GameApi
{
    [System.Serializable]
    public enum StateType
    {
        Permanent,
        Aging
    }

    [System.Serializable]
    public enum LimitAction
    {
        none,
        onlySelectFriend,
        onlySelectEnemy,
        attackAnyRole,
        unableToAct
    }

    [System.Serializable]
    public class Game_State
    {
        public int id;
        public string name;
        public StateType type;
        public LimitAction limitType;
        public float duration;
        public float timeLeft;
        public List<Game_Effect> game_Commands;
        public string imagePath;

        public Game_State()
        {
            init(0, "", 0, 0, new List<Game_Effect>(), "");
        }

        public Game_State(int id, string name, float duration, float timeLeft, List<Game_Effect> game_Commands, string imagePath)
        {
            init(id, name, duration, timeLeft, game_Commands, imagePath);
        }

        public void init(int id, string name, float duration, float timeLeft, List<Game_Effect> game_Commands, string imagePath)
        {
            this.id = id;
            this.name = name;
            this.duration = duration;
            this.timeLeft = timeLeft;
            this.game_Commands = game_Commands;
            this.imagePath = imagePath;
        }

        public void Copy(Game_State a)
        {
            JsonToThis(a.ToJson());
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void JsonToThis(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }

        

        //計算經過時間 並回傳狀態是否持續
        public bool TimeFlow(float elapsedTime)
        {
            if (type == StateType.Permanent) return true;
            timeLeft = timeLeft - elapsedTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
                return false;
            }
            return true;
        }
        
        //初始化剩餘時間
        public void InitTimeLeft()
        {
            timeLeft = duration;
        }

        public void InitTimeLeft(float inputTime)
        {
            timeLeft = inputTime;
        }

        //增加狀態持續時間
        public void AddDuration(float addTime)
        {
            timeLeft = timeLeft + addTime;
        }

        //增加狀態持續時間 且不超過最大持續時間
        public void AddDurationNoOver(float addTime)
        {
            AddDuration(addTime);
            if (timeLeft > duration)
            {
                timeLeft = duration;
            }
            if(timeLeft < 0)
            {
                timeLeft = 0;
            }
        }

        public void Run(Game_Actor a)
        {
            for(int i = 0; i < game_Commands.Count; i++)
            {
                game_Commands[i].Run(a);
            }
        }
    }
}

