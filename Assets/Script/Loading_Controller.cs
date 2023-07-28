using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
//using mLib;

public class Loading_Controller : MonoBehaviour
{
    
   
    // 目标异步场景
    private AsyncOperation asyncOperation;

    void Start()
    {
    
        // 协程启动异步加载
      //  StartCoroutine(this.AsyncLoading());
    }

   // IEnumerator AsyncLoading()
    //{
    
       // this.asyncOperation= SceneManager.LoadSceneAsync((string)SceneController.sceneNames[SceneController.sceneIndex]);
        //终止自动切换场景
       // this.operation.allowSceneActivation = false;
        //yield return operation;
   // }
    void Update()
    {
    

        // 当进度大于0.9时就已经加载完毕
        if (this.asyncOperation.progress >= 0.9f)
        {
    
            // 允许切换。将在下一帧切换场景
         //   this.operation.allowSceneActivation = true;
        }
    }
   
}