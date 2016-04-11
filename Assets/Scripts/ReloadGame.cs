using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadGame : MonoBehaviour {

    public void Reload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Scene_1");
    }
}
