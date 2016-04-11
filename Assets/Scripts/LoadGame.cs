using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        print("Loading...");
        StartCoroutine(LoadLevelWithProgress("Main_Scene_1"));
        print("Loading complete");
        GameObject loadScreen = GameObject.FindGameObjectWithTag("LoadScreen");
        StartCoroutine(extraWaitTime());
        Destroy(loadScreen);
    }

    private IEnumerator LoadLevelWithProgress(string levelToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        while (!operation.isDone)
        {
            yield return operation.isDone;
            Debug.Log("loading progress: " + operation.progress);
        }
    }

    private IEnumerator extraWaitTime()
    {
        yield return new WaitForSeconds(4.0f);
    }

}