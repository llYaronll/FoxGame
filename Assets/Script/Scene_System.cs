using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoraHareSakura_Scene_System
{
    public class Scene_System : MonoBehaviour
    {
        public List<GameObject> needSaveGameObjects;
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        public void Init()
        {
            if(needSaveGameObjects == null)
            {
                needSaveGameObjects = new List<GameObject>();
            }
           // needSaveGameObjects.Add(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadGameChapter1()
        {
            StartCoroutine(LoadScene("GameChapter1"));
            print("ISOK");
        }

        public void LoadSceneByString(string sceneName)
        {
            StartCoroutine(LoadScene(sceneName));
            print("LoadScene is OK!");
        }

        public IEnumerator LoadScene(string sceneName)
        {
            print("is load ...");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                print("Load" + asyncLoad.progress);
                yield return null;
            }
            print("Load is Ok");
            yield return null;

            Scene unloadScene = SceneManager.GetActiveScene();
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
            foreach(GameObject mainGameObject in needSaveGameObjects)
            {
                SceneManager.MoveGameObjectToScene(mainGameObject, scene);
            }

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(unloadScene.name);
            while (!asyncUnload.isDone)
            {
                print("Unload" + asyncUnload.progress);
                yield return null;
            }
            print("Unload is Ok");
            yield return null;
        }
    }

}