using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageShow : MonoBehaviour
{
  public GameObject btn_Lobby;
  public bool lobbyOpen;//true開著；false關閉

  public void Start()
  {
    lobbyOpen = true;
    btn_Lobby.SetActive(lobbyOpen);
  }

  public void OnClick()
  {
    //判斷開關:開著時候關閉；關閉時候開啟
    if (lobbyOpen)
    {
      lobbyOpen = false;
    }else
    {
      lobbyOpen = true;
    }
    btn_Lobby.SetActive(lobbyOpen);
    
  }

}
