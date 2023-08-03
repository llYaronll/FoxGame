using JetBrains.Annotations;
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
        public List<Game_Command> game_Commands;
        public string imagePath;

        public Game_State(int id, string name, float duration, float timeLeft, List<Game_Command> game_Commands, string imagePath)
        {
            init(id,name,duration,timeLeft,game_Commands,imagePath);
        }

        public void init(int id, string name, float duration, float timeLeft, List<Game_Command> game_Commands, string imagePath)
        {
            this.id = id;
            this.name = name;
            this.duration = duration;
            this.timeLeft = timeLeft;
            this.game_Commands = game_Commands;
            this.imagePath = imagePath;
        }

        public void TimeFlow(float elapsedTime)
        {
            timeLeft = timeLeft - elapsedTime;
            if(timeLeft < 0)
            {
                timeLeft = 0;
            }
        }

        public void AddDuration(float addTime)
        {
            timeLeft = timeLeft + addTime;
        }

        public void AddDurationNoOver(float addTime)
        {
            AddDuration(addTime);
            if(timeLeft > duration)
            {
                timeLeft = duration;
            }
        }
    }
}

