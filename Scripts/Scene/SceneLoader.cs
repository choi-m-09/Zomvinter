using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader _inst = null;
    public static SceneLoader Inst
    {
        get
        {
            if(_inst == null)
            {
                _inst = FindObjectOfType<SceneLoader>();
                if(_inst == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SceneLoader";
                    DontDestroyOnLoad(obj);
                    _inst = obj.AddComponent<SceneLoader>();
                }
            }
            return _inst;
        }
    }

    public void LoadScene(int Index)
    {
        StartCoroutine(SceneLoading(Index));
    }

   
    public void LoadScene(string SceneName)
    {
        StartCoroutine(SceneLoading(SceneName));
    }
    IEnumerator SceneLoading(int Index)
    {
        yield return SceneManager.LoadSceneAsync("Loading"); ;
        yield return StartCoroutine(Loading(Index));
    }
    IEnumerator SceneLoading(string SceneName)
    {
        yield return SceneManager.LoadSceneAsync("Loading"); ;
        yield return StartCoroutine(Loading(SceneName));
    }

    IEnumerator Loading(int Index)
    {
        Slider loadingBar = GameObject.Find("LoadingProgress")?.GetComponent<Slider>();
        AsyncOperation ao = SceneManager.LoadSceneAsync(Index);
        //씬 로딩이 끝나기 전까진 씬을 활성화 하지 않는다.
        ao.allowSceneActivation = false;
        while(!ao.isDone)
        {
            float v = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log(v);
            if (loadingBar != null) loadingBar.value = v;

            if(Mathf.Approximately(v, 1.0f))
            {
                yield return new WaitForSeconds(1.0f);
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

    }
    IEnumerator Loading(string SceneName)
    {
        Slider loadingBar = GameObject.Find("LoadingProgress")?.GetComponent<Slider>();
        AsyncOperation ao = SceneManager.LoadSceneAsync(SceneName);
        //씬 로딩이 끝나기 전까진 씬을 활성화 하지 않는다.
        ao.allowSceneActivation = false;
        while (!ao.isDone)
        {
            float v = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log(v);
            if (loadingBar != null) loadingBar.value = v;

            if (Mathf.Approximately(v, 1.0f))
            {
                yield return new WaitForSeconds(1.0f);
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

    }
}
