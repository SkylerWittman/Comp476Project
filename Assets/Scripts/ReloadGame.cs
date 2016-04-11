using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadGame : MonoBehaviour {

    public void Reload()
    {
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("controller").GetComponent<AudioSource>().enabled = true;
        SceneManager.LoadScene("Main_Scene_1");
    }
}
