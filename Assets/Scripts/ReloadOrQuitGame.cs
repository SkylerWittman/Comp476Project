using UnityEngine;
using UnityEngine.SceneManagement;

//This script allows the player to restart or quit the game
//It's attached to all restart and quit buttons
public class ReloadOrQuitGame : MonoBehaviour {

    public void Reload()
    {
        Shoot shoot = GameObject.FindGameObjectWithTag("Player").GetComponent<Shoot>();
        //re-enable crosshair
        shoot.crosshairLockRestart = true;
        //re-enable time
        Time.timeScale = 1;
        //re-enable audio
        GameObject.FindGameObjectWithTag("controller").GetComponent<AudioSource>().enabled = true;
        //load the scene again
        SceneManager.LoadScene("Main_Scene_1");
    }

    public void Quit()
    {
        //Quit the game
        Application.Quit();
    }
}
