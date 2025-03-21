using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerInput : NetworkBehaviour
{
    GameManagerScript gameMan;
    
    public NetworkVariable<bool> isReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
                isReady.Value = !isReady.Value;
                GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>().CheckPlayersReady();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetReadyServerRpc(bool has)
    {
        isReady.Value = has;
        SetReadyClientRpc(has);
    }

    [ClientRpc]
    void SetReadyClientRpc(bool has)
    {
        isReady.Value = has;
    }
}
