using UnityEngine;

public class General : MonoBehaviour {

    public Timer timer;

    public bool run = false;

    private void Update() {
        if (run) {
            if (Input.GetMouseButtonDown(0)) {
                timer.ResetTimer();
            }
            else {
                timer.StartTimer();
            }
        }
    }

    public void Run() {
        run = true;
    }

    public void Stop() {
        run = false;
    }
}
