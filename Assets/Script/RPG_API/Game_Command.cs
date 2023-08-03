using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_GameApi
{
    public class Game_Command
    {
        public string command;
        public List<string> args;

        public Game_Command(string command, List<string> args)
        {
            this.command = command;
            this.args = args;
        }

        public Game_Command(string command)
        {
            string[] commandString = command.Split(' ');
            this.command = commandString[0];
            this.args = new List<string>();
            for(int i = 1; i < commandString.Length; i++)
            {
                this.args.Add(commandString[i]);
            }
        }
    }
}
