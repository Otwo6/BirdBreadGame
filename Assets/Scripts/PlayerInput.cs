using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInput : NetworkBehaviour
{
    GameManagerScript gameMan;
    
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>();

    void Start()
    {
        gameMan = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();

        if(IsOwner)
        {
            isReady.Value = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(IsOwner)
            {
                if(isReady.Value)
                {
                    isReady.Value = false;
                }
                else
                {
                    isReady.Value = true;
                }
                
            }
        }
    }
}
