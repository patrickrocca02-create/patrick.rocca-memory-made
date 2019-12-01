using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tessera : MonoBehaviour
{
    public int i;
    public int ID;
    public bool girata;
    public bool indovinata;
    
    public GameManager gameManager;
    public Animation anim;
    public Button buttonTorso;
    public Button buttonFaccia;

    public bool nome = false;
    public void Start()
    {
        //acquisizione di tutti parametri
        indovinata = false;
        girata = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animation>();
        buttonTorso = GetComponentsInChildren<Button>()[0].GetComponent<Button>();
        buttonFaccia = GetComponentsInChildren<Button>()[1].GetComponent<Button>();
    }

    public void onClick()
    {
        gameManager.giraTessera(this.GetComponent<tessera>());
    }
    public void onClickGirata()
    {
        if (!anim.isPlaying && (this.indovinata == true || (this.indovinata == false && gameManager.nGirate == 1)))
        {
            anim.Play("clickTessera");
        }
    }

    public bool equals(tessera tessera)
    {
        return tessera.ID == this.ID;
    }

}
