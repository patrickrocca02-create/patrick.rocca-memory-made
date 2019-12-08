using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoClip vinto;
    public VideoClip perso;
    public VideoPlayer video;
    public Button returnButton;
    public Button screenButton;

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
        
        returnButton.GetComponent<Animation>().Play("ComparsaBottone");
        yield return new WaitForSeconds(returnButton.GetComponent<Animation>().GetClip("ComparsaBottone").length);
        returnButton.interactable = true;
        screenButton.interactable = true;
        StopCoroutine(ShowButton());
    }
}
