using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadOrQuitGame : MonoBehaviour {

    public void Reload()
    {
        Shoot shoot = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        shoot.crosshairLockRestart = true;
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("controller").GetComponent<AudioSource>().enabled = true;
        SceneManager.LoadScene("Main_Scene_1");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
