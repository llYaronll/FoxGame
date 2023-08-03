using SoraHareSakura_Fight_System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoraHareSakura_GameApi
{
    public class Command_System : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CommandRun(string code,List<string> args)
        {
            switch(code){
                case "Damage":
                    break;
                default:
                    break;
            }
        }
    }

}
