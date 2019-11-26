using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoClip vinto;
    public VideoClip perso;
    public VideoPlayer video;
    void Start() {
        if (GameManager.vinto) {
            video.clip = vinto;
        }
        else {
            video.clip = perso;
        }

        if(!video.isPlaying) {
            video.Play();
        }
    }
}
