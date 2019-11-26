using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caricaGameManager : MonoBehaviour
{
    public GameObject gameManager;
    void Start()
    {
        Instantiate(gameManager);
    }
}
