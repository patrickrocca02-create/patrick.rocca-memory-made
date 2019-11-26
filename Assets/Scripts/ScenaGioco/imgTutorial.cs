using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imgTutorial : MonoBehaviour
{
    public Sprite[] imgsTutorial = new Sprite[4];
    void Start()
    {
        this.GetComponent<Image>().sprite = imgsTutorial[LevelManager.level - 1];
    }
}
