using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    GameManagerScript gameMan;
    
    public bool isReady = false;

    void Start()
    {
        gameMan = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(!isReady)
            {
                isReady = true;
                gameMan.CheckPlayersReady();
                print("Ready");
            }
            else
            {
                isReady = false;
                print("Unready");
            }
        }
    }
}
