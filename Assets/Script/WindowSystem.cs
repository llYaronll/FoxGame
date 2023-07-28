using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSystem : MonoBehaviour
{
  public GameObject widowCam;
  public Vector3 horizontal_Window_Position, direct_View_Window_Position;//horizntal橫向；direct直向
  public void FixedUpdate()
  {
    float width = Screen.width;
    float height = Screen.height;
    float aspect_ratio = width / height;//大於1為橫向；小於1為直向
    Debug.Log(Screen.width+","+Screen.height);
    
    if (aspect_ratio > 1)
    {
      widowCam.transform.position = horizontal_Window_Position;
      widowCam.GetComponent<Camera>().orthographicSize = height / 2;
      Debug.Log("橫向");
    }
    else
    {
      widowCam.transform.position = direct_View_Window_Position;
      widowCam.GetComponent<Camera>().orthographicSize = height / 2;
      Debug.Log("直向");
    }
    
    
  }
}
