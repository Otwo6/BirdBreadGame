using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInput : NetworkBehaviour
{
    public GameManagerScript gameMan;
    
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false);

    void Start()
    {
        gameMan = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        if(gameMan == null)
        {
            LookForGameMan();
        }
    }

    void LookForGameMan()
    {
        gameMan = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        if(gameMan == null)
        {
            LookForGameMan();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (IsServer)
            {
                ToggleReadyState();
            }
            else
            {
                RequestReadyStateToggleServerRpc();
            }
        }
    }

    [ServerRpc]
    void RequestReadyStateToggleServerRpc(ServerRpcParams rpcParams = default)
    {
        ToggleReadyState();
    }

    void ToggleReadyState()
    {
        if (!isReady.Value)
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
