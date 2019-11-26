using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static int level = 1;

    public Animator anim;
    public void SceneTransition(int level) {
        LevelManager.level = level;
        anim.SetTrigger("CambioScena");
    }

    public void LoadScene() {
        int buildIndex;
        if (level > 0 && level <= 4) {
            buildIndex = 2;
        }
        else if (level < 0) {
            buildIndex = 0;
        }
        else if (level == 0){
            buildIndex = 1;
        }
        else {
            buildIndex = 3;
        }
        SceneManager.LoadScene(buildIndex);
    }
}