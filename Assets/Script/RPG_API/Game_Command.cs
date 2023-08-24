using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SoraHareSakura_GameApi
{
    [System.Serializable]
    public class Game_Command
    { 
        public string command;
        public List<string> args;

        public Game_Command()
        {
            command = "";
            args = new List<string>();
        }
        public Game_Command(string command, List<string> args)
        {
            this.command = command;
            this.args = args;
        }

        public Game_Command(string command)
        {
            (string blockCommand,List<string> argsBlock) = CommandParser(command,' ');
            this.command = blockCommand;
            args = argsBlock;
            (this.command,args) = CommandParser(command, ' ');
        }

        public (string,List<string>) CommandParser(string command,char split)//(string,List<string>) is can C# 7.0 Up
        {
            string[] commandString = command.Split(split);
            List<string> block = new List<string>();
            for(int i = 1; i < commandString.Length; i++)
            {
                block.Add(commandString[i]);
            }
            return (commandString[0],block);
        }

        public void Copy(Game_Command a)
        {
            /*command = a.command;
            int X = 0;
            args.ForEach(arg =>
            {
                arg = a.args[X].ToString();
                X++;
            });*/
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
    }
}
