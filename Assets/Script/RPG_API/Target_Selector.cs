using SoraHareSakura_GameApi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace SoraHareSakura_Fight_System
{
    public class Target_Selector : MonoBehaviour
    {
        public GameObject mark;//設定目標標示圖案
        //public Canvas markLayers;
        public List<GameObject> marks;//標示目標優先值的圖案
        public List<GameObject> team;//場上戰鬥的團隊物件
        public int order;//目前選擇第幾個順位
        public int ti;
        public List<bool> SurviveTable;


        // Start is called before the first frame update
        void Start()
        {
            marks = new List<GameObject>();
            InitMark();
            order = 0;
        }

        // Update is called once per frame
        void Update()
        {
            /*if(team.Count != oldCount)
            {
                InitMark();
                oldCount = team.Count;
            }*/
            for(int i = 0; i < team.Count; i++)
            {
                if (!team[i].GetComponent<Game_Batter>().IsSurvive() && SurviveTable[i])
                {
                    team[i].GetComponent<Button>().onClick.RemoveAllListeners();
                    Touch(team[i].GetComponent<RectTransform>());
                    SurviveTable[i] = false;
                }
            }
        }

        public void InitMark()
        {
            for(int i = 0; i < marks.Count; i++)
            {
                GameObject mark = marks[i];
                Destroy(mark);
            }
            marks.Clear();
            SurviveTable = new List<bool>();
            foreach (GameObject button in team)
            {
                button.GetComponent<Button>().onClick.RemoveAllListeners();
                NewMark(button);
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Touch(button.GetComponent<RectTransform>());
                });
                SurviveTable.Add(true);
            }
        }

        //新增mark 到 team 位置
        public void NewMark(GameObject batter)
        {
            GameObject newMark = Instantiate(mark);
            newMark.GetComponent<RectTransform>().parent = batter.GetComponent<RectTransform>().parent;
            newMark.GetComponent<RectTransform>().anchoredPosition = batter.GetComponent<RectTransform>().anchoredPosition;
            newMark.GetComponent<RectTransform>().localPosition = batter.GetComponent<RectTransform>().localPosition;
            newMark.GetComponent<RectTransform>().localScale = batter.GetComponent<RectTransform>().localScale;
            newMark.GetComponent<RectTransform>().localRotation = batter.GetComponent<RectTransform>().localRotation;
            newMark.GetComponentInChildren<Text>().text = "";
            newMark.GetComponent<Button>().onClick.AddListener(() =>
            {
                Touch(batter.GetComponent<RectTransform>());
            });
            newMark.SetActive(false);
            marks.Add(newMark);
        }
        

        //增加batter 到 team
        public void AddBatter(GameObject batter)
        {
            team.Add(batter);
        }

        public void Touch(RectTransform xy)
        {
            int touchId = marks.FindIndex(thisMark =>
                thisMark.GetComponent<RectTransform>().anchoredPosition.x == xy.anchoredPosition.x &&
                thisMark.GetComponent<RectTransform>().anchoredPosition.y == xy.anchoredPosition.y
            );
            if (touchId < order)
            {
                order--;
                if(order < 0)order = 0;
                InsertSwap(touchId, marks.Count-1);
            }
            else
            {
                InsertSwap(touchId);
            }
            WriteTheNumber();
        }

        public void InsertSwap(int id)
        {
            order %= marks.Count;
            if(id < 0)return;
            GameObject k = marks[id];
            marks.RemoveAt(id);
            marks.Insert(order, k);
            order++;
        }

        public void InsertSwap(int id,int insert)
        {
            if (id < 0) return;
            GameObject k = marks[id];
            marks.RemoveAt(id);
            marks.Insert(insert, k);
        }

        public void WriteTheNumber()
        {
            int k = 1;
            marks.ForEach(markObj =>
            {
                if (markObj.GetComponentInChildren<Text>() != null && k < order+1)
                {
                    markObj.GetComponentInChildren<Text>().text = k.ToString();
                    markObj.SetActive(true);
                }
                else
                {
                    markObj.GetComponentInChildren<Text>().text = "";
                    markObj.SetActive(false);
                }
                k++;
            });
        }

        //回傳優先值表
        public List<int> ToOrder()
        {
            List<int> list = new List<int>();
            foreach(GameObject teamObj in team)
            {
                GameObject teamOrderNumber = marks.Find(thisMark =>
                    thisMark.GetComponent<RectTransform>().anchoredPosition.x == teamObj.GetComponent<RectTransform>().anchoredPosition.x &&
                    thisMark.GetComponent<RectTransform>().anchoredPosition.y == teamObj.GetComponent<RectTransform>().anchoredPosition.y
                );
                string orderValue = teamOrderNumber.GetComponentInChildren<Text>().text;
                int showOrder;
                bool isOk = int.TryParse(orderValue, out showOrder);

                if (isOk)
                {
                    list.Add(marks.Count - showOrder);
                }
                else
                {
                    list.Add(-1);
                }
            }
            string showList = "{";
            for(int i = 0; i < list.Count; i++)
            {
                showList += list[i].ToString() + ",";
            }
            showList += "}";
            showList += order;
            Debug.Log(showList);
            return list;
        }

        public void Ti()
        {
            ToOrder();
        }
    }
}
