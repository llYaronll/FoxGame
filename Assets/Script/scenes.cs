using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenes : MonoBehaviour
{
   public void change()
   {
      SceneManager.LoadScene(1);
   }
}
