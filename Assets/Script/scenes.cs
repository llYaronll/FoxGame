using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
   public void change(string str_Scenes)
   {
      SceneManager.LoadScene(str_Scenes);
   }

}
