using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popUp : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject pop_up;
    private Animation anim;
    public string nomeAnim;

    public Button rimescola;
    public Button tornaAlMenu;

    public Sprite[] textures = new Sprite[4];

    private void Start()
    {
        pop_up = gameObject;
        anim = GetComponent<Animation>();
        nomeAnim = anim.clip.name;

        this.GetComponent<Image>().sprite = textures[LevelManager.level - 1];

        foreach (GameObject tessera in gameManager.tessere)
        {
            tessera.GetComponentsInChildren<Button>()[0].interactable = false;
        }
        rimescola.interactable = false;
        tornaAlMenu.interactable = true;
    }
    public void onClick()
    {
        anim.Play(nomeAnim);
        StartCoroutine(disattiva());
    }

    IEnumerator disattiva()
    {
        yield return new WaitForSeconds(anim.clip.averageDuration);

        //Abilita i bottoni sui torsi delle tessere
        foreach (GameObject tessera in gameManager.tessere)
        {
            tessera.GetComponentsInChildren<Button>()[0].interactable = true;
        }

        //abilita i bottoni di rimescolamento e di ritorno al menu
        tornaAlMenu.interactable = true;

        yield return new WaitForSeconds(0.2f);
        gameManager.avviaMescolamentoIniziale();
        pop_up.SetActive(false);
    }
}
