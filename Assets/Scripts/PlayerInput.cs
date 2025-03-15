using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    GameManagerScript gameMan;
    
    bool isReady = false;

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
                gameMan.AddPlayerReady();
                print("Somethings happening here");
            }
            else
            {
                isReady = false;
                gameMan.RemovePlayerReady();
                print("Somethings happening heree");
            }
        }
    }
}
