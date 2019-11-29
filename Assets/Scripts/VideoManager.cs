using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoClip vinto;
    public VideoClip perso;
    public VideoPlayer video;
    public GameObject returnButton;
    public AnimationClip clip;

    private void Awake() {
        if (GameManager.vinto) {
            video.clip = vinto;
        }
        else {
            video.clip = perso;
        }

        StartCoroutine(ShowButton());
        
    }

    IEnumerator ShowButton() {
        yield return new WaitForSeconds((float) video.clip.length);
        video.Pause();
        
        clip.legacy = true;
        returnButton.GetComponent<Animation>().Play("ComparsaBottone");
        StopCoroutine(ShowButton());
    }
}
