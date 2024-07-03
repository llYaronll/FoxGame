using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace SoraHareSakura_General
{
    [System.Serializable]
    public class Attributes
    {
        public float baseValue;//基礎值
        public float upValue;//每級升級
        public float LV;//等級
        public float nowValue;//現在值

        public Attributes()
        {
            baseValue = 0;
            upValue = 0;
            LV = 0;
            nowValue = 0;
        }

        public Attributes(float baseValue, float upValue,float LV,float nowValue) 
        {
            this.baseValue = baseValue;
            this.upValue = upValue;
            this.LV = LV;
            this.nowValue = nowValue;
        }

        public Attributes(string jsonString)
        {
            Attributes newAttributes = JsonUtility.FromJson<Attributes>(jsonString);
            baseValue = newAttributes.baseValue;
            upValue = newAttributes.upValue;
            LV = newAttributes.LV;
            nowValue = newAttributes.nowValue;
        }

        public Attributes(Attributes newAttributes)
        {
            baseValue = newAttributes.baseValue;
            upValue = newAttributes.upValue;
            LV = newAttributes.LV;
            nowValue = newAttributes.nowValue;
        }

        public void Add(float value)
        {
            nowValue += value;
        }

        public void Up(float value)
        {
            LV += value;
            nowValue = MaxValue();
        }

        public float MaxValue()
        {
            return baseValue + LV * upValue;
        }

        public float Value()
        {
            return nowValue;
        }

        override public string ToString()
        {
            return nowValue.ToString();
        }

        static public float operator +(Attributes a,Attributes b)
        {
            return a.nowValue + b.nowValue;
        }

        static public float operator -(Attributes a, Attributes b)
        {
            return a.nowValue - b.nowValue;
        }

        static public float operator *(Attributes a, Attributes b)
        {
            return a.nowValue * b.nowValue;
        }


        static public float operator /(Attributes a, Attributes b)
        {
            return a.nowValue / b.nowValue;
        }

        public string ToJson(){
            return JsonUtility.ToJson(this);
        }
    }
}
