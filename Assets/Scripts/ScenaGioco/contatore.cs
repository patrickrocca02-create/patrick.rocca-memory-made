using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class contatore : MonoBehaviour
{
    public Sprite usata;
    public Sprite nonUsata;
    public Timer timer;

    public GameManager gameManager;
    public GameObject prefabMossa;

    public int mosseMassime;
    int nMosseUsate;

    public bool abilitato;

    public bool contatoreAzzerato = false;

    Image[] mosse;
    void Start()
    {
        calcolaWidth();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nMosseUsate = 0;

        mosse = new Image[mosseMassime];
        for(int i = 0; i < mosseMassime; i++)
        {
            mosse[i] = Instantiate(prefabMossa, this.GetComponent<Transform>()).GetComponent<Image>();
            mosse[i].sprite = nonUsata;
        }
    }

    public void reset()
    {
        for(int i = 0; i < nMosseUsate; i++)
        {
            mosse[i].sprite = nonUsata;
        }
        nMosseUsate = 0;
    }

    public void mossaErrata()
    {
        mosse[nMosseUsate++].sprite = usata;
        if (nMosseUsate == mosseMassime)
        {
            contatoreAzzerato = true;
        }
    }

    public void setAbilitato(bool abilita)
    {
        abilitato = abilita;
        gameObject.SetActive(abilita);
    }

    private void calcolaWidth()
    {
        GridLayoutGroup griglia = this.GetComponent<GridLayoutGroup>();
        float width = griglia.spacing.x * (mosseMassime + 1) + griglia.cellSize.x * mosseMassime;
        RectTransform rect = this.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.rect.height);
    }

    IEnumerator animazioneSconfitta() {
        yield return new WaitForSeconds(gameManager.tessere[0].GetComponent<tessera>().anim.GetClip(gameManager.animCopri).length);
        //animazione scopri tessere 2 a 2
        StopCoroutine(animazioneSconfitta());
    }
}
