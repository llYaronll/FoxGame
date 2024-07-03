using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace SoraHareSakura_General
{
    public class DataList<T>
    {
        public string name;
        public List<T> datas;
        public DataList(string name) { 
            this.name = name;
            datas = new List<T>();
        }

        public DataList(string name,List<T> dataList)
        {
            this.name = name;
            datas = dataList;
        }

        public void AddData(T data) {
            datas.Add(data);
        }

        public T GetData(int index)
        {
            return datas[index];
        }

        public void RemoveData(int index)
        {
            datas.RemoveAt(index);
        }

        public void Clear() { 
            datas.Clear();
        }
    }
}
