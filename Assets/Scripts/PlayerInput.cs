using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInput : NetworkBehaviour
{
    GameManagerScript gameMan;
    
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);

    void Start()
    {
        gameMan = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(!isReady.Value)
            {
                isReady.Value = true;
                gameMan.CheckPlayersReady();
                print("Ready");
            }
            else
            {
                isReady.Value = false;
                print("Unready");
            }
        }
    }
}
