using System.Collections;
using UnityEngine;


public class Timer : MonoBehaviour {

    public LevelManager levelManager;
    public float seconds;
    public int level;

    public void StartTimer() {
        StartCoroutine(Delay());
    }

    IEnumerator Delay() {
        yield return new WaitForSeconds(seconds);
        levelManager.SceneTransition(level);
    }

    public void ResetTimer() {
        StopAllCoroutines();
    }
}
