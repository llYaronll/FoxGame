using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRole : MonoBehaviour
{
    public Button lobbyRole;
    public Sprite spr_LobbyRole,spr_ChangeRole;
    

    public void Start()
    {
        spr_LobbyRole = lobbyRole.GetComponent<Image>().sprite;
    }

    public void OnClick(Sprite image)
    {
        spr_ChangeRole = image;
        lobbyRole.GetComponent<Image>().sprite=image;
    }

    public void LobbyRoleClick()
    {
        
    }
}
