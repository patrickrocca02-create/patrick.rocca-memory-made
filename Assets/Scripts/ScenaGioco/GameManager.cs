using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Timer timer;
    //canvas
    public Canvas canvas;

    public static bool vinto = false;

    //griglie e tessere
    public GameObject prefabPos;
    public GameObject[] posizioni = new GameObject[12];
    public GameObject grigliaPos;

    public GameObject prefabTessera;
    public GameObject[] tessere = new GameObject[12];
    public GameObject grigliaTessere;

    //nomi cartelle per il caricamento delle immagini delle tessere
    public string[] cartelleImmaginiLVL = new string[4];

    //popup
    public Image popUpSpiegazione;

    //contatore copie indovinate 
    private int nCoppie;
    private int nCoppieIndovinate;

    //variabile che conta il numero di carte girate
    public int nGirate = 0;

    //prima tessera girata
    private tessera tessera1;

    //bottone mescolamento
    public Button bottoneMescola;

    //nomi animazioni
    public string animScopri;
    public string animCopri;

    //contatore mosse
    public contatore contatoreMosse;

    float secondiSbagliate;

    private void Update()
    {
        //Debug.Log((tessere[0].transform.localPosition.x + 700) / 1400 + " " + (tessere[0].transform.localPosition.y + 310)/700);
    }
    void Start()
    {
        nCoppieIndovinate = 0;
        nCoppie = tessere.Length / 2;
        nGirate = 0;

        creaTessereNoGrid();
        assegnaTextureTessere(LevelManager.level);

        //contatore
        contatoreMosse.setAbilitato(false);
        if (LevelManager.level == 4)
        {
            contatoreMosse.setAbilitato(true);
        }
        if (LevelManager.level < 3) {
            secondiSbagliate = 1.0f;
        }
        else {
            secondiSbagliate = 2.0f;
        }
    }

    public void assegnaTextureTessere(int lvl)
    {
        Sprite[] textureFrontali = Resources.LoadAll<Sprite>(cartelleImmaginiLVL[lvl-1] + "/tessere");
        Sprite[] nomi = Resources.LoadAll<Sprite>(cartelleImmaginiLVL[lvl - 1] + "/nomi");
        Sprite[] dietro = Resources.LoadAll<Sprite>("gioco/schiene");
        for (int i = 0; i < tessere.Length; i++) {
            tessere[i].GetComponentInChildren<Animator>().gameObject.GetComponent<Image>().sprite = nomi[tessere[i].GetComponent<tessera>().ID];
            tessere[i].GetComponentsInChildren<Button>()[0].GetComponent<Image>().sprite = dietro[lvl - 1];
        }
        if (lvl < 3)
        {
            foreach (GameObject tessera in tessere)
            {
                tessera.GetComponentsInChildren<Button>()[1].GetComponent<Image>().sprite = textureFrontali[tessera.GetComponent<tessera>().ID];
            }
        }
        else 
        {
            for(int i = 0; i < tessere.Length; i++)
            {
                tessere[i].GetComponentsInChildren<Button>()[1].GetComponent<Image>().sprite = textureFrontali[i];
            }
            if (lvl == 4)
            {
                for (int i = 0; i < tessere.Length; i += 2) {
                    tessere[i].GetComponentInChildren<Animator>().gameObject.GetComponent<Image>().sprite = nomi[tessere[i].GetComponent<tessera>().ID];
                }
            }
        }

    }

    public void creaTessereNoGrid()
    {
        int contaID = 0;

        //creazione posizioni
        for (int i = 0; i < posizioni.Length; i++)
        {
            posizioni[i] = Instantiate(prefabPos, grigliaPos.GetComponent<Transform>());
        }
        //creazione tessere
        for(int i = 0; i < posizioni.Length; i++)
        {
            //creazione tessara
            tessere[i] = Instantiate(prefabTessera, grigliaTessere.GetComponent<Transform>());
            tessere[i].GetComponent<tessera>().i = i;
            //assegnazione ID
            tessere[i].GetComponent<tessera>().ID = contaID;
            if (i % 2 == 1)
            {
                contaID++;
            }
        }
        //coroutine per l'assegnazione ritardata delle posizioni
        StartCoroutine(assegnaPosizione());
    }

    public void creaTessere()
    {
        int contaID = 0;
        for (int i = 0; i < tessere.Length; i++)
        {
            tessere[i] = Instantiate(prefabTessera, grigliaTessere.GetComponent<Transform>());
            tessere[i].GetComponent<tessera>().ID = contaID;
            if (i % 2 == 1)
            {
                contaID++;
            }
        }
    }

    public void giraTessera(tessera tessera)
    {
        if (!tessera.girata && nGirate != 2)
        {
            //disabilità il bottone
            tessera.buttonTorso.interactable = false;
            tessera.buttonFaccia.interactable = true;
            
            tessera.anim.Play(animScopri);

            tessera.girata = true;

            if (nGirate == 0)
            {
                nGirate = 1;
                tessera1 = tessera;
                tessera1.girata = true;
            }
            else if (nGirate == 1)
            {
                nGirate = 2;
                if (tessera != tessera1 && tessera.equals(tessera1))
                {
                    //animazione in cui appare il testo
                    //Vector3 pos1 = tessera.gameObject.transform.localPosition;
                    //Vector3 pos2 = tessera1.gameObject.transform.localPosition;
                    //tessera1.GetComponent<RippleEffect>().Emit((pos1.x + 700) / 1400, (pos1.y + 310) / 700);
                    //tessera.GetComponent<RippleEffect>().Emit((pos2.x + 700) / 1400, (pos2.y + 310) / 700);
                    nGirate = 0;
                    tessera.indovinata = true;
                    tessera1.indovinata = true;
                    nCoppieIndovinate++;
                    StartCoroutine(Wait(tessera, tessera1));
                    if (nCoppieIndovinate == nCoppie)
                    {
                        vinto = true;
                        timer.StartTimer();
                    }
                }
                else if(tessera != tessera1)
                { 
                    StartCoroutine(playAndWaitForAnim(tessera));
                }
            }
        }
    }
    IEnumerator Wait(tessera tessera, tessera tessera1) {
        yield return new WaitForSeconds(tessera.anim[animScopri].length);
        tessera.GetComponentInChildren<Animator>().SetBool("Indovinata",true);
        tessera1.GetComponentInChildren<Animator>().SetBool("Indovinata",true);
        StopCoroutine(Wait(tessera, tessera1));
    }

    IEnumerator playAndWaitForAnim(tessera tessera2) {
        
        if (contatoreMosse.abilitato) {
            yield return new WaitForSeconds(tessere[0].GetComponent<tessera>().anim.GetClip("scopri").length);
            contatoreMosse.mossaErrata();
        }
        yield return new WaitForSeconds(secondiSbagliate);

        tessera1.anim.Play(animCopri);
        tessera2.anim.Play(animCopri);

        tessera1.girata = false;
        tessera2.girata = false;

        //riattiva i bottoni delle facce delle tessere
        tessera1.buttonTorso.interactable = true;
        tessera2.buttonTorso.interactable = true;
        //disabilita quelli sul torso
        tessera1.buttonFaccia.interactable = false;
        tessera2.buttonFaccia.interactable = false;
        //reset tessera1
        tessera1 = null;

        nGirate = 0;
        if (contatoreMosse.contatoreAzzerato)
        {
            foreach (GameObject t in tessere)
            {
                t.GetComponent<tessera>().buttonFaccia.interactable = false;
                t.GetComponent<tessera>().buttonTorso.interactable = false;
            }
            yield return new WaitForSeconds(tessere[0].GetComponent<tessera>().anim.GetClip(animCopri).length);
            StartCoroutine(Sconfitta());
        }

    }

    IEnumerator assegnaPosizione()
    {
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < posizioni.Length; i++)
        {
            tessere[i].GetComponent<RectTransform>().localPosition = posizioni[i].GetComponent<RectTransform>().localPosition;
        }
        
    }
    
    IEnumerator mescolamento()
    {
        bool trovata = false;
        for (int i = 0; i < tessere.Length; i++)
        {
            if (tessere[i].GetComponent<tessera>().girata)
            {
                trovata = true;
                tessere[i].GetComponent<Animation>().Play("copri");
                tessere[i].GetComponent<tessera>().girata = false;
            }
        }
        if (trovata)
        {
            yield return new WaitForSeconds(tessere[0].GetComponent<Animation>().GetClip("copri").length);
        }
        int[] iRandom = indiciRandom(12);
        int[] iRandom2 = indiciRandom(12);
        for (int i = 0; i < tessere.Length; i++)
        {
            StartCoroutine(muoviTessera(i, iRandom, false));
            StartCoroutine(muoviTessera(i, iRandom2, true));
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.15f);
        setBottoniTessere(true);
    }

    IEnumerator muoviTessera(int i, int[] iRandom, bool wait)
    {
        if (wait)
        {
            yield return new WaitForSeconds(0.6f);
        }

        Animation anim = tessere[i].GetComponent<Animation>();
        if (anim.GetClip("muoviAlCentro") != null) {
            anim.RemoveClip("muoviAlCentro");
        }
        
        AnimationCurve curve;
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;

        Keyframe[] keysX = new Keyframe[2];
        Keyframe[] keysY = new Keyframe[2];

        keysX[0] = new Keyframe(0.0f, tessere[i].GetComponent<RectTransform>().localPosition.x);
        keysX[1] = new Keyframe(0.6f, posizioni[iRandom[i]].GetComponent<RectTransform>().localPosition.x);
        keysY[0] = new Keyframe(0.0f, tessere[i].GetComponent<RectTransform>().localPosition.y);
        keysY[1] = new Keyframe(0.6f, posizioni[iRandom[i]].GetComponent<RectTransform>().localPosition.y);

        curve = new AnimationCurve(keysX);
        clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.x", curve);
        curve = new AnimationCurve(keysY);
        clip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve);

        anim.AddClip(clip, "muoviAlCentro");

        anim.Play("muoviAlCentro");
    }

    public void mescola()
    {
        bool animInCorso = false;
        for (int i = 0; i < tessere.Length; i++)
        {
            if(tessere[i].GetComponent<tessera>().anim.isPlaying)
            {
                animInCorso = true;
                break;
            }
        }
        if (!animInCorso)
        {
            for(int i = 0; i < tessere.Length; i++)
            {
                tessere[i].GetComponent<tessera>().indovinata = false;
                Animator anim = tessere[i].GetComponentInChildren<Animator>();
                if (anim.GetBool("Indovinata"))
                {
                    anim.SetBool("Indovinata", false);
                }
            }
            //rigirare le coppie indovinate
            setBottoniTessere(false);
            nCoppieIndovinate = 0;
            nGirate = 0;
            if (contatoreMosse.abilitato)
            {
                contatoreMosse.reset();
            }
            StartCoroutine(mescolamento());
        }
        else { Debug.Log("anim in corso"); }
    }
    public void avviaMescolamentoIniziale()
    {
        bottoneMescola.interactable = false;
        setBottoniTessere(false);
        StartCoroutine(mescolamentoIniziale());
    }

    IEnumerator mescolamentoIniziale()
    {
        for (int i = 0; i < tessere.Length; i++)
        {
            tessere[i].GetComponent<Animation>().Play("scopri");
        }
        yield return new WaitForSeconds(2);
        for(int i = 0; i < tessere.Length; i++)
        {
            tessere[i].GetComponent<Animation>().Play("copri");
        }
        yield return new WaitForSeconds(tessere[0].GetComponent<Animation>().GetClip("copri").length + 0.05f);
        bottoneMescola.interactable = true;
        mescola();
    }

    int[] indiciRandom(int length)
    {
        int[] indici = new int[length];
        int indiceRandom;

        bool[] usato = new bool[length];

        for (int i = 0; i < length; i++)
        {
            usato[i] = false;
        }

        for (int i = 0; i < length; i++)
        {
            do
            {
                indiceRandom = (int)Random.Range(0, length);
            } while (usato[indiceRandom]);

            usato[indiceRandom] = true;
            indici[i] = indiceRandom;
        }
        return indici;
    }

    public void setBottoniTessere(bool attiva)
    {
        for(int i = 0; i < tessere.Length; i++)
        {
            tessere[i].GetComponent<tessera>().buttonTorso.interactable = attiva;
        }
    }

    IEnumerator Sconfitta()
    {
        timer.ResetTimer();
        int index = 0;
        int coppieRimaste = nCoppie - nCoppieIndovinate;
        List<tessera> coppieDaGirare = new List<tessera>();
        while (coppieRimaste > 0 && index < tessere.Length)
        {
            tessera t1 = tessere[index].GetComponent<tessera>();
            if (!t1.girata)
            {
                bool coppiaTrovata = false;
                int i = index + 1;
                while (!coppiaTrovata && i < tessere.Length)
                {
                    tessera t2 = tessere[i].GetComponent<tessera>();
                    if (t2.equals(t1))
                    {
                        coppieDaGirare.Add(t1);
                        coppieDaGirare.Add(t2);
                        coppiaTrovata = true;
                    }
                    i++;
                }
            }
            index++;
        }
        for (int i = 0; i < coppieDaGirare.Count; i += 2)
        {
            coppieDaGirare[i].anim.Play(animScopri);
            coppieDaGirare[i + 1].anim.Play(animScopri);
            yield return new WaitForSeconds(tessere[0].GetComponent<tessera>().anim.GetClip("scopri").length);
            coppieDaGirare[i].GetComponentInChildren<Animator>().SetTrigger("Indovinata");
            coppieDaGirare[i + 1].GetComponentInChildren<Animator>().SetTrigger("Indovinata");
            yield return new WaitForSeconds(2.0f);
        }
        timer.StartTimer();
        StopCoroutine(Sconfitta());
    }
}

