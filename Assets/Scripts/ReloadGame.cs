using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadGame : MonoBehaviour {

    public void Reload()
    {
        SceneManager.LoadScene("Main_Scene_1");
    }
}
