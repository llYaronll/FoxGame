using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Base_System : MonoBehaviour
{
    public string findObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        GameObject.Find(findObj).GetComponent<Text>().text="傳輸";
    }
}
